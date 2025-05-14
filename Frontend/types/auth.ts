// types/auth.ts
import type { Role } from "~/types/index";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  fullname: string;
  password: string;
  confirmPassword: string;
  role: Role;
}

export interface UserInfoResponse {
  userId: string;
  email: string;
  roles: string[];
}

export interface AuthResult {
  success: boolean;
  error?: string;
  data?: any; // Optional data to return
}
