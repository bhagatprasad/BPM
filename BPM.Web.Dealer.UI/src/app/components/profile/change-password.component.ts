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

  @Output() edit = new EventEmitter<void>();
  @Output() togglePassword = new EventEmitter<void>();
  @Output() toggleConfirmPassword = new EventEmitter<void>();
  @Output() save = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();



  constructor() {}
  ngOnChanges(changes: SimpleChanges) {
    if (changes['changePswd']) {
      console.log('changePwd changed to', changes['changePswd'].currentValue);      
    }
    if (changes['isChangePassword']) {
      console.log('isChangePassword changed to', changes['isChangePassword'].currentValue);
      this.isChangePassword = changes['isChangePassword'].currentValue;
    }
    if (changes['showPassword']) {
      console.log('showPassword changed to', changes['showPassword'].currentValue);    
    }
    if (changes['showConfirmPassword']) {
      console.log('showConfirmPassword changed to', changes['showConfirmPassword'].currentValue);     
    }
    if (changes['errorMsz']) {
      console.log('errorMsz changed to', changes['errorMsz'].currentValue);     
    }
    if (changes['Edit']) {
      console.log('Edit changed to', changes['Edit'].currentValue);      
    }
    if (changes['togglePassword']) {
      console.log('togglePassword changed to', changes['togglePassword'].currentValue);     
    }
    if (changes['toggleConfirmPassword']) {
      console.log('toggleConfirmPassword changed to', changes['toggleConfirmPassword'].currentValue);      
    }
    if (changes['save']) {
      console.log('save changed to', changes['save'].currentValue);     
    }
    if (changes['cancel']) {
      console.log('cancel changed to', changes['cancel'].currentValue);      
    }
  } 
  getPasswordStrength(password: string): string {
    if (!password || password.length < 6) return 'weak';

    let strength = 0;

  
    if (/[A-Z]/.test(password)) strength++;    
    if (/[a-z]/.test(password)) strength++;   
    if (/[0-9]/.test(password)) strength++;    
    if (/[^A-Za-z0-9]/.test(password)) strength++;    
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
  onEdit():void{
    this.edit.emit();
  }
  onSave():any{
    this.save.emit(this.changePswd);
  }
  onCancel():void{
    this.cancel.emit();
  }

  onTogglePassword():void{
    this.togglePassword.emit();
  }
  onToggleConfirmPassword():void{
    this.toggleConfirmPassword.emit();
  }
  
}
