import { DealerInfo } from './user-profile';
import { DistributorInfo } from './distributor';

export interface User {}

export interface usersRequest {
  dealerId: string;
}
export interface userInformation {
  userId?: string;
  firstName?: string;
  lastName?: string;
  email?: string;
  phone?: string;
  isActive?: boolean;
  dealerId?: string;
  distributorId?: string;
  roleId?: string;
  dealerInfo?: DealerInfo;
  distributorInfo?: DistributorInfo;
  roleInfo?: roleInfo;
}

export interface roleInfo {
  id: string;
  name: string;
  isActive: boolean;
  code: string;
}

export interface UserDistributorUpdateDto {
  userId: string;
  distributorId?: string;
  modifiedBy?: string;
}

export interface CreateUserDto {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  password: string;
  isActive: boolean;
  dealerId: string;
  distributorId: string;
  roleId: string;
}
