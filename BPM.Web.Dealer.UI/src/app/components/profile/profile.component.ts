import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { AuthenticateResponse, UpdateUserRequest, UpdateUserResponse } from '../../models/user-profile';
import { UserService } from '../../services/profile.service';
import { ResetPasswordComponent } from '../reset-password/reset-password.component';
import { ToastrService } from '@iqx-limited/ngx-toastr';

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

  private updateMethod: 'POST' | 'PUT' | 'PATCH' = 'POST';

  constructor(
    private userService: UserService,
    private toastr: ToastrService,
  ) { }

  ngOnInit(): void {
    this.loadUserData();
  }

  // ============ DATA LOADING METHODS ============
  
  loadUserData(): void {
    const storedData = localStorage.getItem('AuthenticatedUserResponse');
    if (storedData) {
      try {
        this.userData = JSON.parse(storedData);
        console.log('Full userData:', this.userData);

        if (this.userData?.authenticateResponseDto) {
          console.log('Using authenticateResponseDto data');
          this.populateFormData();
          this.userId = this.userData.authenticateResponseDto.userId;
        } else {
          console.error('Invalid data structure - missing authenticateResponseDto');
          this.errorMessage = 'Invalid user data format';
        }
      } catch (error) {
        console.error('Error parsing user data:', error);
        this.errorMessage = 'Failed to load user data';
      }
    } else {
      this.errorMessage = 'No user data found. Please login again.';
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
      phone: this.userSection.phone.trim()
    };
  }

  // ============ UPDATE METHODS ============

  saveUserChanges(): void {
    if (!this.validateUserData()) {
      return;
    }

    this.clearMessages();

    const userId = this.userData.authenticateResponseDto.userId;
    const updateData = this.getUpdateData();

    console.log('📤 Updating user profile...');
    console.log('🆔 UserId:', userId);
    console.log('📦 Data:', updateData);

    this.updateMethod = 'PUT';
    this.executeUpdate(userId, updateData);
  }

  saveWithPost(): void {
    if (!this.validateUserData()) {
      return;
    }

    this.clearMessages();

    const userId = this.userData.authenticateResponseDto.userId;
    const updateData = this.getUpdateData();

    console.log('📤 Updating user profile with POST...');
    console.log('🆔 UserId:', userId);
    console.log('📦 Data:', updateData);

    this.updateMethod = 'POST';
    this.executeUpdate(userId, updateData);
  }

  private executeUpdate(userId: string, updateData: UpdateUserRequest): void {
    console.log('🔧 Using method:', this.updateMethod);

    const updateObservable = this.getUpdateObservable(userId, updateData);

    updateObservable
      .pipe(
        catchError(error => {
          console.error(`❌ ${this.updateMethod} Update error:`, error);

          // Try fallback if method not allowed
          if (error.status === 405 && this.updateMethod !== 'POST') {
            console.log(`🔄 ${this.updateMethod} not allowed, trying POST as fallback...`);
            this.updateMethod = 'POST';
            return this.userService.updateUserProfile(userId, updateData);
          }

          this.errorMessage = error.message || 'Unable to update profile. Please try again later.';
          this.toastr.error(this.errorMessage, 'Error');
          return throwError(error);
        })
      )
      .subscribe({
        next: (response: UpdateUserResponse) => {
          this.handleUpdateSuccess(response, updateData);
        },
        error: (error) => {
          // Error already handled in catchError
          console.error('Update failed:', error);
        }
      });
  }

  private getUpdateObservable(userId: string, updateData: UpdateUserRequest): Observable<UpdateUserResponse> {
    switch (this.updateMethod) {
      case 'PUT':
        return this.userService.updateUserProfilePut(userId, updateData);
      case 'PATCH':
        const patchData = this.getPatchData();
        if (Object.keys(patchData).length === 0) {
          this.toastr.info('No changes to save', 'Info');
          return throwError(() => new Error('No changes to save'));
        }
        return this.userService.updateUserProfilePatch(userId, patchData);
      case 'POST':
      default:
        return this.userService.updateUserProfile(userId, updateData);
    }
  }

  private getPatchData(): Partial<UpdateUserRequest> {
    const patchData: Partial<UpdateUserRequest> = {};
    
    if (this.userSection.firstName !== this.originalUserData.firstName) {
      patchData.firstName = this.userSection.firstName.trim();
    }
    if (this.userSection.lastName !== this.originalUserData.lastName) {
      patchData.lastName = this.userSection.lastName.trim();
    }
    if (this.userSection.email !== this.originalUserData.email) {
      patchData.email = this.userSection.email.trim();
    }
    if (this.userSection.phone !== this.originalUserData.phone) {
      patchData.phone = this.userSection.phone.trim();
    }

    return patchData;
  }

  // ============ RESPONSE HANDLING METHODS ============

  private handleUpdateSuccess(response: UpdateUserResponse, updateData: UpdateUserRequest): void {
    console.log('✅ Update response:', response);

    // Check if response is valid and has data
    if (response?.data) {
      this.updateUserDataInStorage(response.data, updateData);
      this.isEditing = false;
      this.successMessage = response.message || 'Profile updated successfully!';
      this.toastr.success(this.successMessage, 'Success');
      this.populateFormData();

      setTimeout(() => {
        this.successMessage = '';
      }, 5000);
    } else {
      // Handle case where response.data is undefined
      this.errorMessage = response?.message || 'Failed to update profile';
      this.toastr.error(this.errorMessage, 'Error');
    }
  }

  private updateUserDataInStorage(responseData: UpdateUserResponse['data'], updateData: UpdateUserRequest): void {
    if (!this.userData?.authenticateResponseDto) {
      return;
    }

    // Use nullish coalescing to safely access properties
    this.userData.authenticateResponseDto.firstName = responseData?.firstName ?? updateData.firstName;
    this.userData.authenticateResponseDto.lastName = responseData?.lastName ?? updateData.lastName;
    this.userData.authenticateResponseDto.email = responseData?.email ?? updateData.email;
    this.userData.authenticateResponseDto.phone = responseData?.phone ?? updateData.phone;

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