import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.css']
})
export class UserCreateSidebarComponent {
  @Input() isVisible: boolean = false;
  @Input() dealerId: string = '';
  @Output() closeSidebar = new EventEmitter<void>();
  @Output() formSubmit = new EventEmitter<any>();

  userForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]{10,15}$')]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      isActive: [true],
      dealerId: [''],
      roleId: ['', Validators.required]
    });
  }

  ngOnChanges() {
    // Update dealerId if it changes from parent
    if (this.dealerId) {
      this.userForm.patchValue({
        dealerId: this.dealerId
      });
    }
  }

  close(): void {
    this.closeSidebar.emit();
    this.resetForm();
  }

  onSubmit(): void {
    if (this.userForm.invalid) {
      Object.keys(this.userForm.controls).forEach(key => {
        const control = this.userForm.get(key);
        if (control?.invalid) {
          control.markAsTouched();
        }
      });
      return;
    }
    
    // Emit the form data to parent
    this.formSubmit.emit(this.userForm.value);
  }

  resetForm(): void {
    this.userForm.reset({
      isActive: true,
      dealerId: this.dealerId
    });
    Object.keys(this.userForm.controls).forEach(key => {
      this.userForm.get(key)?.markAsPristine();
      this.userForm.get(key)?.markAsUntouched();
    });
  }

  // Helper method to reset form from parent
  reset(): void {
    this.resetForm();
  }
}