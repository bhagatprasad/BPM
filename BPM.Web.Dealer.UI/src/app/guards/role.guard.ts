// guards/role.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';

export const roleGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  const toastr = inject(ToastrService);

  // Get current user
  const currentUser = accountService.getCurrentUser();
  
  if (!currentUser) {
    toastr.warning('Please login to access this page.', 'Authentication Required');
    router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }

  // Get user role
  const userRole = currentUser?.authenticateResponseDto?.roleInfo?.name || '';
  const allowedRoles = route.data?.['roles'] as string[] || [];

  // If no roles are specified, allow access (fallback)
  if (allowedRoles.length === 0) {
    return true;
  }

  // Check if user has any of the allowed roles (case-insensitive)
  const hasRole = allowedRoles.some(role => 
    userRole.toLowerCase() === role.toLowerCase()
  );

  if (!hasRole) {
    toastr.error('You do not have permission to access this page.', 'Access Denied');
    
    // Redirect based on user role
    if (userRole.toLowerCase() === 'administrator' || userRole.toLowerCase() === 'admin') {
      router.navigate(['/admin/dashboard']);
    } else if (userRole.toLowerCase() === 'operator') {
      router.navigate(['/operator/dashboard']);
    } else if (userRole.toLowerCase() === 'user') {
      router.navigate(['/operator/dashboard']);
    } else {
      // Fallback to login if role is unknown
      router.navigate(['/login']);
    }
    return false;
  }

  return true;
};