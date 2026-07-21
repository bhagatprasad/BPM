import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AccountService } from '../../services/account.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { SpinnerLoadingService } from '../../common/services/spinner-loading-service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit, OnDestroy {
  resetData = {
    userId: '',
    newPassword: '',
    confirmPassword: ''
  };

  showPassword = false;
  showConfirmPassword = false;
  isSubmitting = false;
  passwordReset = false;
  errorMessage = '';
  isValidLink = false;
  private resetSubscription?: Subscription;
  private routeSubscription?: Subscription;

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private loadingService: SpinnerLoadingService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    console.log('🔵 ResetPasswordComponent constructor');
  }

  ngOnInit(): void {
    console.log('🔵 ResetPasswordComponent ngOnInit');

    // Get userId from query parameters
    this.routeSubscription = this.route.queryParams.subscribe(params => {
      console.log('📧 All query params:', params);

      // FIX: Check for both 'userId' and 'userid' (case insensitive)
      const userId = params['userId'] || params['userid'];

      console.log('📧 UserId from query params:', userId);

      if (userId && userId !== 'undefined' && userId !== 'null' && userId.trim() !== '') {
        this.resetData.userId = userId;
        this.isValidLink = true;
        this.toastr.info('Please enter your new password', 'Reset Password');
        console.log('✅ Valid reset link with userId:', userId);
      } else {
        this.isValidLink = false;
        this.errorMessage = 'Invalid reset link. User ID not found.';
        this.toastr.error('Invalid reset link. Please request a new one.', 'Error');
        console.error('❌ No userId found in query params');
      }
    });
  }

  ngOnDestroy(): void {
    if (this.resetSubscription) {
      this.resetSubscription.unsubscribe();
    }
    if (this.routeSubscription) {
      this.routeSubscription.unsubscribe();
    }
    this.loadingService.loadingOff();
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  onSubmit(form: NgForm) {
    if (form.invalid) {
      this.toastr.warning('Please fix form errors', 'Warning');
      return;
    }

    if (this.resetData.newPassword !== this.resetData.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      this.toastr.warning('Passwords do not match', 'Warning');
      return;
    }

    if (this.resetData.newPassword.length < 6) {
      this.errorMessage = 'Password must be at least 6 characters long';
      this.toastr.warning('Password must be at least 6 characters long', 'Warning');
      return;
    }

    if (!this.resetData.userId || this.resetData.userId === 'undefined' || this.resetData.userId === 'null') {
      this.errorMessage = 'User ID is missing. Please request a new reset link.';
      this.toastr.error('User ID is missing', 'Error');
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.loadingService.loadingOn();

    if (this.resetSubscription) {
      this.resetSubscription.unsubscribe();
    }

    const postData = {
      userId: this.resetData.userId,
      newPassword: this.resetData.newPassword
    };

    console.log('🔵 Submitting reset password for userId:', this.resetData.userId);
    console.log('🔵 Post data:', postData);

    this.resetSubscription = this.accountService.resetPassword(postData).subscribe({
      next: (res: any) => {
        this.isSubmitting = false;
        this.loadingService.loadingOff();
        this.toastr.success(res.message || 'Password reset successfully!', 'Success');
        console.log('✅ Reset password response:', res);
        this.router.navigate(['/login']);
      },
      error: (err: HttpErrorResponse) => {
        this.isSubmitting = false;
        this.loadingService.loadingOff();

        console.error('❌ Reset password error:', err);

        let errorMsg = 'Failed to reset password. Please try again.';
        if (err.status === 0) {
          errorMsg = 'Unable to connect to the server. Please check your network connection.';
        } else if (err.status === 400) {
          errorMsg = 'Invalid request. Please check your input and try again.';
        } else if (err.status === 404) {
          errorMsg = 'User not found. Please request a new reset link.';
        } else if (err.status === 401) {
          errorMsg = 'Invalid or expired reset link. Please request a new one.';
        }

        this.errorMessage = errorMsg;
        this.toastr.error(errorMsg, 'Error');
      }
    });
  }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }

  navigateToForgotPassword(): void {
    this.router.navigate(['/forgot-password']);
  }

  // Helper method to check if form is valid
  isFormValid(): boolean {
    return this.isValidLink &&
      this.resetData.newPassword.length >= 6 &&
      this.resetData.newPassword === this.resetData.confirmPassword;
  }
}