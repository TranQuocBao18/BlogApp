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
import { RootLayout } from '@/components/layouts/Root';
import loginLoader from '@/routes/loaders/auth/loginLoader';
import homeLoader from '@/routes/loaders/user/homeLoader';
/**
 * Pages
 */
const LoginPage = lazy(() =>
  import('@/pages/auth/Login').then((module) => ({ default: module.Login })),
);
const SignupPage = lazy(() =>
  import('@/pages/auth/Signup').then((module) => ({ default: module.Signup })),
);
const HomePage = lazy(() =>
  import('@/pages/user/HomePage').then((module) => ({ default: module.HomePage })),
);

/**
 * Actions
 */
import loginAction from '@/routes/actions/auth/login';
import signupAction from '@/routes/actions/auth/signup';

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
    Component: RootLayout,
    children: [
      {
        index: true,
        Component: HomePage,
        loader: homeLoader,
      },
    ],
  },
]);

export default router;
