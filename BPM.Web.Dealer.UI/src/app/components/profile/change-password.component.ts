import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-change-password',
  imports: [FormsModule, CommonModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css',

})
export class ChangePasswordComponent {

  @Input() changePswd: any = {};
  @Input() isChangePassword: boolean = false;
  @Input() showPassword: boolean = false;
  @Input() showConfirmPassword: boolean = false;
  @Input() errorMsz: string = '';

  @Output() Edit = new EventEmitter<void>();
  @Output() togglePassword = new EventEmitter<void>();
  @Output() toggleConfirmPassword = new EventEmitter<void>();
  @Output() save = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();



  constructor() {
    console.log('ChangePasswordComponent intialized with changePswd:', this.changePswd)
    console.log('ChangePasswordComponent intialized with isChangePassword:', this.isChangePassword)
    console.log('ChangePasswordComponent intialized with showPassword:', this.showPassword)
    console.log('ChangePasswordComponent intialized with showConfirmPassword:', this.showConfirmPassword)
    console.log('ChangePasswordComponent intialized with errorMsz:', this.errorMsz)
    console.log('ChangePasswordComponent intialized with Edit:', this.Edit)
    console.log('ChangePasswordComponent intialized with togglePassword:', this.togglePassword)
    console.log('ChangePasswordComponent intialized with toggleConfirmPassword:', this.toggleConfirmPassword)
    console.log('ChangePasswordComponent intialized with save:', this.save)
    console.log('ChangePasswordComponent intialized with cancel:', this.cancel)
  }
  ngOnChanges(changes: SimpleChanges) {
    if (changes['changePswd']) {
      console.log('changePwd changed to', changes['changePswd'].currentValue);
      this.changePswd = changes['changePswd'].currentValue;
    }
    if (changes['isChangePassword']) {
      console.log('isChangePassword changed to', changes['isChangePassword'].currentValue);
      this.isChangePassword = changes['isChangePassword'].currentValue;
    }
    if (changes['showPassword']) {
      console.log('showPassword changed to', changes['showPassword'].currentValue);
      this.showPassword = changes['showPassword'].currentValue;
    }
    if (changes['showConfirmPassword']) {
      console.log('showConfirmPassword changed to', changes['showConfirmPassword'].currentValue);
      this.showConfirmPassword = changes['showConfirmPassword'].currentValue;
    }
    if (changes['errorMsz']) {
      console.log('errorMsz changed to', changes['errorMsz'].currentValue);
      this.errorMsz = changes['errorMsz'].currentValue;
    }
    if (changes['Edit']) {
      console.log('Edit changed to', changes['Edit'].currentValue);
      this.Edit = changes['Edit'].currentValue;
    }
    if (changes['togglePassword']) {
      console.log('togglePassword changed to', changes['togglePassword'].currentValue);
      this.togglePassword = changes['togglePassword'].currentValue;
    }
    if (changes['toggleConfirmPassword']) {
      console.log('toggleConfirmPassword changed to', changes['toggleConfirmPassword'].currentValue);
      this.toggleConfirmPassword = changes['toggleConfirmPassword'].currentValue;
    }
    if (changes['save']) {
      console.log('save changed to', changes['save'].currentValue);
      this.save = changes['save'].currentValue;
    }
    if (changes['cancel']) {
      console.log('cancel changed to', changes['cancel'].currentValue);
      this.cancel = changes['cancel'].currentValue;
    }
  }
  // Add this method to your ChangePasswordComponent class
  getPasswordStrength(password: string): string {
    if (!password || password.length < 6) return 'weak';

    let strength = 0;

    // Check for uppercase letters
    if (/[A-Z]/.test(password)) strength++;
    // Check for lowercase letters
    if (/[a-z]/.test(password)) strength++;
    // Check for numbers
    if (/[0-9]/.test(password)) strength++;
    // Check for special characters
    if (/[^A-Za-z0-9]/.test(password)) strength++;
    // Check length
    if (password.length >= 12) strength++;

    if (strength <= 2) return 'weak';
    if (strength <= 4) return 'medium';
    return 'strong';
  }

  getPasswordStrengthWidth(password: string): number {
    const strength = this.getPasswordStrength(password);
    if (strength === 'weak') return 33;
    if (strength === 'medium') return 66;
    return 100;
  }
}
