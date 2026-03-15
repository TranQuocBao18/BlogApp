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
import type { ActionResponse, ErrorResponse } from '@/types';
import { AxiosError } from 'axios';
import {
  redirect,
  type ActionFunction,
  type LoaderFunction,
} from 'react-router';

/**
 * Constants
 */
import { APP_CONFIG } from '@/constants/global';
import { AppRouters } from '@/constants/router';

const loginAction: ActionFunction = async ({ request }) => {
  const data = await request.json();

  // const email = formData.get('email') as string;
  // const password = formData.get('password') as string;
  // const rememberMeStr = formData.get('rememberMe') as string;
  // const rememberMe = rememberMeStr === 'true';

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
    console.log(response.succeeded);

    // const responseData = response?.data as AuthResponse;
    if (response.succeeded) {
      localStorage.setItem(APP_CONFIG.ACCESS_TOKEN, response.data.jwToken);
      localStorage.setItem('user', JSON.stringify(response.data.userName));

      return {
        ok: true,
        data: response,
      } as ActionResponse;
    } else {
      return {
        ok: false,
        err: {
          code: 'AuthenticationError',
          message: response.message || 'Login failed',
        } as ErrorResponse,
      } as ActionResponse;
    }
  } catch (err) {
    if (err instanceof AxiosError) {
      return {
        ok: false,
        err: err.response?.data,
      } as ActionResponse;
    }

    throw err;
  }
};

export const loginLoader: LoaderFunction = () => {
  const accessToken = localStorage.getItem(APP_CONFIG.ACCESS_TOKEN);

  if (accessToken) {
    return redirect(AppRouters.HOME);
  }

  return null;
};

export default loginAction;
