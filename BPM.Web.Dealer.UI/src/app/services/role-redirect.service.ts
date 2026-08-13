// services/role-redirect.service.ts
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from './account.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { AuthResponse } from '../models/auth-response';

@Injectable({
  providedIn: 'root'
})
export class RoleRedirectService {
  constructor(
    private router: Router,
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}

  /**
   * Redirect user based on their role
   */
  redirectBasedOnRole(authResponse: AuthResponse | null): void {
    if (!authResponse) {
      this.router.navigate(['/login']);
      return;
    }

    const roleName = authResponse?.authenticateResponseDto?.roleInfo?.name?.toLowerCase() || '';
    const dealerInfo = authResponse?.authenticateResponseDto?.dealerInfo;
    const isAdminOrOperator = roleName === 'administrator' || roleName === 'operator';
    
    console.log('Redirecting based on role:', roleName);
    
    // Check if user has access
    if (dealerInfo || isAdminOrOperator) {
      // Role-based redirect
      if (roleName === 'administrator' || roleName === 'admin') {
        this.router.navigate(['/admin/dashboard']);
        this.toastr.info('Welcome Admin!', 'Success');
      } else if (roleName === 'operator') {
        this.router.navigate(['/operator/dashboard']);
        this.toastr.info('Welcome Operator!', 'Success');
      } else if (roleName === 'user' || roleName === 'dealer') {
        this.router.navigate(['/operator/dashboard']);
        this.toastr.info('Welcome!', 'Success');
      } else {
        // Fallback to drugs page
        this.router.navigate(['/drugs']);
        this.toastr.info('Welcome!', 'Success');
      }
    } else {
      // User doesn't have access
      this.accountService.logout();
      this.router.navigate(['/login']);
      this.toastr.error('You do not have access to this portal.', 'Access Denied');
    }
  }

  /**
   * Get redirect path based on user role
   */
  getRedirectPath(): string {
    const user = this.accountService.getCurrentUser();
    if (!user) return '/login';
    
    const roleName = user?.authenticateResponseDto?.roleInfo?.name?.toLowerCase() || '';
    const dealerInfo = user?.authenticateResponseDto?.dealerInfo;
    const isAdminOrOperator = roleName === 'administrator' || roleName === 'operator';
    
    if (!dealerInfo && !isAdminOrOperator) {
      return '/login';
    }
    
    if (roleName === 'administrator' || roleName === 'admin') {
      return '/admin/dashboard';
    } else if (roleName === 'operator') {
      return '/operator/dashboard';
    } else if (roleName === 'user' || roleName === 'dealer') {
      return '/operator/dashboard';
    }
    return '/drugs';
  }

  /**
   * Get user's base path for navigation
   */
  getBasePath(): string {
    const user = this.accountService.getCurrentUser();
    if (!user) return '';
    
    const roleName = user?.authenticateResponseDto?.roleInfo?.name?.toLowerCase() || '';
    
    if (roleName === 'administrator' || roleName === 'admin') {
      return '/admin';
    } else if (roleName === 'operator' || roleName === 'user') {
      return '/operator';
    }
    return '';
  }

  /**
   * Check if user is admin
   */
  isAdmin(): boolean {
    const user = this.accountService.getCurrentUser();
    if (!user) return false;
    const roleName = user?.authenticateResponseDto?.roleInfo?.name?.toLowerCase() || '';
    return roleName === 'administrator' || roleName === 'admin';
  }

  /**
   * Check if user is operator
   */
  isOperator(): boolean {
    const user = this.accountService.getCurrentUser();
    if (!user) return false;
    const roleName = user?.authenticateResponseDto?.roleInfo?.name?.toLowerCase() || '';
    return roleName === 'operator' || roleName === 'user';
  }

  /**
   * Redirect to dashboard based on current user
   */
  redirectToDashboard(): void {
    const user = this.accountService.getCurrentUser();
    if (user) {
      this.redirectBasedOnRole(user);
    } else {
      this.router.navigate(['/login']);
    }
  }
}