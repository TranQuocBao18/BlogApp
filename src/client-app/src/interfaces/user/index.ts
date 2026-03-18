/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

export interface UserRole {
  id: string;
  roleName: string;
}

export type IUserForm = {
  id?: string;
  username: string;
  email: string;
  name: string;
  phone: string;
  groupId?: string;
  groupName?: string;
};

export class UserForm implements IUserForm {
  isAdmin: boolean = false;
  name: string = '';
  phone: string = '';
  id?: string | undefined = '';
  username: string = '';
  email: string = '';
}

export interface IAddEditUser {
  id?: string;
  username: string;
  email: string;
  fullname: string;
  phoneNumber: string;
}

export interface User {
  id: string;
  username?: string;
  fullName?: string;
  email?: string;
  phoneNumber?: string;
  isAdmin: boolean;
  created: string;
  lastModified?: string;
}
