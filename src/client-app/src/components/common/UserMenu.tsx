/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { Link } from 'react-router';

/**
 * Components
 */
import { SettingsDialog } from '@/components/common/SettingsDialog';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import Avartar from 'react-avatar';

/**
 * Custom hooks
 */
import { useLogout } from '@/hooks/useLogout';
import { useUser } from '@/hooks/useUser';

/**
 * Assets
 */
import { LayoutDashboardIcon, LogOutIcon, SettingsIcon } from 'lucide-react';

export const UserMenu = () => {
  const user = useUser();
  const logout = useLogout();

  if (user) {
    return (
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button
            size='icon'
            variant='ghost'
          >
            <Avartar
              email={user.email}
              size='28'
              name={user.username}
              className='rounded-sm'
            />
          </Button>
        </DropdownMenuTrigger>

        <DropdownMenuContent
          className='min-w-56'
          align='end'
        >
          <DropdownMenuLabel className='p-0 font-normal'>
            <div className='flex items-center gap-2 px-1 py-1.5 text-left-text-sm'>
              <Avartar
                email={user.email}
                size='32'
                name={user.username}
                className='rounded-lg'
              />

              <div className='grid flex-1 text-left text-sm leading-tight'>
                <div className='truncate font-medium'>{user.username}</div>
                <div className='truncate text-xs'>{user.email}</div>
              </div>
            </div>
          </DropdownMenuLabel>

          <DropdownMenuSeparator />

          <DropdownMenuGroup>
            {user.role === 'admin' && (
              <DropdownMenuItem asChild>
                <Link
                  to='/admin/dashboard'
                  viewTransition
                >
                  <LayoutDashboardIcon />
                  Dashboard
                </Link>
              </DropdownMenuItem>
            )}

            <SettingsDialog>
              <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                <SettingsIcon />
                Settings
              </DropdownMenuItem>
            </SettingsDialog>

            <DropdownMenuItem onClick={logout}>
              <LogOutIcon />
              Logout
            </DropdownMenuItem>
          </DropdownMenuGroup>
        </DropdownMenuContent>
      </DropdownMenu>
    );
  }
};
