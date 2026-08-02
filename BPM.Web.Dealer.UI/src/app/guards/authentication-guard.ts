import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';

export const authenticationGuard: CanActivateFn = async (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  const toastr = inject(ToastrService);

  // Check if user is authenticated
  const isAuth = await accountService.isAuthenticated();
  
  if (!isAuth) {
    toastr.warning('Please login to access this page.', 'Authentication Required');
    router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }

  // Get current user data
  const currentUser = accountService.getCurrentUser();
  
  if (currentUser?.jwtToken) {
    // Check if user has dealer access or is Admin/Operator
    const dealerInfo = currentUser?.authenticateResponseDto?.dealerInfo;
    const roleName = currentUser?.authenticateResponseDto?.roleInfo?.name;
    const isAdminOrOperator = roleName === "Administrator" || roleName === "Operator";
    
    // User has access if they have dealer OR are Admin/Operator
    if (dealerInfo || isAdminOrOperator) {
      return true;
    } else {
      // User doesn't have dealer access and is not Admin/Operator
      toastr.error('You are not authorized to access this portal.', 'Access Denied');
      accountService.logout();
      router.navigate(['/login']);
      return false;
    }
  }

  // If token is missing or invalid
  accountService.logout();
  router.navigate(['/login']);
  return false;
};