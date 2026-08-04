import { isActive } from "@angular/router";

// Define the authentication response interface
export interface AuthResponse {
  authenticateResponseDto: {
    userId: string;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    isActive: boolean;
    roleId: string;
    dealerId: string;
    dealerInfo: {
      id: string;
      dealershipName: string;
      contactPerson: string;
      email: string;
      phone: string;
      alternatePhone: string;
      addressLine1: string;
      addressLine2: string;
      city: string;
      state: string;
      country: string;
      postalCode: string;
      gstNumber: string;
      registrationNumber: string;
      tradeLicenseNumber: string;
      website: string;
      isActive: boolean;
    },
    roleInfo: {
      id: string,
      name: string,
      isActive: boolean,
      code: string
    }
  };
  jwtToken: string;
  refreshToken: string;
  message: string;
  isValidUser: boolean;
  isValidPassword: boolean;
}
