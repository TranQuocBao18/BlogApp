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
import type { Blog } from '@/interfaces/blog';
import type { IBlogLikeForm } from '@/interfaces/like';

/**
 * Blog API
 */
export const apiGetBlogs = (
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('get', `/Blog`, {
    pageNumber,
    pageSize,
  });

export const apiGetBlogsByCategory = (
  categoryId: string,
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('get', `/Blog/${categoryId}`, {
    pageNumber,
    pageSize,
  });

export const apiGetBlogBySlug = (slug: string) =>
  request('get', `/Blog/${slug}`);

export const apiSearchBlogs = (
  search: string,
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('post', `/Blog/search`, {
    search,
    pageNumber,
    pageSize,
  });

export const apiSearchBlogsByCategory = (
  categoryId: string,
  search: string,
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('post', `/Blog/${categoryId}/search`, {
    search,
    pageNumber,
    pageSize,
  });

// Admin endpoints
export const apiGetAllBlogs = (
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('get', `/Blog/admin`, {
    pageNumber,
    pageSize,
  });

export const apiSearchAllBlogs = (
  search: string,
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('post', `/Blog/admin/search`, {
    search,
    pageNumber,
    pageSize,
  });

export const apiCreateBlog = (data: Blog) =>
  request('post', `/Blog`, {
    payload: data,
  });

export const apiUpdateBlog = (data: Blog) =>
  request('put', `/Blog`, {
    payload: data,
  });

export const apiDeleteBlog = (id: string) =>
  request('delete', `/Blog/${id}`);

// Blog Like API
export const apiToggleBlogLike = (data: IBlogLikeForm) =>
  request('post', `/BlogLike`, {
    payload: data,
  });
