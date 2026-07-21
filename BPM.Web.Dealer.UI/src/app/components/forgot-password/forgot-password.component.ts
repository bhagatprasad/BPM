import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { SpinnerLoadingService } from '../../common/services/spinner-loading-service';

@Component({
  selector: 'app-forget-password',
  standalone:true,
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgetPasswordComponent implements OnInit {
  @ViewChild('forgetPasswordForm') forgetPasswordForm!: NgForm;
  
  isSubmitting = false;
  email = '';

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private loadingService: SpinnerLoadingService
  ) {}

  ngOnInit(): void {}

  onSubmit(form: NgForm) {
    if (form.invalid) {
      this.toastr.warning('Please fix form errors', 'Warning');
      return;
    }

    this.loadingService.loadingOn();
    this.isSubmitting = true;

    const postData = {
      email: this.email
    };

    this.accountService.forgotPassword(postData).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.toastr.success(res.message || 'Password reset email sent successfully!', 'Success');
          form.resetForm();
          this.email = '';
        } else {
          this.toastr.error(res.message || 'Something went wrong', 'Error');
        }
      },
      error: (err: any) => {
        const errorMessage = err.error?.message || 'Failed to send reset email. Please try again.';
        this.toastr.error(errorMessage, 'Error');
        console.error('Forget password error:', err);
      },
      complete: () => {
        this.loadingService.loadingOff();
        this.isSubmitting = false;
      }
    });
  }
}