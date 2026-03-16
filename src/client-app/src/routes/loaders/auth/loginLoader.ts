/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */

/**
 * Types
 */
import type { LoaderFunction } from 'react-router';
import { redirect } from 'react-router';

/**
 * Constants
 */
import { APP_CONFIG } from '@/constants/global';
import { AppRouters } from '@/constants/router';

const loginLoader: LoaderFunction = () => {
  const accessToken = localStorage.getItem(APP_CONFIG.ACCESS_TOKEN);

  if (accessToken) {
    return redirect(AppRouters.HOME);
  }

  return null;
};

export default loginLoader;
