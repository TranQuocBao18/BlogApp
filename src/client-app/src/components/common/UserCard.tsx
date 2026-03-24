/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { formatDistanceToNowStrict } from 'date-fns';
import { useFetcher } from 'react-router';
import { toast } from 'sonner';

/**
 * Components
 */
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from '@/components/ui/alert-dialog';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import Avatar from 'react-avatar';

/**
 * Assets
 */
import type { UserResponse } from '@/hooks/useUser';
import { TrashIcon } from 'lucide-react';

/**
 * Types
 */
import type { User } from '@/interfaces/user';

type Prop = {
  user: User;
  loggedInUser?: UserResponse;
  onUserDeletedSuccess?: () => void;
};

export const UserCard = ({
  user,
  loggedInUser,
  onUserDeletedSuccess,
}: Prop) => {
  const {
    id: userId,
    username,
    fullName,
    email,
    isAdmin,
    created: createdAt,
  } = user;
  const fetcher = useFetcher();

  return (
    <Card className='group py-4 w-full min-w-0'>
      <CardContent className='grid grid-cols-[max-content_minmax(0,1fr)_max-content] gap-4 px-4 py-2 w-full'>
        <Avatar
          email={email || ''}
          size='40'
          name={fullName || username || email || 'Unknown'}
          className='rounded-lg'
        />

        <div>
          <div className='flex items-center gap-2'>
            <h3 className='font-semibold'>{fullName || username}</h3>

            {isAdmin && (
              <Badge
                variant='outline'
                className='capitalize'
              >
                Admin
              </Badge>
            )}
          </div>

          <p className='text-sm text-muted-foreground truncate'>{email}</p>

          <div className='text-xs text-muted-foreground mt-2'>
            <Tooltip delayDuration={250}>
              <TooltipTrigger>
                Joined{' '}
                {formatDistanceToNowStrict(new Date(createdAt), {
                  addSuffix: true,
                })}
              </TooltipTrigger>

              <TooltipContent side='right'>
                {new Date(createdAt).toLocaleString('en-US', {
                  dateStyle: 'long',
                  timeStyle: 'short',
                })}
              </TooltipContent>
            </Tooltip>
          </div>
        </div>

        {loggedInUser?.username !== username && (
          <AlertDialog>
            <AlertDialogTrigger asChild>
              <Button
                size='icon'
                variant='ghost'
                className='ms-auto -mt-1.5 xl:opacity-0 xl:group-hover:opacity-100'
                aria-label='Delete user'
              >
                <TrashIcon />
              </Button>
            </AlertDialogTrigger>

            <AlertDialogContent>
              <AlertDialogHeader>
                <AlertDialogTitle>
                  Delete User Account: {email}?
                </AlertDialogTitle>

                <AlertDialogDescription>
                  Are you sure you want to delete this user account? <br /> This
                  action is permanent and cannot be undone. All user-related
                  data will be permanently removed.
                </AlertDialogDescription>
              </AlertDialogHeader>

              <AlertDialogFooter>
                <AlertDialogCancel>Cancel</AlertDialogCancel>

                <AlertDialogAction
                  onClick={() => {
                    const submitPromise = fetcher.submit(
                      { userId },
                      {
                        action: '/admin/users',
                        method: 'delete',
                        encType: 'application/json',
                      },
                    );

                    toast.promise(submitPromise, {
                      loading: 'Deleting user account...',
                      success: () => {
                        if (onUserDeletedSuccess) onUserDeletedSuccess();

                        return {
                          message: 'User account deleted successfully.',
                          description:
                            'The user account and all related data have been permanently removed.',
                        };
                      },
                    });
                  }}
                >
                  Delete
                </AlertDialogAction>
              </AlertDialogFooter>
            </AlertDialogContent>
          </AlertDialog>
        )}
      </CardContent>
    </Card>
  );
};
