// types/index.ts

export interface ApiResponse<T> {
  payload: T;
  message?: string;
}

// Problem Details object from ASP.NET Core
export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
  errors?: Record<string, string[]>; // Updated to match the backend response

  // Update to match your backend response format
  validationErrors?: Record<string, string[]>;
  errorMessages?: string[];
  extensions?: Record<string, any>;
}
// TypeScript interfaces matching backend DTOs
export interface DomainPropertiesDto {
  registrationDetails: RegistrationDetails;
  apiVersion: string;
}

export interface RegistrationDetails {
  supportedRoles: RoleDto[];
  passwordPolicy: PasswordPolicy;
}

export enum Role {}

export interface RoleDto {
  role: Role; // This will correspond to Role enum values
  roleName: string;
  description: string;
}

export interface PasswordPolicy {
  minLength: number;
  requiresUppercase: boolean;
  requiresLowercase: boolean;
  requiresDigit: boolean;
  requiresSpecialCharacter: boolean;
}
