/**
 * @copyright 2025 Tran Bao
 * @license Apache-2.0
 */

export interface LoginParams {
  ipAddress?: string;
  email: string;
  password: string;
  rememberMe: boolean;
}

export interface RegisterParams {
  id?: string;
  username: string;
  email: string;
  fullname: string;
  phoneNumber: string;
  isAdmin: boolean;
}

export interface JWTDecode {
  sub: string;
  jti: string;
  email: string;
  uid: string;
  ip: string;
  roles: string[];
  permission: string[];
  exp: number;
  iss: string;
  aud: string;
}

export interface AuthResponse {
  id: string;
  userName: string;
  displayName: string;
  email: string;
  groupCode: string;
  roles: string[];
  isVerified: boolean;
  jwToken: string;
}

export interface ProfileResponse {
  id: string;
  userName?: string;
  displayName?: string;
  email?: string;
  fullName?: string;
  phoneNumber?: string;
  roles?: string[];
}

export interface SignupResponse {
  userId: string;
}
