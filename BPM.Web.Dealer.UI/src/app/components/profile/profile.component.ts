import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, finalize } from 'rxjs/operators';
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
  
  // Tab management
  activeTab: string = 'personal';

  // User section fields (editable)
  userSection = {
    firstName: '',
    lastName: '',
    email: '',
    phone: ''
  };

  // Dealer section fields (read-only)
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

  // Track edit mode for user section
  isEditing: boolean = false;
  originalUserData: any = {};

  // Loading and error states
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  // User ID for password reset
  userId: string = '';

  // Method selection - default to POST
  private updateMethod: 'POST' | 'PUT' | 'PATCH' = 'POST';

  constructor(
    private userService: UserService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.loadUserData();
  }

  loadUserData(): void {
    const storedData = localStorage.getItem('AuthenticatedUserResponse');
    if (storedData) {
      try {
        this.userData = JSON.parse(storedData);
        console.log('Full userData:', this.userData);
        
        if (this.userData.authenticateResponseDto) {
          console.log('Using authenticateResponseDto data');
          this.populateFormData();
          // Set userId for password reset component
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
    if (this.userData && this.userData.authenticateResponseDto) {
      const dto = this.userData.authenticateResponseDto;
      
      // Populate user section from the main DTO
      this.userSection = {
        firstName: dto.firstName || '',
        lastName: dto.lastName || '',
        email: dto.email || '',
        phone: dto.phone || ''
      };

      this.originalUserData = { ...this.userSection };

      // Populate dealer section from dealerInfo
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
    }
  }

  enableEdit(): void {
    this.isEditing = true;
    this.originalUserData = { ...this.userSection };
    this.errorMessage = '';
    this.successMessage = '';
  }

  /**
   * Save user changes - Automatically chooses the best method
   * Tries PUT first, if fails with 405, tries POST
   */
  saveUserChanges(): void {
    if (!this.userData || !this.userData.authenticateResponseDto) {
      this.errorMessage = 'User data not found';
      return;
    }

    // Validate required fields
    if (!this.userSection.firstName.trim() || !this.userSection.lastName.trim()) {
      this.errorMessage = 'First name and last name are required';
      return;
    }

    // Validate email format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.userSection.email)) {
      this.errorMessage = 'Please enter a valid email address';
      return;
    }

    // Validate phone number
    if (!this.userSection.phone.trim() || this.userSection.phone.length < 10) {
      this.errorMessage = 'Please enter a valid phone number (minimum 10 digits)';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const updateData: UpdateUserRequest = {
      firstName: this.userSection.firstName.trim(),
      lastName: this.userSection.lastName.trim(),
      email: this.userSection.email.trim(),
      phone: this.userSection.phone.trim()
    };

    const userId = this.userData.authenticateResponseDto.userId;

    console.log('📤 Updating user profile...');
    console.log('🆔 UserId:', userId);
    console.log('📦 Data:', updateData);

    // Try PUT first (recommended for updates)
    this.updateMethod = 'PUT';
    this.executeUpdate(userId, updateData);
  }

  /**
   * Execute the update with the current method
   * Falls back to POST if PUT/PATCH fails with 405
   */
  private executeUpdate(userId: string, updateData: UpdateUserRequest): void {
    console.log('🔧 Using method:', this.updateMethod);

    let updateObservable: Observable<UpdateUserResponse>;

    // Choose the method based on this.updateMethod
    switch (this.updateMethod) {
      case 'PUT':
        updateObservable = this.userService.updateUserProfilePut(userId, updateData);
        break;
      case 'PATCH':
        // For PATCH, only send fields that have changed
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
        
        // If no changes, show message and exit
        if (Object.keys(patchData).length === 0) {
          this.isLoading = false;
          this.toastr.info('No changes to save', 'Info');
          return;
        }
        
        updateObservable = this.userService.updateUserProfilePatch(userId, patchData);
        break;
      case 'POST':
      default:
        updateObservable = this.userService.updateUserProfile(userId, updateData);
        break;
    }

    updateObservable
      .pipe(
        catchError(error => {
          console.error(`❌ ${this.updateMethod} Update error:`, error);
          
          // If current method fails with 405, try POST as fallback
          if (error.status === 405 && this.updateMethod !== 'POST') {
            console.log(`🔄 ${this.updateMethod} not allowed, trying POST as fallback...`);
            this.updateMethod = 'POST';
            return this.userService.updateUserProfile(userId, updateData);
          }
          
          // If POST also fails or other error
          this.errorMessage = error.message || 'Unable to update profile. Please try again later.';
          this.toastr.error(this.errorMessage, 'Error');
          return throwError(error);
        }),
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe({
        next: (response: UpdateUserResponse) => {
          console.log('✅ Update response:', response);
          
          if (response && response.success) {
            // Update the DTO data
            if (this.userData && this.userData.authenticateResponseDto) {
              this.userData.authenticateResponseDto.firstName = response.firstName || updateData.firstName;
              this.userData.authenticateResponseDto.lastName = response.lastName || updateData.lastName;
              this.userData.authenticateResponseDto.email = response.email || updateData.email;
              this.userData.authenticateResponseDto.phone = response.phone || updateData.phone;
            }

            localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(this.userData));
            this.originalUserData = { ...this.userSection };
            this.isEditing = false;
            this.successMessage = response.message || 'Profile updated successfully!';
            this.toastr.success(this.successMessage, 'Success');
            this.populateFormData();

            setTimeout(() => {
              this.successMessage = '';
            }, 5000);
          } else {
            this.errorMessage = response.message || 'Failed to update profile';
            this.toastr.error(this.errorMessage, 'Error');
          }
        },
        error: (error) => {
          // Error already handled in catchError
          this.isLoading = false;
        }
      });
  }

  /**
   * Alternative: Force using POST method
   */
  saveWithPost(): void {
    if (!this.userData || !this.userData.authenticateResponseDto) {
      this.errorMessage = 'User data not found';
      return;
    }

    // Validate required fields
    if (!this.userSection.firstName.trim() || !this.userSection.lastName.trim()) {
      this.errorMessage = 'First name and last name are required';
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.userSection.email)) {
      this.errorMessage = 'Please enter a valid email address';
      return;
    }

    if (!this.userSection.phone.trim() || this.userSection.phone.length < 10) {
      this.errorMessage = 'Please enter a valid phone number (minimum 10 digits)';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const updateData: UpdateUserRequest = {
      firstName: this.userSection.firstName.trim(),
      lastName: this.userSection.lastName.trim(),
      email: this.userSection.email.trim(),
      phone: this.userSection.phone.trim()
    };

    const userId = this.userData.authenticateResponseDto.userId;

    console.log('📤 Updating user profile with POST...');
    console.log('🆔 UserId:', userId);
    console.log('📦 Data:', updateData);

    this.userService.updateUserProfile(userId, updateData)
      .pipe(
        catchError(error => {
          console.error('❌ POST Update error:', error);
          this.errorMessage = error.message || 'Unable to update profile. Please try again later.';
          this.toastr.error(this.errorMessage, 'Error');
          return throwError(error);
        }),
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe({
        next: (response: UpdateUserResponse) => {
          console.log('✅ Update response:', response);
          
          if (response && response.success) {
            if (this.userData && this.userData.authenticateResponseDto) {
              this.userData.authenticateResponseDto.firstName = response.firstName || updateData.firstName;
              this.userData.authenticateResponseDto.lastName = response.lastName || updateData.lastName;
              this.userData.authenticateResponseDto.email = response.email || updateData.email;
              this.userData.authenticateResponseDto.phone = response.phone || updateData.phone;
            }

            localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(this.userData));
            this.originalUserData = { ...this.userSection };
            this.isEditing = false;
            this.successMessage = response.message || 'Profile updated successfully!';
            this.toastr.success(this.successMessage, 'Success');
            this.populateFormData();

            setTimeout(() => {
              this.successMessage = '';
            }, 5000);
          } else {
            this.errorMessage = response.message || 'Failed to update profile';
            this.toastr.error(this.errorMessage, 'Error');
          }
        },
        error: (error) => {
          this.isLoading = false;
        }
      });
  }

  cancelEdit(): void {
    this.userSection = { ...this.originalUserData };
    this.isEditing = false;
    this.errorMessage = '';
    this.successMessage = '';
  }

  // Handle password reset success
  onPasswordResetSuccess(success: boolean): void {
    if (success) {
      this.toastr.success('Password updated successfully!', 'Success');
    }
  }
}