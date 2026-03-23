/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { data, redirect } from 'react-router';

/**
 * Custom modules
 */
import { apiGetAllBlogs } from '@/api/blog.api';
import { apiGetComments } from '@/api/comment.api';
import { apiGetUsers } from '@/api/user.api';
import { APP_CONFIG } from '@/constants/global';

/**
 * Types
 */
import type { Blog } from '@/interfaces/blog';
import type { Comment } from '@/interfaces/comment';
import type { User } from '@/interfaces/user';
import type { PagedResponse, Response } from '@/types';
import { AxiosError } from 'axios';
import type { LoaderFunction } from 'react-router';

export interface DashboardData {
  blogs: Blog[];
  users: User[];
  comments: Comment[];
  blogsCount: number;
  usersCount: number;
  commentsCount: number;
}

const dashboardLoader: LoaderFunction = async () => {
  const accessToken = localStorage.getItem(APP_CONFIG.ACCESS_TOKEN);
  if (!accessToken) {
    return redirect('/');
  }

  try {
    // Fetch stats data in parallel
    const [blogsResponse, usersResponse, commentsResponse] = await Promise.all([
      apiGetAllBlogs(1, 10).catch((err) => {
        console.error('Failed to fetch blogs:', err);
        return null;
      }),
      apiGetUsers(1, 10).catch((err) => {
        console.error('Failed to fetch users:', err);
        return null;
      }),
      apiGetComments(1, 10).catch((err) => {
        console.error('Failed to fetch comments:', err);
        return null;
      }),
    ]);

    // Parse responses using existing types
    const blogsData = blogsResponse as PagedResponse<Blog> | null;
    const usersData = usersResponse as PagedResponse<User> | null;
    const commentsData = commentsResponse as PagedResponse<Comment> | null;

    const blogs = blogsData?.succeeded ? blogsData.data || [] : [];
    const users = usersData?.succeeded ? usersData.data || [] : [];
    const comments = commentsData?.succeeded ? commentsData.data || [] : [];
    const blogsCount = blogsData?.succeeded ? blogsData.data.length || 0 : 0;
    const usersCount = usersData?.succeeded ? usersData.data.length || 0 : 0;
    const commentsCount = commentsData?.succeeded
      ? commentsData.data.length || 0
      : 0;

    return {
      blogs,
      users,
      comments,
      blogsCount,
      usersCount,
      commentsCount,
    } as DashboardData;
  } catch (err) {
    if (err instanceof AxiosError) {
      const errorData = err.response?.data as Response;
      throw data(
        errorData?.message || err.message || 'Failed to load dashboard data',
        {
          status: err.response?.status || 500,
          statusText: errorData?.errorCode || err.code || 'DASHBOARD_ERROR',
        },
      );
    }

    throw data('An unexpected error occurred while loading dashboard data', {
      status: 500,
      statusText: 'UNEXPECTED_ERROR',
    });
  }
};

export default dashboardLoader;
