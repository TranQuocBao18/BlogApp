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
import { Footer } from '@/components/common/Footer';
import { Header } from '@/components/common/Header';
import { Loading } from '@/components/common/Loading';

/**
 * Assets
 */
import { TooltipProvider } from '@/components/ui/tooltip';
import { Loader2Icon } from 'lucide-react';

export const RootLayout = () => {
  return (
    <div className='flex flex-col min-h-dvh'>
      <TooltipProvider>
        <Loading className='z-40' />

        <Header />

        <main className='grow flex flex-col'>
          <Suspense fallback={<Loader2Icon className='animate-spin' />}>
            <Outlet />
          </Suspense>
        </main>

        <Footer />
      </TooltipProvider>
    </div>
  );
};
