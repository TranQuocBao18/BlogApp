/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { EditorContent, useEditor } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import { useCallback, useMemo } from 'react';
import { useLoaderData, useNavigate } from 'react-router';
import { toast } from 'sonner';

/**
 * Custom modules
 */
import { getReadingTime } from '@/lib/utils';
/**
 * Components
 */
import { Page } from '@/components/common/Page';
import { AspectRatio } from '@/components/ui/aspect-ratio';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Separator } from '@/components/ui/separator';
import Avatar from 'react-avatar';

/**
 * Assets
 */
import {
  ArrowLeftIcon,
  FacebookIcon,
  LinkedinIcon,
  LinkIcon,
  MessageSquareIcon,
  ShareIcon,
  ThumbsUpIcon,
  TwitterIcon,
} from 'lucide-react';

/**
 * Types
 */
import type { Blog } from '@/interfaces/blog';
import type { DropdownMenuProps } from '@radix-ui/react-dropdown-menu';

interface SharedDropdownProps extends DropdownMenuProps {
  blogTitle: string;
}

export const ShareDropDown = ({
  blogTitle,
  children,
  ...props
}: SharedDropdownProps) => {
  const blogUrl = window.location.href;
  const shareText = 'Just read this insightful article and wanted to share !';

  const SHARE_LINKS = useMemo(() => {
    return {
      x: `https://x.com/intent/post?url=${encodeURIComponent(blogUrl)}&text=${encodeURIComponent(`${shareText} "${blogTitle}"`)}`,
      facebook: `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(blogUrl)}&quote=${encodeURIComponent(`${shareText} - "${blogTitle}"`)}`,
      linkedin: `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(blogUrl)}&title=${encodeURIComponent(blogTitle)}&summary=${encodeURIComponent(shareText)}&source=${encodeURIComponent(blogTitle)}`,
    };
  }, [blogTitle, blogUrl]);

  const handleCopy = useCallback(async () => {
    try {
      await navigator.clipboard.writeText(blogUrl);
      toast.success('Link copied!');
    } catch (err) {
      toast.error('Failed to copy!');
      console.error('Failed to copy: ', err);
    }
  }, [blogUrl]);

  const shareOnSocial = useCallback((platformUrl: string) => {
    window.open(platformUrl, '_blank', 'noopener,noreferrer');
  }, []);

  return (
    <DropdownMenu {...props}>
      <DropdownMenuTrigger asChild>{children}</DropdownMenuTrigger>

      <DropdownMenuContent className='min-w-[240px]'>
        <DropdownMenuItem onSelect={handleCopy}>
          <LinkIcon />
          Copy link
        </DropdownMenuItem>

        <DropdownMenuSeparator />

        <DropdownMenuItem onSelect={() => shareOnSocial(SHARE_LINKS.x)}>
          <TwitterIcon />
          Share on X
        </DropdownMenuItem>

        <DropdownMenuSeparator />

        <DropdownMenuItem onSelect={() => shareOnSocial(SHARE_LINKS.facebook)}>
          <FacebookIcon />
          Share on Facebook
        </DropdownMenuItem>

        <DropdownMenuSeparator />

        <DropdownMenuItem onSelect={() => shareOnSocial(SHARE_LINKS.linkedin)}>
          <LinkedinIcon />
          Share on Linkedin
        </DropdownMenuItem>

        <DropdownMenuSeparator />
      </DropdownMenuContent>
    </DropdownMenu>
  );
};

export const BlogDetail = () => {
  const navigate = useNavigate();

  const blog = useLoaderData() as Blog;

  const editor = useEditor({
    extensions: [StarterKit],
    content: blog.content,
    editable: false,
    autofocus: false,
  });

  const bannerUrl = blog.banner?.url || '/default-banner.jpg';
  const bannerWidth = blog.banner?.width || 1890;
  const bannerHeight = blog.banner?.height || 945;

  return (
    <Page>
      <article className='relative container max-w-[1280px] pt-6 pb-12'>
        <Button
          variant='outline'
          size='icon'
          className='sticky top-22 -ms-16'
          onClick={() => navigate(-1)}
        >
          <ArrowLeftIcon />
        </Button>

        <h1 className='text-4xl leading-tight font-semibold -mt-10'>
          {blog.title}
        </h1>

        <div className='flex items-center gap-3 my-8'>
          <div className='flex items-center gap-3'>
            <Avatar
              name={blog.authorName || 'unknown'}
              size='32'
              round
            />

            <span>{blog.authorName || 'unknown'}</span>
          </div>

          <Separator
            orientation='vertical'
            className='data-[orientation=vertical]:h-1 data-[orientation=vertical]:w-1 rounded-full'
          />

          <div className='text-muted-foreground'>
            {editor
              ? `${getReadingTime(editor.getText() || '')} min read`
              : 'Loading...'}
          </div>

          <Separator
            orientation='vertical'
            className='data-[orientation=vertical]:h-1 data-[orientation=vertical]:w-1 rounded-full'
          />

          <div className='text-muted-foreground'>
            {new Date(blog.created).toLocaleDateString('en-US', {
              dateStyle: 'medium',
            })}
          </div>
        </div>

        <Separator />

        <div className='flex items-center gap-2 my-2'>
          <Button variant='ghost'>
            <ThumbsUpIcon />

            {blog.likeCount}
          </Button>

          <Button variant='ghost'>
            <MessageSquareIcon />

            {blog.commentCount}
          </Button>

          <ShareDropDown blogTitle={blog.title}>
            <Button
              variant='ghost'
              className='ms-auto'
            >
              <ShareIcon />
              Share
            </Button>
          </ShareDropDown>
        </div>

        <Separator />

        <div className='my-8'>
          <AspectRatio
            ratio={21 / 9}
            className='overflow-hidden rounded-xl bg-border'
          >
            <img
              src={bannerUrl}
              width={bannerWidth}
              height={bannerHeight}
              alt={`Banner of blog: ${blog.title}`}
              className='w-full h-full object-cover'
            />
          </AspectRatio>
        </div>

        <EditorContent editor={editor} />
      </article>
    </Page>
  );
};
