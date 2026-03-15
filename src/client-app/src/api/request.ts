/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import axios from 'axios';
import registerAxiosTokenRefresh from 'axios-token-refresh';
import { toast } from 'sonner';

/**
 * Constants
 */
import { APP_CONFIG, ENV_CONFIG } from '@/constants/global';
import { AppRouters } from '@/constants/router';

/**
 * Types
 */
import type {
  AxiosRequestConfig,
  InternalAxiosRequestConfig,
  Method,
} from 'axios';

export interface RequestConfig extends AxiosRequestConfig {
  loading?: boolean;
}

export type ParamsGetList = {
  pageNumber: number;
  pageSize: number;
  search?: string;
  searchValue?: string;
  filters?: { key: string; value: string[] }[];
  range?: { from?: string; to?: string };
  filter?: {
    searchValue?: string;
    range?: { from?: string; to?: string };
  };
  debtDocumentId?: string;
  customerId?: string;
};

const axiosInstance = axios.create({
  baseURL: `${ENV_CONFIG.API}/${ENV_CONFIG.API_VERSION}`,
  headers: {
    Accept: 'application/json, text/plain, */*',
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*',
  },
  withCredentials: true,
});
registerAxiosTokenRefresh(axiosInstance, {
  statusCodes: [401, 403],
  refreshRequest: async (failedRequest: any) => {
    // handle refresh token logic here
    try {
      const response = await axios.post(
        `${ENV_CONFIG.API}/${ENV_CONFIG.API_VERSION}/Account/refresh-token`,
        {},
        { withCredentials: true },
      );

      if (response.data.succeeded) {
        localStorage.setItem(
          APP_CONFIG.ACCESS_TOKEN,
          response.data.data.jwToken,
        );
        localStorage.setItem(
          'user',
          JSON.stringify(response.data.data.userName),
        );

        failedRequest.response.config.header['Authorization'] =
          `Bearer ${response.data.data.jwToken}`;

        return Promise.resolve();
      }
    } catch (err) {
      window.localStorage.clear();
      window.location.href = AppRouters.LOGIN;
      return Promise.reject(err);
    }
  },
});

axiosInstance.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = localStorage.getItem(APP_CONFIG.ACCESS_TOKEN);

    config.headers.set('Authorization', `Bearer ${token?.trim()}`);
    return config;
  },
  (error) => {
    return Promise.reject(error);
  },
);

const getCookie = (name: string) => {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) {
    return parts.pop()?.split(';').shift();
  }
  return null;
};

const checkAuthorize = (error: any) => {
  const status = error?.response?.status || error?.status;
  const dataStr = String(error?.response?.data || error?.data);

  const isAuthError =
    dataStr.includes('You are not Authorized') ||
    status === 403 ||
    status === 401;
  if (isAuthError) {
    const hasRefreshToken = getCookie('REFRESH_TOKEN');

    if (!hasRefreshToken) {
      toast.error('Your login session has expired. Please log in again.');
      window.localStorage.clear();
      setTimeout(() => {
        window.location.href = AppRouters.LOGIN;
      }, 2000);

      return false;
    }
    return true;
  }
  return false;
};

axiosInstance.interceptors.response.use(
  (resp) => {
    if (String(resp?.data).includes('You are not Authorized')) {
      const canRefresh = checkAuthorize(resp);
      if (canRefresh) {
        const customAuthError: any = new Error('Custom Unauthorized');
        customAuthError.response = { status: 401, data: resp.data };
        customAuthError.config = resp.config;

        return Promise.reject(customAuthError);
      }
    }

    return resp?.data;
  },
  (error) => {
    const canRefresh = checkAuthorize(error);

    if (canRefresh) {
      return Promise.reject(error);
    }

    return {
      status: false,
      message: error?.message,
      result: null,
    };
  },
);

export type Response<T = any> = {
  status: boolean;
  message: string;
  result: T;
  data?: T;
  [key: string]: any;
};

export type MyResponse<T = any> = Promise<Response<T>>;

export const request = <T = any>(
  method: Lowercase<Method>,
  url: string,
  data?: any,
  config?: RequestConfig,
): MyResponse<T> => {
  // const prefix = '/api'
  const prefix = '';

  url = prefix + url;

  if (method === 'post') {
    return axiosInstance.post(url, data, config);
  } else if (method === 'put') {
    return axiosInstance.put(url, data, config);
  } else if (method === 'delete') {
    return axiosInstance.delete(url, config);
  } else {
    return axiosInstance.get(url, {
      params: data,
      ...config,
    });
  }
};
