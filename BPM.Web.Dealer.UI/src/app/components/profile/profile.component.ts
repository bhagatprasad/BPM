import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UpdateUserRequest, UpdateUserResponse } from '../../models/user-profile';
import { UserService } from '../../services/profile.service';
import { ResetPasswordComponent } from '../reset-password/reset-password.component';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { SpinnerLoadingService } from '../../common/services/spinner-loading-service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [FormsModule, CommonModule, ResetPasswordComponent],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  userData: any = null;
  isAdmin: boolean = false;

  activeTab: string = 'personal';

  userSection = {
    firstName: '',
    lastName: '',
    email: '',
    phone: ''
  };

  dealerSection = {
    dealershipName: '',
    contactPerson: '',
    email: '',
    phone: '',
    alternatePhone: '',
    addressLine1: '',
    addressLine2: '',
    city: '',
    state: '',
    country: '',
    postalCode: '',
    gstNumber: '',
    registrationNumber: '',
    tradeLicenseNumber: '',
    website: ''
  };

  isEditing: boolean = false;
  originalUserData: any = {};

  errorMessage: string = '';
  successMessage: string = '';

  userId: string = '';

  constructor(
    private userService: UserService,
    private toastr: ToastrService,
    private loader: SpinnerLoadingService,
    private cdr: ChangeDetectorRef,
  ) { }

  ngOnInit(): void {
    this.loadUserData();
  }

  // ============ DATA LOADING METHODS ============

  loadUserData(): void {
    const storedData = localStorage.getItem('AuthenticatedUserResponse');
    if (storedData) {
      this.userData = JSON.parse(storedData);
      console.log('Full userData:', this.userData);
      this.userId = this.userData.authenticateResponseDto.userId;
      this.populateFormData();
    }
  }

  populateFormData(): void {
    if (this.userData?.authenticateResponseDto) {
      const dto = this.userData.authenticateResponseDto;
      this.userSection = {
        firstName: dto.firstName || '',
        lastName: dto.lastName || '',
        email: dto.email || '',
        phone: dto.phone || ''
      };

      this.originalUserData = { ...this.userSection };

      if (dto.dealerInfo) {
        console.log('Populating dealer data:', dto.dealerInfo);
        this.dealerSection = {
          dealershipName: dto.dealerInfo.dealershipName || '',
          contactPerson: dto.dealerInfo.contactPerson || '',
          email: dto.dealerInfo.email || '',
          phone: dto.dealerInfo.phone || '',
          alternatePhone: dto.dealerInfo.alternatePhone || '',
          addressLine1: dto.dealerInfo.addressLine1 || '',
          addressLine2: dto.dealerInfo.addressLine2 || '',
          city: dto.dealerInfo.city || '',
          state: dto.dealerInfo.state || '',
          country: dto.dealerInfo.country || '',
          postalCode: dto.dealerInfo.postalCode || '',
          gstNumber: dto.dealerInfo.gstNumber || '',
          registrationNumber: dto.dealerInfo.registrationNumber || '',
          tradeLicenseNumber: dto.dealerInfo.tradeLicenseNumber || '',
          website: dto.dealerInfo.website || ''
        };
      }
      if (dto.roleInfo?.name === "Administrator") {
        this.isAdmin = true;
      }
    }
  }

  // ============ UI CONTROL METHODS ============

  enableEdit(): void {
    this.isEditing = true;
    this.originalUserData = { ...this.userSection };
    this.clearMessages();
  }

  cancelEdit(): void {
    this.userSection = { ...this.originalUserData };
    this.isEditing = false;
    this.clearMessages();
  }

  // ============ VALIDATION METHODS ============

  private validateUserData(): boolean {
    if (!this.userData?.authenticateResponseDto) {
      this.errorMessage = 'User data not found';
      return false;
    }

    if (!this.userSection.firstName?.trim() || !this.userSection.lastName?.trim()) {
      this.errorMessage = 'First name and last name are required';
      return false;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.userSection.email)) {
      this.errorMessage = 'Please enter a valid email address';
      return false;
    }

    if (!this.userSection.phone?.trim() || this.userSection.phone.length < 10) {
      this.errorMessage = 'Please enter a valid phone number (minimum 10 digits)';
      return false;
    }

    return true;
  }

  private getUpdateData(): UpdateUserRequest {
    return {
      firstName: this.userSection.firstName.trim(),
      lastName: this.userSection.lastName.trim(),
      email: this.userSection.email.trim(),
      phone: this.userSection.phone.trim(),
      userId: this.userId,
      modifiedBy: this.userId
    };
  }

  // ============ UPDATE METHODS ============

  saveUserChanges(): void {
    this.loader.show();
    if (!this.validateUserData()) {
      this.loader.hide();
      return;
    }
    const updateData = this.getUpdateData();
    this.userService.updateUserProfile(this.userId, updateData).subscribe({
      next: (response) => {
        console.log(response.message);
        this.toastr.success(response.message);
        this.updateUserDataInStorage(response);
        this.clearMessages();
        this.loader.hide();
        this.isEditing = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error updating user:', error);
        this.toastr.error('Error updating user:' + error);
        this.clearMessages();
        this.loader.hide();
      }
    });
  }

  // ============ RESPONSE HANDLING METHODS ============
  private updateUserDataInStorage(responseData: UpdateUserResponse): void {
    if (!this.userData?.authenticateResponseDto) {
      return;
    }
    // Use nullish coalescing to safely access properties
    this.userData.authenticateResponseDto.firstName = responseData.data?.firstName;
    this.userData.authenticateResponseDto.lastName = responseData.data?.lastName;
    this.userData.authenticateResponseDto.email = responseData.data?.email;
    this.userData.authenticateResponseDto.phone = responseData.data?.phone;
    localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(this.userData));
    this.originalUserData = { ...this.userSection };
  }

  enableDealerEdit() {

  }
  // ============ UTILITY METHODS ============

  private clearMessages(): void {
    this.errorMessage = '';
    this.successMessage = '';
  }

  // ============ EVENT HANDLERS ============

  onPasswordResetSuccess(success: boolean): void {
    if (success) {
      this.toastr.success('Password updated successfully!', 'Success');
    }
  }
}
