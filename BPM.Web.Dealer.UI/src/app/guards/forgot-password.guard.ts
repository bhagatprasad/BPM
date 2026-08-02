import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';

export const forgotPasswordGuard: CanActivateFn = async (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  console.log('🔵 forgotPasswordGuard called');

  // Check if user is authenticated
  const isAuth = await accountService.isAuthenticated();
  
  if (isAuth) {
    const currentUser = accountService.getCurrentUser();
    
    if (currentUser?.jwtToken) {
      const dealerInfo = currentUser?.authenticateResponseDto?.dealerInfo;
      const roleName = currentUser?.authenticateResponseDto?.roleInfo?.name;
      const isAdminOrOperator = roleName === "Administrator" || roleName === "Operator";
      
      // If already logged in and has access, redirect to drugs
      if (dealerInfo || isAdminOrOperator) {
        console.log('✅ User already logged in, redirecting to drugs');
        router.navigateByUrl('/drugs');
        return false;
      } else {
        // User doesn't have access, clear auth and allow forgot password
        accountService.logout();
      }
    }
  }

  // Allow access to forgot-password page
  console.log('✅ Allowing access to forgot-password');
  return true;
};