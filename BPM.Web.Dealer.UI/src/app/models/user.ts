import { DealerInfo } from "./user-profile";

export interface User { }

export interface usersRequest {
    dealerId: string;
}
export interface userInformation{


    userId?: string;
    firstName?: string;
    lastName?: string;
    email?: string;
    phone?: string;
    isActive?: boolean;
    dealerId?: string;
    roleId?: string;
    dealerInfo?: DealerInfo[];
    roleinfo?: roleInfo[];    
}

  export interface roleInfo {
      id: string,
      name: string,
      isActive: boolean,
      code: string
    }