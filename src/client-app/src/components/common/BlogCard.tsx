/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { Editor } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import { formatDistanceToNowStrict } from 'date-fns';
import { Link } from 'react-router';

/**
 * Custom modules
 */
import { cn } from '@/lib/utils';

/**
 * Components
 */
import { AspectRatio } from '@/components/ui/aspect-ratio';
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';

/**
 * Types
 */
import type { Blog } from '@/interfaces/blog';

interface BlogCardProps extends React.ComponentProps<'div'> {
  blog: Blog;
  size?: 'default' | 'sm';
}

export const BlogCard: React.FC<BlogCardProps> = ({
  blog,
  size = 'default',
  className,
  ...props
}) => {
  const { title, content, slug, banner, authorName, created } = blog;
  
  const editor = new Editor({
    extensions: [StarterKit],
    content,
    editable: false,
    autofocus: false,
  });

  // Fallback banner URL
  const bannerUrl = banner?.url || 'https://via.placeholder.com/800x400?text=No+Banner';
  const bannerWidth = banner?.width || 800;
  const bannerHeight = banner?.height || 400;

  return (
    <Card
      className={cn(
        'relative group pt-2 h-full @container',
        size === 'default' && 'flex flex-col-reverse justify-end',
        size === 'sm' && 'py-2 grid grid-cols-[1fr_1.15fr] gap-0 items-center',
        className,
      )}
      {...props}
    >
      <CardHeader
        className={cn(
          'gap-2',
          size === 'sm' && 'content-center order-1 ps-4 py-3',
        )}
      >
        <div className='flex items-center gap-2 text-muted-foreground text-sm font-medium'>
          <p className='@max-4xs:hidden'>{authorName || 'Unknown'}</p>

          <div className='w-1 h-1 rounded-full bg-muted-foreground/50 @max-4xs:hidden'></div>

          <Tooltip delayDuration={250}>
            <TooltipTrigger>
              {formatDistanceToNowStrict(new Date(created), { addSuffix: true })}
            </TooltipTrigger>

            <TooltipContent>
              {new Date(created).toLocaleString('en-US', {
                dateStyle: 'long',
                timeStyle: 'short',
              })}
            </TooltipContent>
          </Tooltip>
        </div>

        <Link
          to={`/blogs/${slug}`}
          viewTransition
        >
          <CardTitle
            className={cn(
              'underline-offset-4 hover:underline leading-tight line-clamp-2',
              size === 'default' && 'text-xl @md:text-2xl',
            )}
          >
            {title}
          </CardTitle>
        </Link>

        <CardDescription
          className={cn(
            'line-clamp-2 text-balance',
            size === 'sm' && '@max-4xs:hidden',
          )}
        >
          {editor.getText()}
        </CardDescription>
      </CardHeader>

      <CardContent className='px-2'>
        <Link
          to={`/blogs/${slug}`}
          viewTransition
        >
          <AspectRatio
            ratio={21 / 9}
            className='rounded-lg overflow-hidden'
          >
            <img
              src={bannerUrl}
              width={bannerWidth}
              height={bannerHeight}
              alt={title}
              className='w-full h-full object-cover'
            />
          </AspectRatio>
        </Link>
      </CardContent>
    </Card>
  );
};
