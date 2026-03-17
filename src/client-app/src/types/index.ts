/**
 * @copyright 2025 Tran Bao
 * @license Apache-2.0
 */

export interface User {
  id: string;
  username?: string;
  fullName?: string;
  email?: string;
  phoneNumber?: string;
  isAdmin: boolean;
  created: string;
  lastModified?: string;
}

export interface Blog {
  id: string;
  title: string;
  slug?: string;
  content: string;
  banner?: Banner;
  category?: Category;
  tags: Tag[];
  likeCount?: number;
  isLikeByCurrentUser?: boolean;
  commentCount: number;
  status: 'draft' | 'published' | string;
}

export interface Comment {
  id: string;
  content: string;
  userId?: string;
  blogId: string;
  parentId?: string;
  user?: User | null;
  likeCount?: number;
  isLikeByCurrentUser?: boolean;
  childComments?: Comment[];
  replyCount?: number;
  created: string;
}

export interface Category {
  id?: string;
  name: string;
  slug?: string;
}

export interface Tag {
  name: string;
  slug?: string;
}

export interface Banner {
  id?: string;
  publicId: string;
  url: string;
  width: number;
  height: number;
}

export type PaginatedResponse<T, K extends string> = {
  limit: number;
  offset: number;
  total: number;
} & {
  [key in K]: T[];
};

export type FieldValidationError = {
  /**
   * Indicates that the error occurred because a field had an invalid value
   */
  type: 'field';
  /**
   * The location within the request where this field is
   */
  location: Location;
  /**
   * The path to the field which has a validation error
   */
  path: string;
  /**
   * The value of the field. It might be unset if the value is hidden.
   */
  value?: string;
  /**
   * The error message
   */
  msg: string;
};

export type ErrorCode =
  | 'BadRequest'
  | 'ValidationError'
  | 'AuthenticationError'
  | 'AuthorizationError'
  | 'NotFound'
  | 'ServerError';

export type ValidationError = {
  code: ErrorCode;
  errors: Record<string, FieldValidationError>;
};

export type ErrorResponse = {
  code: ErrorCode;
  message: string;
};

export interface ActionResponse<T = unknown> {
  ok: boolean;
  err?: ValidationError | ErrorResponse;
  data?: T;
}

export interface AuthResponse {
  id: string;
  userName: string;
  displayName: string;
  email: string;
  groupCode: string;
  roles: string[];
  isVerified: boolean;
  jwToken: string;
}

export interface ProfileResponse {
  id: string;
  userName?: string;
  displayName?: string;
  email?: string;
  fullName?: string;
  phoneNumber?: string;
  roles?: string[];
}

export interface BlogCreateResponse {
  blog: Blog;
}

export interface SignupResponse {
  userId: string;
}

export interface ApiResponse {
  Succeeded: boolean;
  Message: string;
  ErrorCode: number;
}
