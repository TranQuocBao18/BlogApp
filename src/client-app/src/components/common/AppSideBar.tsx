/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { Link, useLocation } from 'react-router';

/**
 * Components
 */
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupLabel,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from '@/components/ui/sidebar';
import { Logo } from '@/components/common/Logo';
import { SidebarUserMenu } from '@/components/common/SidebarUserMenu';

/**
 * Constants
 */
import { MAIN_MENU } from '@/constants/dashboard';

export const AppSideBar = ({
  ...props
}: React.ComponentProps<typeof Sidebar>) => {
  const location = useLocation();

  return (
    <Sidebar
      variant='inset'
      {...props}
    >
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton size='lg'>
              <Logo />
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>

      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupLabel>Main menu</SidebarGroupLabel>

          <SidebarMenu>
            {MAIN_MENU.map((item) => (
              <SidebarMenuItem key={item.url}>
                <SidebarMenuButton
                  isActive={location.pathname === item.url}
                  tooltip={item.label}
                  asChild
                >
                  <Link
                    to={item.url}
                    viewTransition
                  >
                    <item.icon />

                    <span>{item.label}</span>
                  </Link>
                </SidebarMenuButton>
              </SidebarMenuItem>
            ))}
          </SidebarMenu>
        </SidebarGroup>
      </SidebarContent>

      <SidebarFooter>
        <SidebarUserMenu />
      </SidebarFooter>
    </Sidebar>
  );
};
