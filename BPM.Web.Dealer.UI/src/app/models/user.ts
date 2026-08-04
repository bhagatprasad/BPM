export interface User { }

export interface usersRequest {
    dealerId: string;
}

export interface UsersResponse {
    userId?: string;
    firstName?: string;
    lastName?: string;
    email?: string;
    phone?: string;
}
