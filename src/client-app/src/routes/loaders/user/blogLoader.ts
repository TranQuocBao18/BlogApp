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
import type { LoaderFunction } from 'react-router';

const blogLoader: LoaderFunction = async ({ request }) => {
  const url = new URL(request.url);

  // Get pagination parameters
  const pageNumber = parseInt(url.searchParams.get('pageNumber') || '1', 10);
  const pageSize = parseInt(url.searchParams.get('pageSize') || '10', 10);

  try {
    const response = await apiGetBlogs(pageNumber, pageSize);

    // Handle backend error response
    if (!response.succeeded) {
      throw data(response.message || 'Failed to fetch blogs', {
        status: 400,
        statusText: response.errorCode || 'Bad Request',
      });
    }

    // Return full PagedResponse for access to pagination metadata
    return response as PagedResponse<Blog>;
  } catch (err) {
    const errorMessage = (err as any)?.message || 'Failed to fetch blogs';

    throw data(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  }
};

export default blogLoader;
