/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import {
  isRouteErrorResponse,
  Navigate,
  useNavigate,
  useRouteError,
} from 'react-router';

/**
 * Components
 */
import { Button } from '@/components/ui/button';

/**
 * Constants
 */
import { AppRouters } from '@/constants/router';

const getErrorMessage = (status: number, data: unknown): string => {
  const dataStr =
    typeof data === 'string' ? data : JSON.stringify(data, null, 2);

  const statusMessages: Record<number, string> = {
    400: 'Invalid request. Please check your input.',
    401: 'Your session has expired or you are not authorized. Please login again.',
    403: 'You do not have permission to access this resource.',
    404: 'The page you are looking for does not exist.',
    500: 'Something went wrong on our server. Please try again later.',
    503: 'The service is temporarily unavailable. Please try again later.',
  };

  return statusMessages[status] || dataStr || 'An error occurred.';
};

export const RootErrorBoundary = () => {
  const error = useRouteError();
  const navigate = useNavigate();

  if (isRouteErrorResponse(error)) {
    const isUnauthorized = error.status === 401;
    const isForbidden = error.status === 403;

    // Redirect to login for 401 errors
    if (isUnauthorized) {
      return <Navigate to={AppRouters.LOGIN} />;
    }

    // Redirect to home for 403 errors
    if (isForbidden) {
      return <Navigate to={AppRouters.HOME} />;
    }

    const errorMessage = getErrorMessage(error.status, error.data);

    return (
      <div className='h-dvh grid place-content-center place-items-center gap-6 px-4'>
        <div className='text-center space-y-3'>
          <h1 className='text-5xl font-bold text-destructive'>
            {error.status}
          </h1>

          <h2 className='text-2xl font-semibold text-foreground'>
            {error.statusText || 'Error'}
          </h2>

          <p className='text-muted-foreground max-w-[60ch] text-base leading-relaxed'>
            {errorMessage}
          </p>
        </div>

        <div className='flex gap-3'>
          <Button
            variant='outline'
            onClick={() => navigate(-1)}
          >
            Go Back
          </Button>
          <Button onClick={() => navigate(AppRouters.HOME)}>Home</Button>
        </div>
      </div>
    );
  }

  // Handle non-route errors
  if (error instanceof Error) {
    return (
      <div className='h-dvh grid place-content-center place-items-center gap-6 px-4'>
        <div className='text-center space-y-3'>
          <h1 className='text-5xl font-bold text-destructive'>Error</h1>

          <p className='text-muted-foreground max-w-[60ch] text-base'>
            {error.message || 'An unexpected error occurred.'}
          </p>

          {import.meta.env.DEV && (
            <details className='mt-4 text-left bg-muted p-4 rounded-md max-w-xl'>
              <summary className='cursor-pointer font-semibold'>
                Stack Trace (Dev Only)
              </summary>
              <pre className='mt-2 text-xs overflow-auto'>{error.stack}</pre>
            </details>
          )}
        </div>

        <div className='flex gap-3'>
          <Button
            variant='outline'
            onClick={() => navigate(-1)}
          >
            Go Back
          </Button>
          <Button onClick={() => navigate(AppRouters.HOME)}>Home</Button>
        </div>
      </div>
    );
  }

  // Unknown error
  return (
    <div className='h-dvh grid place-content-center place-items-center gap-6'>
      <div className='text-center space-y-3'>
        <h1 className='text-5xl font-bold text-destructive'>Oops!</h1>
        <p className='text-muted-foreground'>An unknown error occurred.</p>
      </div>

      <Button onClick={() => navigate(AppRouters.HOME)}>Back to Home</Button>
    </div>
  );
};
