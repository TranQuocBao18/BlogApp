/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { Link, useFetcher, useNavigate } from 'react-router';
import { z } from 'zod';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useCallback, useEffect } from 'react';
import { toast } from 'sonner';

/**
 * Custome modules
 */
import { cn } from '@/lib/utils';

/**
 * Components
 */
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { InputPassword } from '@/components/auth/InputPassword';
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group';
import { Label } from '@/components/ui/label';

/**
 * Assets
 */
import { signupBanner } from '@/assets';
import { LoaderCircleIcon } from 'lucide-react';

/**
 * Types
 */
import type {
  ActionResponse,
  AuthResponse,
  ErrorResponse,
  SignupResponse,
  ValidationError,
} from '@/types';
type SignupField = 'username' | 'email' | 'fullname' | 'phoneNumber';

/**
 * Constants
 */
const SIGNUP_FORM = {
  title: 'Create an account',
  description: 'Enter your email below to create an account',
  footerText: 'Already have an account ?',
} as const;

/**
 * Signup form schema
 */
const formSchema = z.object({
  username: z
    .string()
    .nonempty('Username is required')
    .max(50, 'Username must be less than 50 characters'),
  email: z
    .string()
    .nonempty('Email is required')
    .max(50, 'Email must be less than 50 characters')
    .email('Invalid email address'),
  password: z
    .string()
    .nonempty('Password is required')
    .min(8, 'Password must be at least 8 characters long'),
  fullname: z
    .string()
    .nonempty('Fullname is required')
    .max(50, 'Fullname must be less than 50 characters'),
  phoneNumber: z
    .string()
    .nonempty('Phone number is required')
    .max(10, 'Phone number must be 10 characters'),
});

export const SignupForm = ({
  className,
  ...props
}: React.ComponentProps<'div'>) => {
  const navigate = useNavigate();
  const fetcher = useFetcher();
  const signupResponse = fetcher.data;

  const isLoading = fetcher.state !== 'idle';

  //React hook form initial
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      username: '',
      email: '',
      password: '',
      fullname: '',
      phoneNumber: '',
    },
  });

  //Handle server error response
  useEffect(() => {
    if (!signupResponse) return;

    if (signupResponse.ok) {
      toast.success('Signup successful!');
      navigate('/login', { viewTransition: true });
      return;
    }

    if (!signupResponse.err) return;

    // if (signupResponse.err.code === 'AuthorizationError') {
    //   console.log('Showing toast for AuthorizationError');
    //   const authorizationError = signupResponse.err as ErrorResponse;

    //   toast.error(authorizationError.message, {
    //     position: 'top-center',
    //   });
    //   return;
    // }

    if (signupResponse.err.code === 'ValidationError') {
      const validationError = signupResponse.err as ValidationError;

      Object.entries(validationError.errors).forEach((value) => {
        const [, validationError] = value;
        const signupField = validationError.path as SignupField;

        form.setError(
          signupField,
          {
            type: 'custom',
            message: validationError.msg,
          },
          { shouldFocus: true },
        );
      });
    }
  }, [signupResponse]);

  //Handle form submition
  const onSubmit = useCallback(async (values: z.infer<typeof formSchema>) => {
    // const formData = new FormData();
    // formData.append('username', values.username);
    // formData.append('email', values.email);
    // formData.append('fullname', values.fullname);
    // formData.append('phoneNumber', values.phoneNumber);

    await fetcher.submit(values, {
      action: '/signup',
      method: 'post',
      encType: 'application/json',
    });
  }, []);

  return (
    <div
      className={cn('flex flex-col gap-6', className)}
      {...props}
    >
      <Card className='overflow-hidden p-0'>
        <CardContent className='grid p-0 md:grid-cols-2'>
          <Form {...form}>
            <form
              className='p-6 md:p-8'
              onSubmit={form.handleSubmit(onSubmit)}
            >
              <div className='flex flex-col gap-6'>
                <div className='flex flex-col items-center text-center'>
                  <h1 className='text-2xl font-semibold'>
                    {SIGNUP_FORM.title}
                  </h1>

                  <p className='text-muted-foreground text-sm px-6'>
                    {SIGNUP_FORM.description}
                  </p>
                </div>

                <FormField
                  control={form.control}
                  name='username'
                  render={({ field }) => (
                    <FormItem className='grid gap-3'>
                      <FormLabel>Username</FormLabel>
                      <FormControl>
                        <Input
                          placeholder='John'
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name='email'
                  render={({ field }) => (
                    <FormItem className='grid gap-3'>
                      <FormLabel>Email</FormLabel>
                      <FormControl>
                        <Input
                          placeholder='join@example.com'
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name='password'
                  render={({ field }) => (
                    <FormItem className='grid gap-3'>
                      <FormLabel>Password</FormLabel>
                      <FormControl>
                        <InputPassword
                          placeholder='Enter your password here'
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name='fullname'
                  render={({ field }) => (
                    <FormItem className='grid gap-3'>
                      <FormLabel>Fullname</FormLabel>
                      <FormControl>
                        <Input
                          placeholder='John Doe'
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name='phoneNumber'
                  render={({ field }) => (
                    <FormItem className='grid gap-3'>
                      <FormLabel>Phone number</FormLabel>
                      <FormControl>
                        <Input
                          placeholder='Type your phone number here...'
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <Button
                  type='submit'
                  className='w-full'
                  disabled={isLoading}
                >
                  {isLoading && <LoaderCircleIcon className='animate-spin' />}

                  <span>Signup</span>
                </Button>
              </div>

              <div className='mt-4 text-center text-sm'>
                {SIGNUP_FORM.footerText}{' '}
                <Link
                  to='/login'
                  className='underline underline-offset-4 hover:text-primary'
                  viewTransition
                >
                  Log in
                </Link>
              </div>
            </form>
          </Form>

          <figure className='bg-muted relative hidden md:block'>
            <img
              src={signupBanner}
              width={400}
              height={400}
              alt='Login banner'
              className='absolute inset-0 w-full h-full object-cover'
            />
          </figure>
        </CardContent>
      </Card>

      <div className='text-muted-foreground *:[a]:hover:text-primary text-center text-xs text-balance *:[a]:underline *:[a]:underline-offset-4'>
        By clicking continue, you agree to our <a href='#'>Terms of Service</a>{' '}
        and <a href='#'>Privacy Policy</a>
      </div>
    </div>
  );
};
