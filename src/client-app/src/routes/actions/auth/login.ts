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
import { apiLogin } from '@/api/user.api';

/**
 * Types
 */
import type { Response } from '@/types';
import { AxiosError } from 'axios';
import { type ActionFunction } from 'react-router';

/**
 * Constants
 */
import { APP_CONFIG } from '@/constants/global';

const loginAction: ActionFunction = async ({ request }) => {
  const data = await request.json();

  let ipAddress = '';

  try {
    const ipResponse = await fetch('https://api.ipify.org/?format=json');
    const ipData = await ipResponse.json();
    ipAddress = ipData.ip;
  } catch (error) {
    console.error('Failed to get IP address:', error);
  }

  try {
    const response = await apiLogin(
      {
        ...data,
        ipAddress,
      },
      { loading: false },
    );

    // Response has Response<T> structure from backend
    if (response.succeeded) {
      localStorage.setItem(APP_CONFIG.ACCESS_TOKEN, response.data.jwToken);
      localStorage.setItem(
        'user',
        JSON.stringify({
          username: response.data.userName,
          email: response.data.email,
          roles: response.data.roles || [],
        }),
      );

      return {
        succeeded: true,
        message: response.message || 'Login successful',
        data: response.data,
      } as Response;
    } else {
      return {
        succeeded: false,
        message: response.message || 'Login failed',
        errorCode: 'AuthenticationError',
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
        errorCode: errorResponse?.errorCode || 'AuthenticationError',
        errors: errorResponse?.errors,
        data: null,
      } as Response;
    }

    throw err;
  }
};

export default loginAction;
