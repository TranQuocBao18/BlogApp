/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { redirect } from 'react-router';

/**
 * Custom modules
 */
import { apiAddUser } from '@/api/user.api';

/**
 * Components
 */
import { toast } from 'sonner';

/**
 * Types
 */
import type { ActionFunction } from 'react-router';
import { AxiosError } from 'axios';
import type { ActionResponse, ErrorResponse, ApiResponse } from '@/types';
import type { IAddEditUser } from '@/interfaces/user';

const signupAction: ActionFunction = async ({ request }) => {
  const data = await request.json();

  try {
    const response = await apiAddUser(data);

    if (response.data.succeeded) {
      return {
        ok: true,
        data: response.data,
      } as ActionResponse;
    } else {
      // Business logic failed nhưng HTTP OK
      return {
        ok: false,
        err: {
          code: 'BadRequest',
          message: response.message || 'SignUp failed',
        } as ErrorResponse,
      } as ActionResponse;
    }
  } catch (err) {
    if (err instanceof AxiosError) {
      return {
        ok: false,
        err: err.response?.data,
      } as ActionResponse;
    }

    throw err;
  }
};

export default signupAction;
