/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { lazy } from 'react';
import { createBrowserRouter } from 'react-router';

/**
 * Loaders
 */

/**
 * Pages
 */
const LoginPage = lazy(() =>
  import('@/pages/auth/Login').then((module) => ({ default: module.Login })),
);
const SignupPage = lazy(() =>
  import('@/pages/auth/Signup').then((module) => ({ default: module.Signup })),
);

/**
 * Actions
 */
import loginAction, { loginLoader } from '@/routes/action/auth/login';
import signupAction from '@/routes/action/auth/signup';

/**
 * Error boundaries
 */

/**
 * Constants
 */
import { AppRouters } from '@/constants/router';

const router = createBrowserRouter([
  {
    path: AppRouters.LOGIN,
    Component: LoginPage,
    action: loginAction,
    loader: loginLoader,
  },
  {
    path: AppRouters.SIGNUP,
    Component: SignupPage,
    action: signupAction,
    loader: loginLoader,
  },
  {
    path: AppRouters.HOME,
  },
]);

export default router;
