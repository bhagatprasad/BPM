import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { SpinnerLoadingService } from '../../common/services/spinner-loading-service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit, OnDestroy {
  email = '';
  isSubmitting = false;
  emailSent = false;
  errorMessage = '';
  private forgotPasswordSubscription?: Subscription;

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private loadingService: SpinnerLoadingService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Check if user is already logged in
    const loggedData = localStorage.getItem('AuthenticatedUserResponse');
    if (loggedData) {
      try {
        const authResponse = JSON.parse(loggedData);
        if (authResponse?.jwtToken) {
          this.router.navigateByUrl('/drugs-catalog');
          return;
        }
      } catch (e) {
        console.error('Error parsing auth data:', e);
      }
    }
  }

  ngOnDestroy(): void {
    if (this.forgotPasswordSubscription) {
      this.forgotPasswordSubscription.unsubscribe();
    }
    this.loadingService.hide();
  }

  onSubmit(form: NgForm) {
    if (form.invalid) {
      this.toastr.warning('Please enter a valid email address', 'Warning');
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.loadingService.show('Sending reset link...');

    if (this.forgotPasswordSubscription) {
      this.forgotPasswordSubscription.unsubscribe();
    }

    console.log('🔵 Sending forgot password request for:', this.email);

    this.forgotPasswordSubscription = this.accountService.forgotPassword(this.email).subscribe({
      next: (res: any) => {
        this.isSubmitting = false;
        this.loadingService.hide();

        console.log('✅ Forgot password response:', res);

        if (res.success) {
          this.emailSent = true;
          this.toastr.success(res.message || 'Password reset email sent successfully!', 'Success');

          // Navigate with userId from response
          if (res.userId) {
            console.log('📧 Navigating to reset-password with userId:', res.userId);
            this.router.navigate(['/reset-password'], {
              queryParams: { userId: res.userId }
            });
          } else {
            console.warn('⚠️ No userId received in response');
            // If no userId, navigate with email as fallback
            this.router.navigate(['/reset-password'], {
              queryParams: { userId: this.email }
            });
          }

          form.resetForm();
          this.email = '';
        } else {
          this.errorMessage = res.message || 'Something went wrong. Please try again.';
          this.toastr.error(this.errorMessage, 'Error');
        }
      },
      error: (error: any) => {
        this.isSubmitting = false;
        this.loadingService.hide();

        console.error('❌ Forgot password error:', error);
        
        let errorMsg = 'Failed to send reset email. Please try again.';
        if (error.status === 0) {
          errorMsg = 'Unable to connect to the server. Please check your network connection.';
        } else if (error.status === 404) {
          errorMsg = 'Email address not found. Please check and try again.';
        } else if (error.status === 400) {
          errorMsg = 'Invalid email address. Please check and try again.';
        } else if (error.error?.message) {
          errorMsg = error.error.message;
        }

        this.errorMessage = errorMsg;
        this.toastr.error(errorMsg, 'Error');
      }
    });
  }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }

  resendEmail(): void {
    if (this.email) {
      // Create a form object to resend
      const form = {
        valid: true,
        invalid: false,
        resetForm: () => { },
        value: { email: this.email }
      } as any;
      this.onSubmit(form);
    } else {
      this.toastr.warning('Please enter your email address to resend.', 'Warning');
      this.emailSent = false;
    }
  }
}