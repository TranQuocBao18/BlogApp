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
import { apiGetBlogBySlug } from '@/api/blog.api';

/**
 * Types
 */
import type { Blog } from '@/interfaces/blog';
import type { Response } from '@/types';
import type { LoaderFunction } from 'react-router';

const blogDetailLoader: LoaderFunction = async ({ params }) => {
  const slug = params.slug;
  if (!slug) {
    throw data('Blog slug is required', {
      status: 400,
      statusText: 'Bad Request',
    });
  }

  try {
    const response = (await apiGetBlogBySlug(slug)) as Response<Blog>;

    // Handle backend error response
    if (!response.succeeded) {
      throw data(response.message || 'Failed to fetch blog', {
        status: 400,
        statusText: response.errorCode || 'Bad Request',
      });
    }

    // Return blog data
    return response.data;
  } catch (err) {
    const errorMessage = (err as any)?.message || 'Failed to fetch blog';

    throw data(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  }
};

export default blogDetailLoader;
