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
import { apiGetProfile } from '@/api/user.api';
import { APP_CONFIG } from '@/constants/global';

/**
 * Types
 */
import type { ProfileResponse } from '@/interfaces/auth';
import { AxiosError } from 'axios';
import type { LoaderFunction } from 'react-router';

interface ApiResponse<T> {
  succeeded: boolean;
  data?: T;
  message?: string;
  code?: string;
}

const adminLoader: LoaderFunction = async () => {
  const accessToken = localStorage.getItem(APP_CONFIG.ACCESS_TOKEN);
  if (!accessToken) {
    return redirect('/');
  }

  try {
    const response = await apiGetProfile();
    const apiResponse = response as ApiResponse<ProfileResponse>;

    // Check if request succeeded
    if (!apiResponse.succeeded) {
      throw data(apiResponse.message || 'Failed to fetch user profile', {
        status: 401,
        statusText: apiResponse.code || 'UNAUTHORIZED',
      });
    }

    const userProfile = apiResponse.data;

    // Check if user has admin role (SuperAdmin, Admin, etc.)
    const adminRoles = ['SuperAdmin', 'Admin'];
    const hasAdminRole = userProfile?.roles?.some((role) =>
      adminRoles.includes(role),
    );

    if (!hasAdminRole) {
      return redirect('/');
    }

    // Return user profile data if admin
    return userProfile;
  } catch (err) {
    if (err instanceof AxiosError) {
      throw data(err.response?.data?.message || err.message, {
        status: err.response?.status || 401,
        statusText: err.response?.data?.code || err.code,
      });
    }

    throw err;
  }
};

export default adminLoader;
