/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */

/**
 * Components
 */
import { Page } from '@/components/common/Page';
import { AllBlogs } from '@/components/home/AllBlogs';
import { Hero } from '@/components/home/Hero';
import { RecentBlogs } from '@/components/home/RecentBlogs';

export const HomePage = () => {
  return (
    <Page>
      <Hero />

      <RecentBlogs />

      <AllBlogs />
    </Page>
  );
};
