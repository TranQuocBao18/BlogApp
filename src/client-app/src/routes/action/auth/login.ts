/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { redirect } from 'react-router';
import { useState, useEffect } from 'react';
import { toast } from 'sonner';

/**
 * Custom modules
 */
import { apiLogin } from '@/api/user.api';

/**
 * Types
 */
import type { ActionFunction } from 'react-router';
import { AxiosError } from 'axios';
import type { ActionResponse, AuthResponse, ErrorResponse } from '@/types';
import type { LoginParams } from '@/interfaces/user';

/**
 * Constants
 */
import { APP_CONFIG } from '@/constants/global';

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
    console.log(response.data.succeeded);

    // const responseData = response?.data as AuthResponse;
    if (response.data.succeeded) {
      localStorage.setItem(APP_CONFIG.ACCESS_TOKEN, response.data.data.jwToken);
      localStorage.setItem('user', JSON.stringify(response.data.data.userName));

      return {
        ok: true,
        data: response.data,
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

export default loginAction;
