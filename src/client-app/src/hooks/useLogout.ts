/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

import { useLocation, useNavigate } from 'react-router';

/**
 * Custom modules
 */
import { apiLogout } from '@/api/user.api';
import { toast } from 'sonner';

/**
 * Types
 */
import type { Response } from '@/types';

/**
 * Constants
 */
import { APP_CONFIG } from '@/constants/global';

// Hàm xóa cookie
const deleteCookie = (name: string) => {
  document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
};

export const useLogout = () => {
  const navigate = useNavigate();
  const location = useLocation();

  return async () => {
    try {
      const response = (await apiLogout()) as Response;

      // Nếu logout thất bại, hiện lỗi và return
      if (!response?.succeeded) {
        toast.error(response?.message || 'Logout failed');
        return;
      }

      // Logout thành công - clear localStorage và cookies
      localStorage.removeItem(`${APP_CONFIG.ACCESS_TOKEN}`);
      localStorage.removeItem('user');

      // Xóa refresh token từ cookie
      deleteCookie('REFRESH_TOKEN');

      if (location.pathname === '/') {
        window.location.reload();
        return;
      }

      navigate('/', { viewTransition: true });
    } catch (error) {
      // Lỗi request - vẫn clear localStorage và cookies để user có thể login lại
      localStorage.removeItem(`${APP_CONFIG.ACCESS_TOKEN}`);
      localStorage.removeItem('user');
      deleteCookie('REFRESH_TOKEN');
      toast.error('Logout error. Please try again.');
      navigate('/', { viewTransition: true });
    }
  };
};
