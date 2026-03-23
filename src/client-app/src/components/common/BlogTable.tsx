/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import {
  flexRender,
  getCoreRowModel,
  useReactTable,
} from '@tanstack/react-table';
import { Editor } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import { formatDistanceToNowStrict } from 'date-fns';
import { motion } from 'motion/react';
import { useMemo } from 'react';
import { Link, useFetcher } from 'react-router';

/**
 * Custom modules
 */
import { cn } from '@/lib/utils';

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
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import Avatar from 'react-avatar';

const MotionTableBody = motion.create(TableBody);
const MotionTableRow = motion.create(TableRow);

/**
 * Assets
 */
import { Loader2Icon, MoreHorizontalIcon } from 'lucide-react';

/**
 * Types
 */
import type { Blog } from '@/interfaces/blog';
import type { ColumnDef } from '@tanstack/react-table';
import type { Variants } from 'motion/react';

interface BlogTableProps<TData, TValue> {
  columns: ColumnDef<TData, TValue>[];
  data: TData[];
}

/**
 * Framer motion variants
 */
const tableBodyVariants: Variants = {
  to: {
    transition: {
      staggerChildren: 0.01,
    },
  },
};

const tableRowVariants: Variants = {
  from: { opacity: 0 },
  to: {
    opacity: 1,
    transition: {
      duration: 0.5,
    },
  },
};

const BlogActionDropdown = ({ blog }: { blog: Blog }) => {
  const fetcher = useFetcher();

  const isPublished = useMemo(() => blog.status === 'published', [blog.status]);

  const isChanging = fetcher.state !== 'idle';
  const isUpdating = isChanging && fetcher.formMethod === 'PUT';
  const isDeleting = isChanging && fetcher.formMethod === 'DELETE';

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button
          variant='ghost'
          className='data-[state=open]:bg-muted text-muted-foreground flex size-8'
        >
          <span className='sr-only'>Open menu</span>

          <MoreHorizontalIcon />
        </Button>
      </DropdownMenuTrigger>

      <DropdownMenuContent
        align='end'
        className='w-32'
      >
        <DropdownMenuItem asChild>
          <Link
            to={`/admin/blogs/${blog.slug}/edit`}
            viewTransition
          >
            Edit
          </Link>
        </DropdownMenuItem>

        <AlertDialog>
          <AlertDialogTrigger asChild>
            <DropdownMenuItem
              onSelect={(e) => e.preventDefault()}
              disabled={isUpdating}
            >
              {isUpdating && <Loader2Icon className='animate-spin' />}

              {isPublished ? 'Unpublish' : 'Publish'}
            </DropdownMenuItem>
          </AlertDialogTrigger>

          <AlertDialogContent>
            <AlertDialogHeader>
              <AlertDialogTitle>
                {isPublished ? 'Unpublish Blog Post?' : 'Publish Blog Post?'}
              </AlertDialogTitle>

              <AlertDialogDescription>
                {isPublished
                  ? 'This blog will no longer be visible to readers. You can publish it again anytime. Are you sure you want to unpublish this blog post?'
                  : 'Once published, this blog will be visible to everyone. Are you sure you want to publish this blog post right now?'}
              </AlertDialogDescription>
            </AlertDialogHeader>

            <AlertDialogFooter>
              <AlertDialogCancel>Cancel</AlertDialogCancel>

              <AlertDialogAction
                onClick={() => {
                  const formData = new FormData();

                  formData.append(
                    'status',
                    isPublished ? 'draft' : 'published',
                  );

                  fetcher.submit(formData, {
                    action: `/admin/blogs/${blog.slug}/edit`,
                    method: 'put',
                    encType: 'multipart/form-data',
                  });
                }}
              >
                {isPublished ? 'Unpublish' : 'Publish'}
              </AlertDialogAction>
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialog>

        <DropdownMenuSeparator />

        <AlertDialog>
          <AlertDialogTrigger asChild>
            <DropdownMenuItem
              onSelect={(e) => e.preventDefault()}
              disabled={isDeleting}
              className='text-destructive focus:bg-destructive focus:text-destructive-foreground'
            >
              {isDeleting && <Loader2Icon className='animate-spin' />}
              Delete
            </DropdownMenuItem>
          </AlertDialogTrigger>

          <AlertDialogContent>
            <AlertDialogHeader>
              <AlertDialogTitle>Delete Blog Post?</AlertDialogTitle>

              <AlertDialogDescription>
                This action cannot be undone. Are you sure you want to delete
                this blog post permanently?
              </AlertDialogDescription>
            </AlertDialogHeader>

            <AlertDialogFooter>
              <AlertDialogCancel>Cancel</AlertDialogCancel>

              <AlertDialogAction
                onClick={() => {
                  const data = { blogId: blog.id };

                  fetcher.submit(data, {
                    action: '/admin/blogs',
                    method: 'delete',
                    encType: 'application/json',
                  });
                }}
              >
                Delete
              </AlertDialogAction>
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialog>
      </DropdownMenuContent>
    </DropdownMenu>
  );
};

export const columns: ColumnDef<Blog>[] = [
  {
    accessorKey: 'title',
    header: 'Blog',
    cell: ({ row }) => {
      const blog = row.original;
      const editor = new Editor({
        extensions: [StarterKit],
        content: row.original.content,
        editable: false,
        autofocus: false,
      });

      // Fallback banner - same pattern as BlogCard
      const bannerUrl =
        blog.banner?.url ||
        'https://via.placeholder.com/800x400?text=No+Banner';
      const bannerWidth = blog.banner?.width || 800;
      const bannerHeight = blog.banner?.height || 400;

      return (
        <Link
          className='flex items-center gap-4 group'
          to={`/blogs/${blog.slug}`}
          viewTransition
        >
          <figure className='shrink-0 w-[120px] h-[68px] rounded-md overflow-hidden'>
            <img
              src={bannerUrl}
              width={bannerWidth}
              height={bannerHeight}
              alt={blog.title}
              className='w-full h-full object-cover'
            />
          </figure>

          <div>
            <div className='font-semibold mb-1 truncate max-w-[50ch] group-hover:underline'>
              {blog.title}
            </div>

            <p className='text-muted-foreground line-clamp-2 max-w-[50ch] text-wrap'>
              {editor.getText()}
            </p>
          </div>
        </Link>
      );
    },
  },
  {
    accessorKey: 'authorName',
    header: 'Author',
    cell: ({ row }) => {
      const authorName = row.getValue('authorName') as string;

      return (
        <div className='flex items-center gap-2'>
          <Avatar
            name={authorName || 'unknown'}
            size='24'
            round
            className='rounded-md'
          />
          <div>{authorName || 'unknown'}</div>
        </div>
      );
    },
  },
  {
    accessorKey: 'status',
    header: 'Status',
    cell: ({ row }) => {
      const status = row.getValue('status') as 'draft' | 'published';

      return (
        <Badge
          variant='outline'
          className={cn(
            'gap-1.5 capitalize',
            status === 'published'
              ? 'border-emerald-300 dark:border-emerald-800 bg-emerald-100/20 dark:bg-emerald-800/20'
              : 'border-amber-300 dark:border-amber-800 bg-amber-100/20 dark:bg-amber-800/20',
          )}
        >
          <div
            className={cn(
              'w-1.5 h-1.5 rounded-full',
              status === 'published'
                ? 'bg-emerald-500 dark:bg-emerald-600'
                : 'bg-amber-500 dark:bg-amber-600',
            )}
          ></div>
          {status}
        </Badge>
      );
    },
  },
  {
    accessorKey: 'created',
    header: 'Created At',
    cell: ({ row }) => {
      const createdAt = row.getValue('created') as string;
      const date = formatDistanceToNowStrict(createdAt, {
        addSuffix: true,
      });

      return (
        <Tooltip delayDuration={250}>
          <TooltipTrigger>{date}</TooltipTrigger>

          <TooltipContent>
            {new Date(createdAt).toLocaleString('en-US', {
              dateStyle: 'long',
              timeStyle: 'short',
            })}
          </TooltipContent>
        </Tooltip>
      );
    },
  },
  {
    id: 'actions',
    enableHiding: true,
    cell: ({ row }) => <BlogActionDropdown blog={row.original} />,
  },
];

export const BlogTable = <TData, TValue>({
  columns,
  data,
}: BlogTableProps<TData, TValue>) => {
  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <Table>
      <TableHeader>
        {table.getHeaderGroups().map((headerGroup) => (
          <TableRow
            key={headerGroup.id}
            className='border-none'
          >
            {headerGroup.headers.map((header) => (
              <TableHead
                key={header.id}
                colSpan={header.colSpan}
                className='bg-muted px-4 first:rounded-l-lg last:rounded-r-lg'
              >
                {header.isPlaceholder
                  ? null
                  : flexRender(
                      header.column.columnDef.header,
                      header.getContext(),
                    )}
              </TableHead>
            ))}
          </TableRow>
        ))}
      </TableHeader>

      <MotionTableBody
        initial='from'
        animate='to'
        variants={tableBodyVariants}
      >
        {table.getRowModel().rows.length ? (
          table.getRowModel().rows.map((row) => (
            <MotionTableRow
              key={row.id}
              data-state={row.getIsSelected() && 'selected'}
              variants={tableRowVariants}
            >
              {row.getVisibleCells().map((cell) => (
                <TableCell
                  key={cell.id}
                  className='px-4 py-3 min-h-16 max-w-max'
                >
                  {flexRender(cell.column.columnDef.cell, cell.getContext())}
                </TableCell>
              ))}
            </MotionTableRow>
          ))
        ) : (
          <TableRow>
            <TableCell
              colSpan={columns.length}
              className='h-24 text-center'
            >
              No results.
            </TableCell>
          </TableRow>
        )}
      </MotionTableBody>
    </Table>
  );
};
