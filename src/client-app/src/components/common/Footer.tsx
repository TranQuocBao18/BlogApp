/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */

/**
 * Custom modules
 */
import { cn } from '@/lib/utils';

/**
 * Components
 */
import { Logo } from '@/components/common/Logo';
import { Button } from '@/components/ui/button';
import {
  Tooltip,
  TooltipTrigger,
  TooltipContent,
} from '@/components/ui/tooltip';

/**
 * Assets
 */
import { Facebook, Instagram, Linkedin, Youtube } from 'lucide-react';

/**
 * Constants
 */
const SOCIAL_LINKS = [
  {
    href: 'https://facebook.com/tran.bao.34441',
    Icon: Facebook,
    label: 'Facebook Page',
  },
  {
    href: 'https://www.instagram.com/_nothing.27_/',
    Icon: Instagram,
    label: 'Instagram Page',
  },
  {
    href: 'http://www.linkedin.com/in/bảo-trần-534345346',
    Icon: Linkedin,
    label: 'Linkedin Page',
  },
  {
    href: 'http://youtube.com',
    Icon: Youtube,
    label: 'Youtube Page',
  },
] as const;

export const Footer = ({
  className,
  ...props
}: React.ComponentProps<'footer'>) => {
  return (
    <footer
      className={cn('border-t', className)}
      {...props}
    >
      <div className='container py-8 grid max-md:justify-items-center md:grid-cols-[1fr_3fr_1fr] md:items-center'>
        <Logo />

        <p className='text-muted-foreground order-1 max-md:text-center md:order-none md:justify-self-center'>
          &copy; {new Date().getFullYear()} TranBao. All right reserved.
        </p>

        <ul className='flex items-center gap-1 max-md:mt-6 max-md:mb-4 md:justify-self-end'>
          {SOCIAL_LINKS.map(({ href, Icon, label }) => (
            <li key={href}>
              <Tooltip>
                <TooltipTrigger asChild>
                  <Button
                    variant='ghost'
                    size='icon'
                    aria-label={label}
                    asChild
                  >
                    <a
                      href={href}
                      target='_blank'
                    >
                      <Icon />
                    </a>
                  </Button>
                </TooltipTrigger>

                <TooltipContent>{label}</TooltipContent>
              </Tooltip>
            </li>
          ))}
        </ul>
      </div>
    </footer>
  );
};
