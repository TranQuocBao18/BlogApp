/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { Fragment } from 'react';
import { Link, useLoaderData } from 'react-router';

/**
 * Components
 */
import { BlogTable, columns } from '@/components/common/BlogTable';
import { CommentCard } from '@/components/common/CommentCard';
import { UserCard } from '@/components/common/UserCard';
import { Button } from '@/components/ui/button';
import {
  Card,
  CardAction,
  CardContent,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';

/**
 * Custom hooks
 */
import { useUser } from '@/hooks/useUser';

/**
 * Assets
 */
import { MessageSquareIcon, TextIcon, UserRoundIcon } from 'lucide-react';

/**
 * Types
 */
import type { DashboardData } from '@/routes/loaders/admin/dashboardLoader';

export const Dashboard = () => {
  const loaderData = useLoaderData() as DashboardData;
  const loggedInUser = useUser();

  return (
    <div className='container p-4 space-y-4'>
      <h2 className='text-2xl font-semibold'>Dashboard</h2>

      <div className='grid grid-cols-1 lg:grid-cols-3 gap-4'>
        <Card className='gap-4 p-4'>
          <CardHeader className='flex flex-row items-center gap-2.5 px-4'>
            <div className='bg-muted text-muted-foreground max-w-max p-2 rounded-lg'>
              <TextIcon size={18} />
            </div>

            <CardTitle className='font-normal text-lg'>
              Total Articles
            </CardTitle>
          </CardHeader>

          <CardContent className='text-4xl tracking-wider px-4'>
            {loaderData.blogsCount}
          </CardContent>
        </Card>

        <Card className='gap-4 p-4'>
          <CardHeader className='flex flex-row items-center gap-2.5 px-4'>
            <div className='bg-muted text-muted-foreground max-w-max p-2 rounded-lg'>
              <MessageSquareIcon size={18} />
            </div>

            <CardTitle className='font-normal text-lg'>
              Total Comments
            </CardTitle>
          </CardHeader>

          <CardContent className='text-4xl tracking-wider px-4'>
            {loaderData.commentsCount}
          </CardContent>
        </Card>

        <Card className='gap-4 p-4'>
          <CardHeader className='flex flex-row items-center gap-2.5 px-4'>
            <div className='bg-muted text-muted-foreground max-w-max p-2 rounded-lg'>
              <UserRoundIcon size={18} />
            </div>

            <CardTitle className='font-normal text-lg'>Total Users</CardTitle>
          </CardHeader>

          <CardContent className='text-4xl tracking-wider px-4'>
            {loaderData.usersCount}
          </CardContent>
        </Card>
      </div>

      <Card className='gap-4 py-4'>
        <CardHeader className='flex flex-row items-center gap-2.5 px-6'>
          <div className='bg-muted text-muted-foreground max-w-max p-2 rounded-lg'>
            <TextIcon size={18} />
          </div>

          <CardTitle className='font-normal text-lg'>Recent Articles</CardTitle>

          <CardAction className='ms-auto'>
            <Button
              variant='link'
              size='sm'
            >
              <Link to='/admin/blogs'>See all</Link>
            </Button>
          </CardAction>
        </CardHeader>

        <CardContent className='px-4'>
          <BlogTable
            columns={columns}
            data={loaderData.blogs}
          />
        </CardContent>
      </Card>

      <div className='grid grid-cols-1 xl:grid-cols-[2fr_1fr] gap-4'>
        <Card className='gap-4 p-4'>
          <CardHeader className='flex flex-row items-center gap-2.5 px-4'>
            <div className='bg-muted text-muted-foreground max-w-max p-2 rounded-lg'>
              <MessageSquareIcon size={18} />
            </div>

            <CardTitle className='font-normal text-lg'>
              Recent Comments
            </CardTitle>

            <CardAction className='ms-auto'>
              <Button
                variant='link'
                size='sm'
              >
                <Link to='/admin/comments'>See all</Link>
              </Button>
            </CardAction>
          </CardHeader>

          <CardContent className='px-4'>
            {loaderData.comments.map((comment, index, arr) => (
              <Fragment key={comment.id}>
                <CommentCard comment={comment} />

                {index < arr.length - 1 && <Separator className='my-1' />}
              </Fragment>
            ))}
          </CardContent>
        </Card>

        <Card className='gap-4 py-4 px-2'>
          <CardHeader className='flex flex-row items-center gap-2.5 px-4'>
            <div className='bg-muted text-muted-foreground max-w-max p-2 rounded-lg'>
              <UserRoundIcon size={18} />
            </div>

            <CardTitle className='font-normal text-lg'>Latest Users</CardTitle>

            <CardAction className='ms-auto'>
              <Button
                variant='link'
                size='sm'
              >
                <Link to='/admin/users'>See all</Link>
              </Button>
            </CardAction>
          </CardHeader>

          <CardContent className='px-4 space-y-2'>
            {loaderData.users.map((user) => (
              <UserCard
                key={user.id}
                user={user}
                loggedInUser={loggedInUser}
              />
            ))}
          </CardContent>
        </Card>
      </div>
    </div>
  );
};
