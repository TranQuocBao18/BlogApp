/**
 * @copyright 2025 Tran Bao
 * @license Apache-2.0
 */

export interface CommentLike {
  id: string;
  commentId: string;
  userId: string;
  isDeleted: boolean;
  createdBy?: string;
  created: string;
  lastModifiedBy?: string;
  lastModified?: string;
}

export interface CommentLikeResponse {
  id: string;
  commentId: string;
}

export interface ICommentLikeForm {
  commentId: string;
}
