/**
 * @copyright 2025 Tran Bao
 * @license Apache-2.0
 */

export interface BlogLike {
  id: string;
  blogId: string;
  userId: string;
  isDeleted: boolean;
  createdBy?: string;
  created: string;
  lastModifiedBy?: string;
  lastModified?: string;
}

export interface BlogLikeResponse {
  id: string;
  blogId: string;
}

export interface IBlogLikeForm {
  blogId: string;
}
