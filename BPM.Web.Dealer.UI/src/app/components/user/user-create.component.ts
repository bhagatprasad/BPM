// user-create.component.ts
import { CommonModule } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RoleService } from '@app/services/role.service';
import { roleInfo, userInformation, UserDistributorUpdateDto } from '@app/models/user';
import { UserUpdateDto } from '@app/models/user-update-dto';
import { DistributorInfo } from '@app/models/distributor';
import { DistributorService } from '@app/services/distributor.service';

@Component({
  selector: 'app-user-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.css'],
})
export class UserCreateSidebarComponent implements OnInit {
  @Input() isVisible: boolean = false;
  @Input() dealerId: string = '';
  @Input() editUserData: userInformation | null = null;
  @Output() closeSidebar = new EventEmitter<void>();
  @Output() formSubmit = new EventEmitter<any>();
  @Output() userUpdate = new EventEmitter<UserUpdateDto>();
  @Output() userDistributorUpdate = new EventEmitter<UserDistributorUpdateDto>();

  roles: roleInfo[] = [];
  filteredRoles: roleInfo[] = [];
  distributors: DistributorInfo[] = [];
  showPassword: boolean = false;
  isEditMode: boolean = false;
  userId: string = '';

  userForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private roleService: RoleService,
    private distributorService: DistributorService,
    private cdr: ChangeDetectorRef,
  ) {
    this.userForm = this.fb.group({
      id: [''],
      firstName: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
          Validators.pattern('^[A-Za-z ]+$'),
        ],
      ],
      lastName: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(50),
          Validators.pattern('^[A-Za-z ]+$'),
        ],
      ],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]{10,15}$')]],
      password: [
        '',
        [Validators.minLength(6), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,}$')],
      ],
      isActive: [true],
      dealerId: [''],
      distributorId: ['', Validators.required],
      roleId: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadRoles();
    this.loadDistributors();
  }

  loadRoles(): void {
    this.roleService.getAllRolesAsync().subscribe({
      next: (roles) => {
        this.roles = roles || [];
        // Filter to show only Operator and Dealer (exclude Administrator)
        this.filteredRoles = this.roles.filter((role) => role.code === 'OPERATOR');
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading roles:', error);
      },
    });
  }

  loadDistributors(): void {
    this.distributorService.getAllDistributors().subscribe({
      next: (distributors) => {
        this.distributors = distributors;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading Distributors: ', error);
      },
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['dealerId'] && this.dealerId) {
      this.userForm.patchValue({
        dealerId: this.dealerId,
      });
    }

    if (changes['editUserData'] && this.editUserData) {
      this.loadUserForEdit(this.editUserData);
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  close(): void {
    this.closeSidebar.emit();
    this.resetForm();
    this.isEditMode = false;
  }

  onSubmit(): void {
    if (this.userForm.invalid) {
      Object.keys(this.userForm.controls).forEach((key) => {
        const control = this.userForm.get(key);
        if (control?.invalid) {
          control.markAsTouched();
        }
      });
      return;
    }

    const formData = this.userForm.value;

    if (this.isEditMode) {
      // Prepare UpdateUserDto for your API
      const updateDto: UserUpdateDto = {
        userId: formData.id,
        firstName: formData.firstName,
        lastName: formData.lastName,
        email: formData.email,
        phone: formData.phone,
        isActive: formData.isActive,
        modifiedBy: '', // Will be set in parent component
      };

      this.userUpdate.emit(updateDto);

      const distributorUpdateDto: UserDistributorUpdateDto = {
        userId: formData.id,
        distributorId: formData.distributorId || undefined,
        modifiedBy: '',
      };

      this.userDistributorUpdate.emit(distributorUpdateDto);
    } else {
      this.formSubmit.emit(formData);
    }
    this.close();
  }

  resetForm(): void {
    this.userForm.reset({
      isActive: true,
      dealerId: this.dealerId,
      distributorId: '',
    });
    this.isEditMode = false;
    this.userId = '';
    // Reset password validators to required for create mode
    this.userForm
      .get('password')
      ?.setValidators([
        Validators.required,
        Validators.minLength(6),
        Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,}$'),
      ]);
    this.userForm.get('password')?.updateValueAndValidity();

    Object.keys(this.userForm.controls).forEach((key) => {
      this.userForm.get(key)?.markAsPristine();
      this.userForm.get(key)?.markAsUntouched();
    });
  }

  reset(): void {
    this.resetForm();
  }

  openEditMode(user: userInformation): void {
    this.isEditMode = true;
    this.editUserData = user;
    // Make password optional for edit mode
    this.userForm.get('password')?.clearValidators();
    this.userForm
      .get('password')
      ?.setValidators([
        Validators.minLength(6),
        Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,}$'),
      ]);
    this.userForm.get('password')?.updateValueAndValidity();
    this.loadUserForEdit(user);
  }

  private loadUserForEdit(user: userInformation): void {
    this.userId = user.userId || '';
    this.userForm.patchValue({
      id: user.userId || '',
      firstName: user.firstName || '',
      lastName: user.lastName || '',
      email: user.email || '',
      phone: user.phone || '',
      isActive: user.isActive ?? true,
      dealerId: user.dealerId || this.dealerId,
      distributorId: user.distributorId || '',
      roleId: user.roleId || '',
    });
    this.userForm.get('password')?.setValue('');
    this.cdr.detectChanges();
  }

  get formTitle(): string {
    return this.isEditMode ? 'Edit User' : 'Add New User';
  }

  get submitButtonText(): string {
    return this.isEditMode ? 'Update User' : 'Create User';
  }
}
