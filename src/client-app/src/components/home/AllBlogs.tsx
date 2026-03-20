/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { motion } from 'motion/react';
import { Link, useLoaderData } from 'react-router';

/**
 * Custom modules
 */
import { cn } from '@/lib/utils';

/**
 * Components
 */
import { BlogCard } from '@/components/common/BlogCard';
import { Button } from '@/components/ui/button';

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

export const AllBlogs = ({
  className,
  ...props
}: React.ComponentProps<'section'>) => {
  const { allBlog } = useLoaderData<HomeLoaderResponse>();

  // Extract blogs from paginated response structure
  const blogs = allBlog.data || [];

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
          All blog posts
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
          <>
            <motion.ul
              className='grid lg:grid-cols-2 xl:grid-cols-3 gap-4'
              initial='from'
              whileInView='to'
              viewport={{ once: true }}
              variants={listVariant}
            >
              {blogs.map((blog) => (
                <motion.li
                  key={blog.slug}
                  variants={itemVariant}
                >
                  <BlogCard blog={blog} />
                </motion.li>
              ))}
            </motion.ul>

            <motion.div
              className='mt-8 flex justify-center md:mt-10'
              initial={{ opacity: 0 }}
              animate={{
                opacity: 1,
                transition: {
                  duration: 0.5,
                  ease: 'backInOut',
                },
              }}
            >
              <Button
                size='lg'
                asChild
              >
                <Link
                  to='/blogs'
                  viewTransition
                >
                  See all blogs
                </Link>
              </Button>
            </motion.div>
          </>
        )}
      </div>
    </section>
  );
};
