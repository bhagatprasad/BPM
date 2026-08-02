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
   * Get user's dealership name
   */
  getDealershipName(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.dealerInfo?.dealershipName || '';
  }

  /**
   * Get user's email
   */
  getUserEmail(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.email || '';
  }

  /**
   * Get user's role
   */
  getUserRole(): string {
    const user = this.getCurrentUser();
    return user?.authenticateResponseDto?.roleId || '';
  }

  /**
   * Check if user has specific role
   */
  hasRole(roleId: string): boolean {
    const userRole = this.getUserRole();
    return userRole === roleId;
  }
}