import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { SpinnerLoadingService } from '../../common/services/spinner-loading-service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,        
    RouterModule,
  ],
  templateUrl: './reset-passwor.component.html',
  styleUrls: ['./reset-passwor.component.css']
})
export class ResetPasswordComponent implements OnInit {
  // Form data
  password: string = '';
  confirmPassword: string = '';
  
  token: string = '';
  isValidToken: boolean = false;
  isSubmitting: boolean = false;
  
  // For password visibility toggle
  showPassword: boolean = false;
  showConfirmPassword: boolean = false;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _accountService: AccountService,
    private _toastr: ToastrService,
    private _spinner: SpinnerLoadingService
  ) {}

  ngOnInit(): void {
    this.token = this._route.snapshot.params['token'];
    this.validateToken();
  }

  validateToken() {
    this._spinner.loadingOn();
    this._accountService.validateResetToken(this.token).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.isValidToken = true;
        } else {
          this._toastr.error(res.message || 'Invalid token', 'Error');
          setTimeout(() => this._router.navigate(['/forgot-password']), 3000);
        }
      },
      error: (err: any) => {
        this._toastr.error(err.error?.message || 'Invalid or expired reset link', 'Error');
        setTimeout(() => this._router.navigate(['/forgot-password']), 3000);
      },
      complete: () => this._spinner.loadingOff()
    });
  }

  onSubmit(form: NgForm) {
    // Check if passwords match manually
    if (this.password !== this.confirmPassword) {
      this._toastr.warning('Passwords do not match', 'Warning');
      return;
    }

    if (form.invalid) {
      this._toastr.warning('Please fix form errors', 'Warning');
      return;
    }

    this.isSubmitting = true;
    this._spinner.loadingOn();

    this._accountService.resetPassword(this.token, {
      password: this.password
    }).subscribe({
      next: (res: any) => {
        if (res.success) {
          this._toastr.success('Password reset successful! Please login.', 'Success');
          setTimeout(() => this._router.navigate(['/login']), 2000);
        } else {
          this._toastr.error(res.message || 'Failed to reset password', 'Error');
        }
      },
      error: (err: any) => {
        this._toastr.error(err.error?.message || 'Failed to reset password', 'Error');
      },
      complete: () => {
        this.isSubmitting = false;
        this._spinner.loadingOff();
      }
    });
  }

  // Password visibility toggle methods
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }
}