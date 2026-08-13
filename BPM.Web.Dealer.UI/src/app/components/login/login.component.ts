import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
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
    
    // Check if user is already authenticated
    this.accountService.isAuthenticated().then(isAuth => {
      if (isAuth) {
        const currentUser = this.accountService.getCurrentUser();
        if (currentUser) {
          this.redirectBasedOnRole(currentUser);
        }
      }
    });

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

  /**
   * Redirect user based on their role
   */
  private redirectBasedOnRole(authResponse: any): void {
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

  validateEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  clearErrors(): void {
    this.emailError = '';
    this.passwordError = '';
    this.errorMessage = '';
  }

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

  onLogin(): void {
    this.clearErrors();

    const email = this.loginObj.username?.trim() || '';
    const password = this.loginObj.password?.trim() || '';

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
        this.isLoading = false;
        this.spinnerService.hide();
        console.log('Login response:', res);

        if (res?.jwtToken) {
          if (this.rememberMe) {
            localStorage.setItem('savedUsername', this.loginObj.username);
          } else {
            localStorage.removeItem('savedUsername');
          }

          this.toastr.success('Login successful!', 'Success');

          // Redirect based on role after a short delay
          setTimeout(() => {
            this.redirectBasedOnRole(res);
          }, 500);
        } else {
          this.errorMessage = 'Invalid username or password. Please try again.';
          this.toastr.error('Invalid username or password. Please try again.', 'Error');
        }
      },
      error: (err: any) => {
        this.isLoading = false;
        this.spinnerService.hide();
        console.error('Login failed', err);

        let errorMsg = 'Login failed. Please try again.';
        if (err.status === 0) {
          errorMsg = 'Unable to connect to the server. Please check your network connection.';
        } else if (err.status === 401) {
          errorMsg = 'Invalid username or password. Please try again.';
        } else if (err.status === 403) {
          errorMsg = 'Access forbidden. Please contact support.';
        } else if (err.error?.message) {
          errorMsg = err.error.message;
        }

        this.errorMessage = errorMsg;
        this.toastr.error(errorMsg, 'Error');
      }
    });
  }
}