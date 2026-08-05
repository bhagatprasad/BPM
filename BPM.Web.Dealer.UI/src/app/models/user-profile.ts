export interface User { }

export interface DealerInfo {
  addressLine1: string;
  addressLine2: string;
  alternatePhone: string;
  city: string;
  contactPerson: string;
  country: string;
  dealershipName: string;
  email: string;
  gstNumber: string;
  id: string;
  isActive: boolean;
  phone: string;
  postalCode: string;
  registrationNumber: string;
  state: string;
  tradeLicenseNumber: string;
  website: string;
}

export interface AuthenticateResponse {
  authenticateResponseDto: any;
  dealerId: string;
  dealerInfo: DealerInfo;
  email: string;
  firstName: string;
  isActive: boolean;
  lastName: string;
  phone: string;
  roleId: string;
  userId: string;
  isValidPassword: boolean;
  isValidUser: boolean;
  jwtToken: string;
  message: string;
  refreshToken: string;
}


export interface UpdateUserRequest {
  userId: string,
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  modifiedBy: string
}

export interface UpdateUserResponse {
  message?: string;
  data?: userDto;
}
export interface userDto {
  userId?: string;
  firstName?: string;
  lastName?: string;
  email?: string;
  phone?: string;
}


export interface ChangePasswordRequest{
  userId?: string;
  newPassword?: string;
  modifiedBy?: string;
}

export interface ChangePasswordResponse{
  message? : string;
}