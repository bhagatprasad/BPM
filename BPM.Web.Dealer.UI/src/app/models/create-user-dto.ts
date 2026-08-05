export interface CreateUserDto {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    password: string;
    isActive: boolean;
    dealerId: string;
    roleId: string;
}