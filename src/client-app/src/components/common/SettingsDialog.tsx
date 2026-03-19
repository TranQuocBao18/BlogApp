/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { zodResolver } from '@hookform/resolvers/zod';
import { useCallback, useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { z } from 'zod';

/**
 * Custom modules
 */
import {
  apiChangePasswordUser,
  apiGetProfile,
  apiUpdateProfile,
} from '@/api/user.api';

/**
 * Components
 */
import { InputPassword } from '@/components/common/InputPassword';
import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Separator } from '@/components/ui/separator';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';

/**
 * Custom hooks
 */

/**
 * Assets
 */
import { AtSignIcon, Loader2Icon, MailIcon } from 'lucide-react';

/**
 * Types
 */
import type { ProfileResponse } from '@/interfaces/auth';
import type { Response } from '@/types';
import type { DialogProps } from '@radix-ui/react-dialog';

/**
 * Profile form schema
 */
const profileFormSchema = z.object({
  username: z
    .string()
    .max(20, 'Username must be less than 20 characters')
    .optional(),
  fullname: z
    .string()
    .max(50, 'Full name must be less than 50 characters')
    .optional(),
  email: z
    .string()
    .max(50, 'Email must be less than 50 characters')
    .email('Invalid email address')
    .optional(),
  phoneNumber: z
    .string()
    .max(11, 'Phone number must be less than 11 characters')
    .optional(),
});

const ProfileSettingsForm = ({ onSuccess }: { onSuccess?: () => void }) => {
  const [loading, setLoading] = useState(false);
  const [profileData, setProfileData] = useState<ProfileResponse | null>(null);

  //React hook form initial
  const form = useForm<z.infer<typeof profileFormSchema>>({
    resolver: zodResolver(profileFormSchema),
    defaultValues: {
      username: '',
      fullname: '',
      email: '',
      phoneNumber: '',
    },
  });

  // Fetch profile data
  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = (await apiGetProfile()) as Response<ProfileResponse>;

        // Check if the response indicates success
        if (response?.succeeded) {
          const profile = response.data as ProfileResponse;
          setProfileData(profile);
          form.reset({
            username: profile.userName || '',
            fullname: profile.fullName || '',
            email: profile.email || '',
            phoneNumber: profile.phoneNumber || '',
          });
        } else {
          console.error(
            'Failed to fetch profile:',
            response?.message || 'Unknown error',
          );
        }
      } catch (error: any) {
        console.error('Failed to fetch profile:', error);
        // Only show error toast if it's not a 403/401 auth error
        if (
          error?.response?.status !== 403 &&
          error?.response?.status !== 401
        ) {
          toast.error('Failed to load profile data');
        }
      }
    };

    fetchProfile();
  }, [form]);

  //Handle form submit
  const onSubmit = useCallback(
    async (values: z.infer<typeof profileFormSchema>) => {
      setLoading(true);
      try {
        const response = (await apiUpdateProfile({
          id: profileData?.id,
          username: values.username || '',
          email: values.email || '',
          fullname: values.fullname || '',
          phoneNumber: values.phoneNumber || '',
        })) as Response;

        // Check if the response indicates success
        if (response?.succeeded) {
          toast.success('Profile has been updated successfully');
          onSuccess?.();
        } else {
          // Show error message from response
          const errorMessage = response?.message || 'Failed to update profile';
          toast.error(errorMessage);
        }
      } catch (error) {
        console.error('Failed to update profile:', error);
        toast.error('Failed to update profile');
      } finally {
        setLoading(false);
      }
    },
    [profileData?.id, onSuccess],
  );

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        <div>
          <h3 className='font-semibold text-lg'>Personal info</h3>

          <p className='text-sm text-muted-foreground'>
            Update your personal details here.
          </p>

          <Separator className='my-5' />
        </div>

        <FormField
          control={form.control}
          name='username'
          render={({ field }) => (
            <FormItem className='grid gap-2 items-start lg:grid-cols-[1fr_2fr]'>
              <FormLabel>Username</FormLabel>

              <div className='space-y-2'>
                <div className='relative'>
                  <AtSignIcon
                    size={20}
                    className='absolute top-1/2 left-3 -translate-y-1/2 pointer-events-none text-muted-foreground'
                  />

                  <FormControl>
                    <Input
                      type='text'
                      placeholder='johndoe'
                      className='ps-10'
                      {...field}
                    />
                  </FormControl>
                </div>

                <FormMessage />
              </div>
            </FormItem>
          )}
        />

        <Separator className='my-5' />

        <FormField
          control={form.control}
          name='fullname'
          render={({ field }) => (
            <FormItem className='grid gap-2 items-start lg:grid-cols-[1fr_2fr]'>
              <FormLabel>Full name</FormLabel>

              <FormControl>
                <Input
                  type='text'
                  placeholder='John Doe'
                  {...field}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        <Separator className='my-5' />

        <FormField
          control={form.control}
          name='email'
          render={({ field }) => (
            <FormItem className='grid gap-2 items-start lg:grid-cols-[1fr_2fr]'>
              <FormLabel>Email address</FormLabel>

              <div className='space-y-2'>
                <div className='relative'>
                  <MailIcon
                    size={20}
                    className='absolute top-1/2 left-3 -translate-y-1/2 pointer-events-none text-muted-foreground'
                  />

                  <FormControl>
                    <Input
                      type='email'
                      placeholder='john@example.com'
                      className='ps-10'
                      {...field}
                    />
                  </FormControl>
                </div>

                <FormMessage />
              </div>
            </FormItem>
          )}
        />

        <Separator className='my-5' />

        <FormField
          control={form.control}
          name='phoneNumber'
          render={({ field }) => (
            <FormItem className='grid gap-2 items-start lg:grid-cols-[1fr_2fr]'>
              <FormLabel>Phone number</FormLabel>

              <FormControl>
                <Input
                  type='tel'
                  placeholder='+1 (555) 000-0000'
                  {...field}
                />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        <Separator className='my-5' />

        <div className='flex justify-end gap-3'>
          <Button
            variant='outline'
            asChild
          >
            <DialogClose>Cancel</DialogClose>
          </Button>

          <Button
            type='submit'
            disabled={loading}
          >
            {loading && <Loader2Icon className='animate-spin' />}
            {loading ? 'Saving' : 'Save'}
          </Button>
        </div>
      </form>
    </Form>
  );
};

/**
 * Password form schema
 */
const passwordFormSchema = z
  .object({
    oldPassword: z
      .string()
      .min(8, 'Current password must be at least 8 characters long'),
    password: z
      .string()
      .min(8, 'New password must be at least 8 characters long'),
    confirm_password: z.string(),
  })
  .refine((data) => data.password === data.confirm_password, {
    message: "Password doesn't match",
    path: ['confirm_password'],
  });

const PasswordSettingsForm = () => {
  const [loading, setLoading] = useState(false);

  //React hook form initial
  const form = useForm<z.infer<typeof passwordFormSchema>>({
    resolver: zodResolver(passwordFormSchema),
    defaultValues: {
      oldPassword: '',
      password: '',
      confirm_password: '',
    },
  });

  //Handle form submit
  const onSubmit = useCallback(
    async (values: z.infer<typeof passwordFormSchema>) => {
      setLoading(true);
      try {
        const response = (await apiChangePasswordUser(
          values.oldPassword,
          values.password,
        )) as Response;

        // Check if the response indicates success
        if (response?.succeeded) {
          toast.success('Password has been updated successfully');
          form.reset();
        } else {
          // Show error message from response
          const errorMessage = response?.message || 'Failed to update password';
          toast.error(errorMessage);
        }
      } catch (error) {
        console.error('Failed to update password:', error);
        toast.error('Failed to update password');
      } finally {
        setLoading(false);
      }
    },
    [form],
  );

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        <div>
          <h3 className='font-semibold text-lg'>Password</h3>

          <p className='text-sm text-muted-foreground'>
            Please enter your current password to change your password.
          </p>

          <Separator className='my-5' />
        </div>

        <FormField
          control={form.control}
          name='oldPassword'
          render={({ field }) => (
            <FormItem className='grid gap-2 items-start lg:grid-cols-[1fr_2fr]'>
              <FormLabel>Current password</FormLabel>

              <div className='space-y-2'>
                <FormControl>
                  <InputPassword
                    placeholder='••••••••'
                    {...field}
                  />
                </FormControl>

                <FormMessage />
              </div>
            </FormItem>
          )}
        />

        <Separator className='my-5' />

        <FormField
          control={form.control}
          name='password'
          render={({ field }) => (
            <FormItem className='grid gap-2 items-start lg:grid-cols-[1fr_2fr]'>
              <FormLabel>New password</FormLabel>

              <div className='space-y-2'>
                <FormControl>
                  <InputPassword
                    placeholder='••••••••'
                    {...field}
                  />
                </FormControl>

                <FormMessage />
              </div>
            </FormItem>
          )}
        />

        <Separator className='my-5' />

        <FormField
          control={form.control}
          name='confirm_password'
          render={({ field }) => (
            <FormItem className='grid gap-2 items-start lg:grid-cols-[1fr_2fr]'>
              <FormLabel>Confirm new password</FormLabel>

              <div className='space-y-2'>
                <FormControl>
                  <InputPassword
                    placeholder='••••••••'
                    {...field}
                  />
                </FormControl>

                <FormMessage />
              </div>
            </FormItem>
          )}
        />

        <Separator className='my-5' />

        <div className='flex justify-end gap-3'>
          <Button
            variant='outline'
            asChild
          >
            <DialogClose>Cancel</DialogClose>
          </Button>

          <Button
            type='submit'
            disabled={loading}
          >
            {loading && <Loader2Icon className='animate-spin' />}
            {loading ? 'Updating' : 'Update'}
          </Button>
        </div>
      </form>
    </Form>
  );
};

export const SettingsDialog = ({
  children,
  ...props
}: React.PropsWithChildren<DialogProps>) => {
  const [open, setOpen] = useState(false);

  const handleSuccess = () => {
    setOpen(false);
  };

  return (
    <Dialog
      open={open}
      onOpenChange={setOpen}
      {...props}
    >
      <DialogTrigger asChild>{children}</DialogTrigger>

      <DialogContent className='md:min-w-[80vw] xl:min-w-4xl'>
        <DialogHeader>
          <DialogTitle className='text-2xl'>Settings</DialogTitle>
        </DialogHeader>

        <Tabs
          defaultValue='profile'
          className='gap-5'
        >
          <TabsList className='grid grid-cols-2 w-full'>
            <TabsTrigger value='profile'>Profile</TabsTrigger>

            <TabsTrigger value='password'>Password</TabsTrigger>
          </TabsList>

          <TabsContent value='profile'>
            <ProfileSettingsForm onSuccess={handleSuccess} />
          </TabsContent>

          <TabsContent value='password'>
            <PasswordSettingsForm />
          </TabsContent>
        </Tabs>
      </DialogContent>
    </Dialog>
  );
};
