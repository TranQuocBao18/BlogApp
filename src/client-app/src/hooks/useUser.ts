/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { useEffect, useState } from 'react';

/**
 * Types
 */
import type { User } from '@/interfaces/user';
export type UserResponse = Pick<User, 'username' | 'email'> & {
  roles?: string[];
};

export const useUser = () => {
  const [user, setUser] = useState<UserResponse>();

  useEffect(() => {
    // Function to read user from localStorage
    const readUser = () => {
      const userJson = localStorage.getItem('user');
      if (userJson) {
        try {
          const parsedUser = JSON.parse(userJson) as UserResponse;
          setUser(parsedUser);
        } catch (e) {
          console.error('Failed to parse user from localStorage:', e);
        }
      }
    };

    // Read user on mount
    readUser();

    // Listen for storage changes (from other tabs/windows)
    const handleStorageChange = (e: StorageEvent) => {
      if (e.key === 'user') {
        readUser();
      }
    };

    window.addEventListener('storage', handleStorageChange);

    // Custom event listener for same-tab updates
    const handleUserUpdate = () => {
      readUser();
    };

    window.addEventListener('userUpdated', handleUserUpdate as EventListener);

    return () => {
      window.removeEventListener('storage', handleStorageChange);
      window.removeEventListener(
        'userUpdated',
        handleUserUpdate as EventListener,
      );
    };
  }, []);

  return user;
};
