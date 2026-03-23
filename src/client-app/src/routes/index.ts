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
import { AdminLayout } from '@/components/layouts/AdminLayout';
import { RootLayout } from '@/components/layouts/Root';
import adminLoader from '@/routes/loaders/admin/adminLoader';
import dashboardLoader from '@/routes/loaders/admin/dashboardLoader';
import loginLoader from '@/routes/loaders/auth/loginLoader';
import blogDetailLoader from '@/routes/loaders/user/blogDetailLoader';
import blogLoader from '@/routes/loaders/user/blogLoader';
import homeLoader from '@/routes/loaders/user/homeLoader';
/**
 * Pages
 */
const LoginPage = lazy(() =>
  import('@/pages/auth/LoginPage').then((module) => ({
    default: module.LoginPage,
  })),
);
const SignupPage = lazy(() =>
  import('@/pages/auth/SignupPage').then((module) => ({
    default: module.SignupPage,
  })),
);
const HomePage = lazy(() =>
  import('@/pages/user/HomePage').then((module) => ({
    default: module.HomePage,
  })),
);
const BlogPage = lazy(() =>
  import('@/pages/user/BlogPage').then((module) => ({
    default: module.BlogPage,
  })),
);
const BlogDetailPage = lazy(() =>
  import('@/pages/user/BlogDetail').then((module) => ({
    default: module.BlogDetail,
  })),
);
const Dashboard = lazy(() =>
  import('@/pages/admin/Dashboard').then((module) => ({
    default: module.Dashboard,
  })),
);

/**
 * Actions
 */
import loginAction from '@/routes/actions/auth/login';
import signupAction from '@/routes/actions/auth/signup';

/**
 * Error boundaries
 */
import { RootErrorBoundary } from '@/pages/error/Root';

/**
 * Constants
 */
import { AppRouters } from '@/constants/router';

export const toRelativePath = (absolutePath: string): string => {
  return absolutePath.startsWith('/') ? absolutePath.slice(1) : absolutePath;
};

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
      {
        path: AppRouters.BLOGS,
        Component: BlogPage,
        loader: blogLoader,
      },
      {
        path: AppRouters.BLOG_DETAIL,
        Component: BlogDetailPage,
        loader: blogDetailLoader,
      },
    ],
  },
  {
    path: AppRouters.ADMIN,
    Component: AdminLayout,
    loader: adminLoader,
    ErrorBoundary: RootErrorBoundary,
    children: [
      {
        path: toRelativePath(AppRouters.DASHBOARD),
        Component: Dashboard,
        loader: dashboardLoader,
        handle: { breadcrumb: 'Dashboard' },
      },
      {
        path: toRelativePath(AppRouters.BLOGS_CREATE),
        handle: { breadcrumb: 'Create new Blog' },
      },
      {
        path: toRelativePath(AppRouters.BLOGS_EDIT),
        handle: { breadcrumb: 'Edit Blog' },
      },
      {
        path: toRelativePath(AppRouters.USERS),
        handle: { breadcrumb: 'Users' },
      },
    ],
  },
  // {
  //       path: '/settings',
  //       action: settingsAction,
  // }
]);

export default router;
