/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */

/**
 * Custom modules
 */
import { apiRegister } from '@/api/user.api';

/**
 * Components
 */

/**
 * Types
 */
import type { Response } from '@/types';
import { AxiosError } from 'axios';
import type { ActionFunction } from 'react-router';

const signupAction: ActionFunction = async ({ request }) => {
  const data = await request.json();

  try {
    const response = await apiRegister(data);

    // Backend returns Response<T> with PascalCase: Succeeded, Message, ErrorCode, Errors, Data
    if (response.succeeded) {
      return {
        succeeded: true,
        message: 'Signup successful',
        data: response.data,
      } as Response;
    } else {
      // Business logic failed but HTTP OK - return exact backend message
      return {
        succeeded: false,
        message: response.message || 'SignUp failed',
        errorCode: response.errorCode,
        errors: response.errors,
        data: null,
      } as Response;
    }
  } catch (err) {
    if (err instanceof AxiosError) {
      const errorResponse = err.response?.data as Response;
      return {
        succeeded: false,
        message: errorResponse?.message || 'Network error',
        errorCode: errorResponse?.errorCode || 'BadRequest',
        errors: errorResponse?.errors,
        data: null,
      } as Response;
    }

    throw err;
  }
};

export default signupAction;
