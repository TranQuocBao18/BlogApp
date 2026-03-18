/**
 * @copyright 2025 Tran Bao
 * @license Apache-2.0
 */

/**
 * Types
 */
import type { User } from '@/interfaces/user';

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
