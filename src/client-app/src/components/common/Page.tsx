/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */

/**
 * Types
 */
import type React from 'react';

export const Page = ({ children }: React.PropsWithChildren) => {
  return <div className='pt-24 pb-10'>{children}</div>;
};
