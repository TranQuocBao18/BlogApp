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
import type { Comment } from '@/interfaces/comment';

export const apiGetCommentsByBlogId = (
  blogId: string,
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('get', `/Comment/${blogId}`, {
    pageNumber,
    pageSize,
  });

export const apiGetCommentReplies = (
  parentId: string,
  blogId: string,
  pageNumber: number = 1,
  pageSize: number = 10,
) =>
  request('get', `/Comment/replies`, {
    parentId,
    blogId,
    pageNumber,
    pageSize,
  });

export const apiGetComments = (pageNumber: number = 1, pageSize: number = 10) =>
  request('get', `/Comment`, {
    pageNumber,
    pageSize,
  });

export const apiCreateComment = (data: Comment) =>
  request('post', `/Comment`, {
    payload: data,
  });

export const apiUpdateComment = (data: Comment) =>
  request('put', `/Comment`, {
    payload: data,
  });

export const apiDeleteComment = (id: string) =>
  request('delete', `/Comment/${id}`);
