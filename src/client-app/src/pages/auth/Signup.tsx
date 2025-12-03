/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Components
 */
import { SignupForm } from '@/components/auth/SignupForm';

export const Signup = () => {
  return (
    <div className='h-dvh flex items-center justify-center p-6 md:p-10'>
      <div className='w-full max-w-sm md:max-w-3xl'>
        <SignupForm />
      </div>
    </div>
  );
};
