/**
 * @copyright 2025 Tran Bao
 * @license Apache-2.0
 */

/**
 * Main Response Wrapper - Matches Backend Response<T> Pattern
 * Backend always returns 200 OK, success indicated in Succeeded field
 */
export interface Response<T = unknown> {
  succeeded: boolean;
  message: string;
  errorCode?: string;
  errors?: string[];
  data?: T;
}

/**
 * Paginated Response - Matches Backend PagedResponse<T> Pattern
 * Extends Response with pagination metadata
 * data is always an array of T items
 */
export interface PagedResponse<T = unknown> extends Response<T[]> {
  data: T[];
  pageNumber: number;
  pageSize: number;
  totalItems: number;
}

/**
 * Generic API Response for any endpoint
 * Use this for all backend calls
 */
export type ApiResponse<T = unknown> = Response<T> | PagedResponse<T>;

/**
 * Utility to check if response is success
 */
export const isSuccess = (response: Response): boolean =>
  response.succeeded === true;

/**
 * Utility to check if response is paged
 */
export const isPaged = <T>(
  response: ApiResponse<T>,
): response is PagedResponse<T> => {
  return (
    'pageNumber' in response &&
    'pageSize' in response &&
    'totalItems' in response
  );
};
