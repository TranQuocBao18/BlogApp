/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Assets
 */
import {
  LayoutDashboardIcon,
  TextIcon,
  MessageSquareIcon,
  UserIcon,
} from 'lucide-react';

export const MAIN_MENU = [
  {
    label: 'Dashboard',
    url: '/admin/dashboard',
    icon: LayoutDashboardIcon,
  },
  {
    label: 'Blogs',
    url: '/admin/blogs',
    icon: TextIcon,
  },
  {
    label: 'Comments',
    url: '/admin/comments',
    icon: MessageSquareIcon,
  },
  {
    label: 'Users',
    url: '/admin/users',
    icon: UserIcon,
  },
];
