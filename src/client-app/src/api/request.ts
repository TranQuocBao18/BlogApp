/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import axios from 'axios';
import registerAxiosTokenRefresh from 'axios-token-refresh';

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
