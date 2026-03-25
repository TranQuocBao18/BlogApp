/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { motion } from 'motion/react';
import { Link } from 'react-router';

/**
 * Components
 */
const MotionLink = motion.create(Link);

/**
 * Assets
 */
import { logoDark, logoLight } from '@/assets';

export const Logo = () => {
  return (
    <MotionLink
      to='/'
      className='text-primary text-lg font-semibold'
      whileHover={{ scale: 1.05 }}
      whileTap={{ scale: 0.9 }}
      viewTransition
    >
      <img
        src={logoLight}
        width={45}
        height={45}
        className='hidden dark:block'
        alt='logo'
      />
      <img
        src={logoDark}
        width={45}
        height={45}
        className='dark:hidden'
        alt='logo'
      />
    </MotionLink>
  );
};
