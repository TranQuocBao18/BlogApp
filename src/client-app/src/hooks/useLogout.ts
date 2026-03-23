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

export const useLogout = () => {
  const navigate = useNavigate();
  const location = useLocation();

  return async () => {
    try {
      const response = (await apiLogout()) as Response<boolean>;

      const isLogoutSuccess =
        response?.succeeded === true && response?.data !== false;

      if (!isLogoutSuccess) {
        toast.error(response?.message || 'Logout failed');
      }

      localStorage.removeItem(`${APP_CONFIG.ACCESS_TOKEN}`);
      localStorage.removeItem('user');

      if (location.pathname === '/') {
        window.location.reload();
        return;
      }

      navigate('/', { viewTransition: true });
    } catch (error) {
      localStorage.removeItem(`${APP_CONFIG.ACCESS_TOKEN}`);
      localStorage.removeItem('user');
      toast.error('Logout error. Please try again.');
      navigate('/', { viewTransition: true });
    }
  };
};
