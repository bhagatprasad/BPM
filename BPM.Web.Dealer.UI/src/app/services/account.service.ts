import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, of, throwError } from 'rxjs';
import { catchError, tap, map, shareReplay } from 'rxjs/operators';
import { ApiService } from '../common/services/api.service';
import { AuthResponse } from '@app/models/auth-response';
import { LoginCredentials } from '@app/models/login-credentials';
import { ForgotPasswordRequest } from '@app/models/forgot-password-request';
import { ResetPasswordRequest } from '@app/models/reset-password-request';
import { ChangePasswordRequest } from '@app/models/change-password-request';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  // Cache for authentication state
  private authState = new BehaviorSubject<boolean>(false);
  authState$ = this.authState.asObservable();
  
  // Cache for user data
  private currentUserSubject = new BehaviorSubject<AuthResponse | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(public apiService: ApiService) {
    // Initialize auth state on service creation
    this.checkAndUpdateAuthState();
  }

  /**
   * Authenticate user with username and password
   */
  authenticateAsync(credentials: LoginCredentials): Observable<AuthResponse> {
    return this.apiService.send<AuthResponse>(
      'POST',
      'Account/authenticate',
      credentials
    ).pipe(
      tap((response: AuthResponse) => {
        if (response?.jwtToken) {
          // Update auth state
          this.authState.next(true);
          this.currentUserSubject.next(response);
          
          // Store in localStorage
          localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(response));
        }
      }),
      catchError((error) => {
        this.authState.next(false);
        console.error('Authentication failed:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Check if user is authenticated
   */
  async isAuthenticated(): Promise<boolean> {
    try {
      const loggedData = localStorage.getItem('AuthenticatedUserResponse');
      if (!loggedData) {
        this.authState.next(false);
        return false;
      }
      
      const authResponse = JSON.parse(loggedData);
      const isValid = !!authResponse?.jwtToken;
      
      this.authState.next(isValid);
      if (isValid) {
        this.currentUserSubject.next(authResponse);
      }
      
      return isValid;
    } catch (error) {
      console.error('Error checking authentication:', error);
      this.authState.next(false);
      return false;
    }
  }

  /**
   * Get current authentication state synchronously
   */
  get isAuthenticatedSync(): boolean {
    return this.authState.value;
  }

  /**
   * Get current user data
   */
  getCurrentUser(): AuthResponse | null {
    try {
      const loggedData = localStorage.getItem('AuthenticatedUserResponse');
      if (!loggedData) return null;
      return JSON.parse(loggedData);
    } catch (error) {
      console.error('Error getting current user:', error);
      return null;
    }
  }

  /**
   * Get JWT token
   */
  getToken(): string | null {
    const user = this.getCurrentUser();
    return user?.jwtToken || null;
  }

  /**
   * Get refresh token
   */
  getRefreshToken(): string | null {
    const user = this.getCurrentUser();
    return user?.refreshToken || null;
  }

  /**
   * Forgot password - send reset link to email
   */
  forgotPassword(email: string): Observable<any> {
    const request: ForgotPasswordRequest = { email };
    return this.apiService.send<any>(
      'POST',
      'Account/forgot-password',
      request
    ).pipe(
      tap((response) => {
        console.log('Forgot password request sent successfully');
      }),
      catchError((error) => {
        console.error('Forgot password failed:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Reset password with token
   */
  resetPassword(resetPassword: ResetPasswordRequest): Observable<any> {
    return this.apiService.send<any>("POST","account/reset-password",resetPassword);
  }

  /**
   * Validate reset token
   */
  validateResetToken(token: string): Observable<any> {
    if (!token) {
      return throwError(() => new Error('Token is required'));
    }
    
    return this.apiService.send<any>(
      'POST',
      'Account/validate-reset-token',
      { token }
    ).pipe(
      tap((response) => {
        console.log('Token validated successfully');
      }),
      catchError((error) => {
        console.error('Token validation failed:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Change password for authenticated user
   */
  changePassword(currentPassword: string, newPassword: string): Observable<any> {
    if (!currentPassword || !newPassword) {
      return throwError(() => new Error('Current password and new password are required'));
    }
    
    const request: ChangePasswordRequest = {
      currentPassword,
      newPassword
    };
    
    return this.apiService.send<any>(
      'POST',
      'Account/change-password',
      request
    ).pipe(
      tap((response) => {
        console.log('Password changed successfully');
      }),
      catchError((error) => {
        console.error('Change password failed:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Logout user - clear all auth data
   */
  logout(): void {
    localStorage.removeItem('AuthenticatedUserResponse');
    this.authState.next(false);
    this.currentUserSubject.next(null);
    console.log('User logged out successfully');
  }

  /**
   * Update user data in storage and cache
   */
  updateUserData(authResponse: AuthResponse): void {
    if (authResponse?.jwtToken) {
      localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(authResponse));
      this.authState.next(true);
      this.currentUserSubject.next(authResponse);
    }
  }

  /**
   * Check and update auth state from localStorage
   */
  private checkAndUpdateAuthState(): void {
    const user = this.getCurrentUser();
    const isValid = !!user?.jwtToken;
    this.authState.next(isValid);
    if (isValid) {
      this.currentUserSubject.next(user);
    }
  }

  // ==================== USER INFORMATION METHODS ====================

  /**
   * Get user's full name
   */
  getUserFullName(): string {
    const user = this.getCurrentUser();
    if (!user) return '';
    
    const firstName = user.authenticateResponseDto?.firstName || '';
    const lastName = user.authenticateResponseDto?.lastName || '';
    
    if (firstName && lastName) {
      return `${firstName} ${lastName}`;
    }
    return firstName || lastName || '';
  }

  /**
   * Get user's first name
   */
  getFirstName(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.firstName || '';
  }

  /**
   * Get user's last name
   */
  getLastName(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.lastName || '';
  }

  /**
   * Get user's email
   */
  getUserEmail(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.email || '';
  }

  /**
   * Get user's phone number
   */
  getUserPhone(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.phone || '';
  }

  /**
   * Get user's role ID
   */
  getUserRoleId(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.roleId || '';
  }

  /**
   * Get user's role name
   */
  getUserRoleName(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.roleInfo?.name || '';
  }

  /**
   * Get user's role code
   */
  getUserRoleCode(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.roleInfo?.code || '';
  }

  /**
   * Get user's dealer ID
   */
  getDealerId(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.dealerId || '';
  }

  /**
   * Get user's dealership name
   */
  getDealershipName(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.dealerInfo?.dealershipName || '';
  }

  /**
   * Get complete dealer information
   */
  getDealerInfo(): any {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.dealerInfo || null;
  }

  /**
   * Get user's user ID
   */
  getUserId(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.userId || '';
  }

  // ==================== ROLE CHECK METHODS ====================

  /**
   * Check if user has specific role by role ID
   */
  hasRole(roleId: string): boolean {
    const userRole = this.getUserRoleId();
    return userRole === roleId;
  }

  /**
   * Check if user has specific role by role name
   */
  hasRoleByName(roleName: string): boolean {
    const userRole = this.getUserRoleName().toLowerCase();
    return userRole === roleName.toLowerCase();
  }

  /**
   * Check if user is Administrator
   */
  isAdmin(): boolean {
    const roleName = this.getUserRoleName().toLowerCase();
    return roleName === 'administrator' || roleName === 'admin';
  }

  /**
   * Check if user is Operator
   */
  isOperator(): boolean {
    const roleName = this.getUserRoleName().toLowerCase();
    return roleName === 'operator';
  }

  /**
   * Check if user is User/Dealer
   */
  isUser(): boolean {
    const roleName = this.getUserRoleName().toLowerCase();
    return roleName === 'user' || roleName === 'dealer';
  }

  /**
   * Check if user has admin or operator access
   */
  hasAdminOrOperatorAccess(): boolean {
    const roleName = this.getUserRoleName().toLowerCase();
    return roleName === 'administrator' || roleName === 'admin' || roleName === 'operator';
  }

  /**
   * Check if user has dealer access
   */
  hasDealerAccess(): boolean {
    const user = this.getCurrentUser();
    return !!user?.authenticateResponseDto?.dealerInfo;
  }

  /**
   * Check if user can access the portal
   * (Has dealer access OR is admin/operator)
   */
  canAccessPortal(): boolean {
    return this.hasDealerAccess() || this.hasAdminOrOperatorAccess();
  }

  /**
   * Get user's access level
   */
  getAccessLevel(): 'admin' | 'operator' | 'user' | 'none' {
    if (this.isAdmin()) {
      return 'admin';
    } else if (this.isOperator()) {
      return 'operator';
    } else if (this.hasDealerAccess()) {
      return 'user';
    }
    return 'none';
  }

  // ==================== NAVIGATION HELPER METHODS ====================

  /**
   * Get the base path for the current user's role
   */
  getBasePath(): string {
    if (this.isAdmin()) {
      return '/admin';
    } else if (this.isOperator() || this.isUser()) {
      return '/operator';
    }
    return '';
  }

  /**
   * Get the dashboard path for the current user
   */
  getDashboardPath(): string {
    const basePath = this.getBasePath();
    return basePath ? `${basePath}/dashboard` : '/drugs';
  }

  /**
   * Get the redirect path based on user role
   */
  getRedirectPath(): string {
    if (this.isAdmin()) {
      return '/admin/dashboard';
    } else if (this.isOperator() || this.isUser()) {
      return '/operator/dashboard';
    }
    return '/drugs';
  }

  /**
   * Check if current user is authenticated and has access
   */
  isValidUser(): boolean {
    return this.isAuthenticatedSync && this.canAccessPortal();
  }

  // ==================== UTILITY METHODS ====================

  /**
   * Check if user has any of the given roles
   */
  hasAnyRole(roles: string[]): boolean {
    const userRole = this.getUserRoleName().toLowerCase();
    return roles.some(role => userRole === role.toLowerCase());
  }

  /**
   * Check if user has all of the given roles
   */
  hasAllRoles(roles: string[]): boolean {
    const userRole = this.getUserRoleName().toLowerCase();
    return roles.every(role => userRole === role.toLowerCase());
  }

  /**
   * Get user's display name (full name or email)
   */
  getDisplayName(): string {
    const fullName = this.getUserFullName();
    if (fullName) {
      return fullName;
    }
    return this.getUserEmail() || 'User';
  }

  /**
   * Get user's initials
   */
  getUserInitials(): string {
    const firstName = this.getFirstName();
    const lastName = this.getLastName();
    const initials = (firstName?.charAt(0) || '') + (lastName?.charAt(0) || '');
    return initials.toUpperCase() || 'U';
  }

  /**
   * Get user's avatar color based on role
   */
  getAvatarColor(): string {
    if (this.isAdmin()) {
      return 'linear-gradient(135deg, #dc3545, #c82333)';
    } else if (this.isOperator()) {
      return 'linear-gradient(135deg, #ffc107, #e0a800)';
    } else if (this.hasDealerAccess()) {
      return 'linear-gradient(135deg, #0d9488, #0e7490)';
    }
    return 'linear-gradient(135deg, #6c757d, #5a6268)';
  }

  /**
   * Get user's role badge class
   */
  getRoleBadgeClass(): string {
    if (this.isAdmin()) {
      return 'bg-danger text-white';
    } else if (this.isOperator()) {
      return 'bg-warning text-dark';
    } else if (this.hasDealerAccess()) {
      return 'bg-success text-white';
    }
    return 'bg-secondary text-white';
  }

  /**
   * Get user's role display name
   */
  getRoleDisplayName(): string {
    const roleName = this.getUserRoleName();
    if (!roleName) return 'User';
    
    // Capitalize first letter
    return roleName.charAt(0).toUpperCase() + roleName.slice(1).toLowerCase();
  }
}