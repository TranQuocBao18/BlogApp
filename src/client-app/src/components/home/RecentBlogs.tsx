/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { motion } from 'motion/react';
import { useLoaderData } from 'react-router';

/**
 * Custom modules
 */
import { cn } from '@/lib/utils';

/**
 * Components
 */
import { BlogCard } from '@/components/common/BlogCard';

/**
 * Types
 */
import type { HomeLoaderResponse } from '@/routes/loaders/user/homeLoader';
import type { Variants } from 'motion/react';

/**
 * Motion variants
 */
const listVariant: Variants = {
  to: {
    transition: {
      staggerChildren: 0.05,
    },
  },
};

const itemVariant: Variants = {
  from: { opacity: 0 },
  to: {
    opacity: 1,
    transition: {
      duration: 1,
      ease: 'backInOut',
    },
  },
};

export const RecentBlogs = ({
  className,
  ...props
}: React.ComponentProps<'section'>) => {
  const { recentBlog } = useLoaderData<HomeLoaderResponse>();

  // Extract blogs from paginated response structure
  const blogs = recentBlog.data || [];

  return (
    <section
      className={cn('section', className)}
      {...props}
    >
      <div className='container'>
        <motion.h2
          className='section-title'
          initial={{ opacity: 0 }}
          animate={{
            opacity: 1,
            transition: {
              duration: 0.5,
              ease: 'easeOut',
            },
          }}
        >
          Recent blog posts
        </motion.h2>

        {blogs.length === 0 ? (
          <motion.div
            className='text-center py-12'
            initial={{ opacity: 0 }}
            animate={{
              opacity: 1,
              transition: {
                duration: 0.5,
                ease: 'easeOut',
              },
            }}
          >
            <p className='text-muted-foreground'>
              No blog posts available yet. Check back soon!
            </p>
          </motion.div>
        ) : (
          <motion.ul
            className='grid gap-4 lg:grid-cols-2 lg:grid-rows-3'
            initial='from'
            whileInView='to'
            viewport={{ once: true }}
            variants={listVariant}
          >
            {blogs.map((blog, index) => (
              <motion.li
                key={blog.slug}
                className={cn(index === 0 && 'lg:row-span-3')}
                variants={itemVariant}
              >
                <BlogCard
                  blog={blog}
                  size={index > 0 ? 'sm' : 'default'}
                />
              </motion.li>
            ))}
          </motion.ul>
        )}
      </div>
    </section>
  );
};
