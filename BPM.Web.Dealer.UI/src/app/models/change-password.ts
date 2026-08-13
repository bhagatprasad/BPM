export interface ChangePassword {
    userId?: string;
    newPassword?: string;
    confirmPassword?: string;
    modifiedBy?: string;
    resetPassword?: false;
}
