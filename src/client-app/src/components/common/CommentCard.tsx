/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { formatDistanceToNow } from 'date-fns';

/**
 * Components
 */
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import Avatar from 'react-avatar';

/**
 * Assets
 */

/**
 * Types
 */
import type { Comment } from '@/interfaces/comment';

type Prop = {
  comment: Comment;
};

export const CommentCard = ({ comment }: Prop) => {
  const { content, user, created: createdAt, likeCount: likesCount } = comment;

  return (
    <div className='@container'>
      <div className='group flex flex-col items-start gap-4 p-4 rounded-xl hover:bg-accent/25 @md:flex-row'>
        <Avatar
          email={user?.email}
          size='40'
          name={user?.fullName || user?.username || user?.email || 'Unknown'}
          round
        />

        <div className='flex flex-col gap-2 me-auto'>
          <div className='flex items-center gap-2'>
            {user ? (
              <div className='text-sm text-muted-foreground'>
                @{user.username}
              </div>
            ) : (
              <div className='text-sm text-destructive/80 italic'>
                <Tooltip delayDuration={250}>
                  <TooltipTrigger>Account deleted</TooltipTrigger>

                  <TooltipContent>This account has been removed</TooltipContent>
                </Tooltip>
              </div>
            )}

            <div className='size-1 rounded-full bg-muted-foreground/50'></div>

            <div className='text-sm text-muted-foreground'>
              <Tooltip delayDuration={250}>
                <TooltipTrigger>
                  {formatDistanceToNow(createdAt, { addSuffix: true })}
                </TooltipTrigger>

                <TooltipContent>
                  {new Date(createdAt).toLocaleString('en-US', {
                    dateStyle: 'long',
                    timeStyle: 'short',
                  })}
                </TooltipContent>
              </Tooltip>
            </div>
          </div>

          <div className='max-w-[60ch] text-sm'>{content}</div>
        </div>
      </div>
    </div>
  );
};
