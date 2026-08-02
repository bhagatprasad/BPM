import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';

export const loginGuard: CanActivateFn = async (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  // Check if user is already authenticated
  const isAuth = await accountService.isAuthenticated();
  
  if (isAuth) {
    const currentUser = accountService.getCurrentUser();
    
    if (currentUser?.jwtToken) {
      const dealerInfo = currentUser?.authenticateResponseDto?.dealerInfo;
      const roleName = currentUser?.authenticateResponseDto?.roleInfo?.name;
      const isAdminOrOperator = roleName === "Administrator" || roleName === "Operator";
      
      // User has access if they have dealer OR are Admin/Operator
      if (dealerInfo || isAdminOrOperator) {
        // Already authenticated and has access, redirect to drugs
        router.navigateByUrl('/drugs');
        return false;
      } else {
        // User doesn't have dealer access and is not Admin/Operator
        // Clear invalid data and allow login
        accountService.logout();
        return true;
      }
    }
  }

  // Allow access to login page
  return true;
};