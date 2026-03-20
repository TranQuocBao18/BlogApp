/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Constants
 */

/**
 * Custom modules
 */
import { request } from '@/api/request';

/**
 * Types
 */
import type { Category } from '@/interfaces/category';
export const apiGetCategories = (
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('get', `/Category`, {
    pageNumber,
    pageSize,
  });

export const apiSearchCategories = (
  search: string,
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('post', `/Category/search`, {
    search,
    pageNumber,
    pageSize,
  });

export const apiCreateCategory = (data: Category) =>
  request('post', `/Category`, {
    payload: data,
  });

export const apiUpdateCategory = (data: Category) =>
  request('put', `/Category`, {
    payload: data,
  });

export const apiDeleteCategory = (id: string) =>
  request('delete', `/Category/${id}`);