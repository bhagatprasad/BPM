import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AccountService } from '../../services/account.service';
import { CommonModule } from '@angular/common';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { Subscription } from 'rxjs';
import { SpinnerLoadingService } from '../../common/services/spinner-loading-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit, OnDestroy {
  loginObj = {
    username: '',
    password: ''
  };

  rememberMe = false;
  isLoading = false;
  errorMessage = '';
  showPassword = false;
  emailError: string = '';
  passwordError: string = '';
  private loginSubscription?: Subscription;

  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private spinnerService: SpinnerLoadingService
  ) { 
    console.log('LoginComponent constructor');
  }

  ngOnInit(): void {
    console.log('LoginComponent ngOnInit');
    
    // Check if user is already authenticated - if so, redirect
    const loggedData = localStorage.getItem('AuthenticatedUserResponse');
    console.log('Logged data:', loggedData);
    
    if (loggedData) {
      try {
        const authResponse = JSON.parse(loggedData);
        if (authResponse?.jwtToken) {
          console.log('User already logged in, redirecting to drugs-catalog');
          this.router.navigateByUrl('/drugs-catalog');
          return;
        }
      } catch (e) {
        console.error('Error parsing auth data:', e);
      }
    }

    // Load saved username if exists
    const savedUsername = localStorage.getItem('savedUsername');
    if (savedUsername) {
      this.loginObj.username = savedUsername;
      this.rememberMe = true;
    }
  }

  ngOnDestroy(): void {
    if (this.loginSubscription) {
      this.loginSubscription.unsubscribe();
    }
    this.spinnerService.hide();
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onForgotPassword(): void {
    console.log('Navigating to forgot password');
    this.router.navigate(['/forgot-password']).catch(err => {
      console.error('Navigation error:', err);
      window.location.href = '/forgot-password';
    });
  }

  // Validate email format
  validateEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  // Check form validity
  checkFormValidity(): boolean {
    const email = this.loginObj.username?.trim() || '';
    const password = this.loginObj.password?.trim() || '';
    return this.validateEmail(email) && password.length >= 6;
  }

  // Clear all errors
  clearErrors(): void {
    this.emailError = '';
    this.passwordError = '';
    this.errorMessage = '';
  }

  // Show error message
  showError(field: string, message: string): void {
    if (field === 'loginEmailError' || field === 'emailError') {
      this.emailError = message;
    } else if (field === 'loginPasswordError' || field === 'passwordError') {
      this.passwordError = message;
    } else {
      this.errorMessage = message;
    }
    
    setTimeout(() => {
      if (field === 'loginEmailError' || field === 'emailError') {
        this.emailError = '';
      } else if (field === 'loginPasswordError' || field === 'passwordError') {
        this.passwordError = '';
      } else {
        this.errorMessage = '';
      }
    }, 5000);
  }

  // Check if user has access to dealer portal
  hasDealerAccess(appUser: any): boolean {
    // Check if dealerInfo exists (has dealer access)
    const hasDealer = appUser?.authenticateResponseDto?.dealerInfo !== null && 
                      appUser?.authenticateResponseDto?.dealerInfo !== undefined;
    
    // Check if user is Administrator or Operator
    const roleName = appUser?.authenticateResponseDto?.roleInfo?.name;
    const isAdminOrOperator = roleName === "Administrator" || roleName === "Operator";
    
    // User has access if:
    // 1. They have dealerInfo (dealer user), OR
    // 2. They are Administrator or Operator (can login without dealer)
    return hasDealer || isAdminOrOperator;
  }

  // Handle authentication success
  handleAuthenticationSuccess(response: any): void {
    console.info('Authentication response:', response);

    // Check if response has appUser
    if (response) {
      const appUser = response;

      // Check if user has access to this portal
      if (!this.hasDealerAccess(appUser)) {
        const errorMsg = 'You are not authorized to login to this portal. Please use the dealer portal to login.';
        this.showError('loginEmailError', errorMsg);
        this.toastr.error(errorMsg, 'Access Denied');
        this.spinnerService.hide();
        this.isLoading = false;
        return;
      }

      // Check if user is authenticated successfully
      if (appUser.jwtToken) {
        // Store user info
        if (this.rememberMe) {
          localStorage.setItem('savedUsername', this.loginObj.username);
        } else {
          localStorage.removeItem('savedUsername');
        }

        // Store auth data
        localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(response));
        localStorage.setItem('ApplicationUser', JSON.stringify(appUser));
        
        // Also set in ApiService for consistency
        this.accountService.apiService.setAuthData(response);

        // Get role name
        const roleName = appUser.authenticateResponseDto?.roleInfo?.name;

        // Hide loader
        this.spinnerService.hide();
        this.isLoading = false;

        // Show success message
        this.toastr.success('Login successful!', 'Success');

        // Redirect based on role
        setTimeout(() => {
          if (roleName === "Administrator" || roleName === "Operator") {
            // Admin or Operator - redirect to Admin dashboard
            this.router.navigateByUrl('/AdminBoard/Index');
          } else {
            // Regular user with dealer - redirect to User dashboard
            this.router.navigateByUrl('/UserBoard/Index');
          }
        }, 500);
      } else {
        // JWT token is missing - show error message
        const errorMessage = appUser.message || 'Authentication failed. Please try again.';
        this.showError('loginEmailError', errorMessage);
        this.spinnerService.hide();
        this.isLoading = false;
      }
    } else {
      // Invalid response
      this.showError('loginEmailError', 'Invalid response from server. Please try again.');
      this.spinnerService.hide();
      this.isLoading = false;
    }
  }

  // Handle authentication error
  handleAuthenticationError(error: any): void {
    console.error('Authentication error:', error);

    // Hide loader
    this.spinnerService.hide();
    this.isLoading = false;

    let errorMsg = 'Invalid email or password. Please try again.';
    
    if (error instanceof HttpErrorResponse) {
      if (error.status === 0) {
        errorMsg = 'Unable to connect to the server. Please check your network connection.';
      } else if (error.status === 401) {
        errorMsg = 'Invalid email or password. Please try again.';
      } else if (error.status === 403) {
        errorMsg = 'Access forbidden. Please contact support.';
      } else if (error.status === 404) {
        errorMsg = 'Login service not found. Please try again later.';
      } else if (error.status === 500) {
        errorMsg = 'Internal server error. Please try again later.';
      } else if (error.error?.message) {
        errorMsg = error.error.message;
      } else if (error.error?.error) {
        errorMsg = error.error.error;
      }
    } else if (error.message) {
      errorMsg = error.message;
    }

    this.showError('loginEmailError', errorMsg);
    this.toastr.error(errorMsg, 'Error');
  }

  // Handle Sign In
  onLogin(): void {
    // Clear previous errors
    this.clearErrors();

    const email = this.loginObj.username?.trim() || '';
    const password = this.loginObj.password?.trim() || '';

    // Validate fields
    if (!this.validateEmail(email)) {
      this.showError('loginEmailError', 'Please enter a valid email address.');
      return;
    }

    if (!password || password.length < 6) {
      this.showError('loginPasswordError', 'Password must be at least 6 characters.');
      return;
    }

    if (this.isLoading) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.spinnerService.show('Signing you in...');

    const userAuthentication = {
      username: email,
      password: password,
      rememberMe: this.rememberMe
    };

    if (this.loginSubscription) {
      this.loginSubscription.unsubscribe();
    }

    this.loginSubscription = this.accountService.authenticateAsync(userAuthentication).subscribe({
      next: (res: any) => {
        console.log('Login response:', res);
        this.handleAuthenticationSuccess(res);
      },
      error: (err: HttpErrorResponse) => {
        this.handleAuthenticationError(err);
      }
    });
  }

  // Update environment and version
  updateEnvironmentAndVersion(): void {
    localStorage.setItem('Environment', window.location.hostname);
    localStorage.setItem('Version', '1.0.0.0');
  }
}