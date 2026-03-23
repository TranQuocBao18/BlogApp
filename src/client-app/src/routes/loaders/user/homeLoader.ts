/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { data } from 'react-router';

/**
 * Custom modules
 */
import { apiGetBlogs } from '@/api/blog.api';

/**
 * Types
 */
import type { Blog } from '@/interfaces/blog';
import type { PagedResponse } from '@/types';
import { AxiosError } from 'axios';
import type { LoaderFunction } from 'react-router';

export interface HomeLoaderResponse {
  recentBlog: PagedResponse<Blog>;
  allBlog: PagedResponse<Blog>;
}

const homeLoader: LoaderFunction = async () => {
  try {
    const allBlogsResponse = await apiGetBlogs(1, 16);

    const recentBlogResponse = {
      ...allBlogsResponse,
      data: allBlogsResponse.data?.slice(0, 4) || [],
      pageSize: 4,
    };

    return {
      recentBlog: recentBlogResponse,
      allBlog: allBlogsResponse,
    } as HomeLoaderResponse;
  } catch (err) {
    if (err instanceof AxiosError) {
      throw data(err.response?.data?.message || err.message, {
        status: err.response?.status || err.status,
        statusText: err.response?.data?.code || err.code,
      });
    }

    throw err;
  }
};

export default homeLoader;
