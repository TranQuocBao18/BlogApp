/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

export const ENV_CONFIG = {
  API: import.meta.env.VITE_API_DOMAIN,
  GOOGLE_API_KEY: import.meta.env.VITE_GOOGLE_API_KEY,
  STORAGE: `${import.meta.env.VITE_API_DOMAIN}/Storage/`,
  API_VERSION: import.meta.env.VITE_API_VERSION,
  LICENSE_INFO: import.meta.env.VITE_LICENSE_INFO,
};

export const APP_CONFIG = {
  ACCESS_TOKEN: 'ACCESS_TOKEN',
  DEBT_INFO: 'DEBT_INFO',
};
