/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { Outlet } from 'react-router';
import { Suspense } from 'react';

/**
 * Components
 */
import { Loading } from '@/components/common/Loading';
// import { Header } from '@/components/common/Header';
import { Footer } from '@/components/common/Footer';

/**
 * Assets
 */
import { Loader2Icon } from 'lucide-react';

export const RootLayout = () => {
  return (
    <div className='flex flex-col min-h-dvh'>
      <Loading className='z-40' />

      {/* <Header /> */}

      <main className='grow flex flex-col'>
        <Suspense fallback={<Loader2Icon className='animate-spin' />}>
          <Outlet />
        </Suspense>
      </main>

      <Footer />
    </div>
  );
};
