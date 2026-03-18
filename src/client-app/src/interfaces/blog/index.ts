/**
 * @copyright 2025 Tran Bao
 * @license Apache-2.0
 */

/**
 * Types
 */
import type { Banner } from '@/interfaces/banner';
import type { Category } from '@/interfaces/category';
import type { Tag } from '@/interfaces/tag';

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

export interface BlogCreateResponse {
  blog: Blog;
}
