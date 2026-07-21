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
    password: '',
    JwtToken: ''
  };

  rememberMe = false;
  isLoading = false;
  errorMessage = '';
  showPassword = false;
  private loginSubscription?: Subscription;

  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private spinnerService: SpinnerLoadingService
     
  ) { }

  ngOnInit(): void {
    // Check if user is already authenticated
    this.toastr.info('Checking authentication status...', 'Info');
    const loggedData = localStorage.getItem('AuthenticatedUserResponse');
    if (loggedData) {
      try {
        const authResponse = JSON.parse(loggedData);
        if (authResponse?.jwtToken) {
          this.router.navigateByUrl('/drugs-catalog');
          return;
        }
      } catch (e) {
        // Invalid data, continue to login
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
    // Clean up subscription to prevent memory leaks
    if (this.loginSubscription) {
      this.loginSubscription.unsubscribe();
    }
    // Hide spinner if component is destroyed while loading
    this.spinnerService.loadingOff(); // 👈 Ensure spinner is hidden
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onLogin(): void {
    // Prevent multiple submissions
    if (this.isLoading) {
      return;
    }

    if (!this.loginObj.username || !this.loginObj.password) {
      this.errorMessage = 'Please enter both username and password.';
      this.toastr.error('Please enter both username and password.', 'Error');
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.spinnerService.loadingOn(); // 👈 Show spinner when login starts

    // Unsubscribe from previous subscription if exists
    if (this.loginSubscription) {
      this.loginSubscription.unsubscribe();
    }

    this.loginSubscription = this.accountService.authenticateAsync(this.loginObj).subscribe({
      next: (res: any) => {
        this.isLoading = false;
        this.spinnerService.loadingOff(); // 👈 Hide spinner on success
        console.log('Login response:', res);

        if (res.jwtToken) {
          // Save username if remember me is checked
          if (this.rememberMe) {
            localStorage.setItem('savedUsername', this.loginObj.username);
          } else {
            localStorage.removeItem('savedUsername');
          }

          localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(res));
          this.toastr.success('Login successful!', 'Success');
          
          // Use router navigation instead of window.location
          setTimeout(() => {
            this.router.navigateByUrl('/drugs-catalog');
          }, 500);
        } else {
          this.errorMessage = 'Invalid username or password. Please try again.';
          this.toastr.error('Invalid username or password. Please try again.', 'Error');
        }
      },
      error: (err: HttpErrorResponse) => {
        console.error('Login failed', err);
        this.isLoading = false;
        this.spinnerService.loadingOff(); // 👈 Hide spinner on error

        let errorMsg = 'Login failed. Please try again.';
        if (err.status === 0) {
          errorMsg = 'Unable to connect to the server. Please check your network connection.';
        } else if (err.status === 401) {
          errorMsg = 'Invalid username or password. Please try again.';
        } else if (err.status === 403) {
          errorMsg = 'Access forbidden. Please contact support.';
        } else if (err.status === 404) {
          errorMsg = 'Login service not found. Please try again later.';
        } else if (err.status === 500) {
          errorMsg = 'Internal server error. Please try again later.';
        }

        this.errorMessage = errorMsg;
        this.toastr.error(errorMsg, 'Error');
      }
    });
  }
}