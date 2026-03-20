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
import type { Tag } from '@/interfaces/tag';

export const apiGetTags = (
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('get', `/Tag`, {
    pageNumber,
    pageSize,
  });

export const apiSearchTags = (
  search: string,
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('post', `/Tag/search`, {
    search,
    pageNumber,
    pageSize,
  });

export const apiCreateTag = (data: Tag) =>
  request('post', `/Tag`, {
    payload: data,
  });

export const apiUpdateTag = (data: Tag) =>
  request('put', `/Tag`, {
    payload: data,
  });

export const apiDeleteTag = (id: string) =>
  request('delete', `/Tag/${id}`);
