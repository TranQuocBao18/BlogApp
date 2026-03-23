/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { Suspense } from 'react';
import { Outlet } from 'react-router';

/**
 * Components
 */
import { AppSideBar } from '@/components/common/AppSideBar';
import { TopAppBar } from '@/components/common/TopAppBar';
import { SidebarInset, SidebarProvider } from '@/components/ui/sidebar';

/**
 * Assets
 */
import { Loader2Icon } from 'lucide-react';

export const AdminLayout = () => {
  return (
    <SidebarProvider>
      <AppSideBar />

      <SidebarInset className='relative max-h-[calc(100dvh-16px)] overflow-auto'>
        <TopAppBar />

        <Suspense fallback={<Loader2Icon className='animate-spin' />}>
          <Outlet />
        </Suspense>
      </SidebarInset>
    </SidebarProvider>
  );
};
