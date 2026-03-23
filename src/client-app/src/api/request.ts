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
  statusCodes: [401],
  refreshRequest: async (failedRequest: any) => {
    // handle refresh token logic here
    try {
      const response = await axios.post(
        `${ENV_CONFIG.API}/Account/refresh-token`,
        {},
        { withCredentials: true },
      );

      // Backend returns Response<T> structure
      if (response.data?.succeeded) {
        localStorage.setItem(
          APP_CONFIG.ACCESS_TOKEN,
          response.data.data?.jwToken || '',
        );
        localStorage.setItem(
          'user',
          JSON.stringify({
            username: response.data.data?.userName || '',
            email: response.data.data?.email || '',
            roles: response.data.data?.roles || [],
          }),
        );

        failedRequest.response.config.headers['Authorization'] =
          `Bearer ${response.data.data?.jwToken}`;

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

    if (token && token.trim()) {
      config.headers.set('Authorization', `Bearer ${token.trim()}`);
    }
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
    dataStr.includes('You are not Authorized') || status === 401;
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

    // Return backend Response<T> structure as-is
    return resp?.data;
  },
  (error) => {
    const canRefresh = checkAuthorize(error);

    if (canRefresh) {
      return Promise.reject(error);
    }

    // Extract error message from backend Response<T> structure
    const backendResponse = error?.response?.data;
    const statusCode = error?.response?.status;

    return {
      succeeded: false,
      message:
        backendResponse?.message || error?.message || `Error (${statusCode})`,
      errorCode: backendResponse?.errorCode || error?.code || 'NetworkError',
      errors: backendResponse?.errors || [],
      data: null,
    };
  },
);

/**
 * Main Response Type - Matches Backend Response<T>
 * Backend always returns HTTP 200, success indicated by 'succeeded' field
 */
export type Response<T = any> = {
  succeeded: boolean;
  message: string;
  errorCode?: string;
  errors?: string[];
  data?: T;
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
