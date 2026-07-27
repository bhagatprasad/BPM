import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { catchError, finalize } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { AuthenticateResponse, UpdateUserRequest, UpdateUserResponse } from '../../models/user-profile';
import { UserService } from '../../services/profile.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [FormsModule, CommonModule], // Only import these two
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  userData: AuthenticateResponse | null = null;
  isDealer: boolean = false;
  isUser: boolean = false;

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

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUserData();
  }

  loadUserData(): void {
    const storedData = localStorage.getItem('AuthenticatedUserResponse');
    if (storedData) {
      try {
        this.userData = JSON.parse(storedData);
        this.populateFormData();
        this.determineUserRole();
      } catch (error) {
        console.error('Error parsing user data:', error);
        this.errorMessage = 'Failed to load user data';
      }
    } else {
      this.errorMessage = 'No user data found. Please login again.';
    }
  }

  determineUserRole(): void {
    if (this.userData) {
      if (this.userData.dealerInfo && this.userData.dealerInfo.dealershipName) {
        this.isDealer = true;
        this.isUser = false;
      } else {
        this.isDealer = false;
        this.isUser = true;
      }
    }
  }

  populateFormData(): void {
    if (this.userData) {
      this.userSection = {
        firstName: this.userData.firstName || '',
        lastName: this.userData.lastName || '',
        email: this.userData.email || '',
        phone: this.userData.phone || ''
      };

      this.originalUserData = { ...this.userSection };

      if (this.userData.dealerInfo) {
        this.dealerSection = {
          dealershipName: this.userData.dealerInfo.dealershipName || '',
          contactPerson: this.userData.dealerInfo.contactPerson || '',
          email: this.userData.dealerInfo.email || '',
          phone: this.userData.dealerInfo.phone || '',
          alternatePhone: this.userData.dealerInfo.alternatePhone || '',
          addressLine1: this.userData.dealerInfo.addressLine1 || '',
          addressLine2: this.userData.dealerInfo.addressLine2 || '',
          city: this.userData.dealerInfo.city || '',
          state: this.userData.dealerInfo.state || '',
          country: this.userData.dealerInfo.country || '',
          postalCode: this.userData.dealerInfo.postalCode || '',
          gstNumber: this.userData.dealerInfo.gstNumber || '',
          registrationNumber: this.userData.dealerInfo.registrationNumber || '',
          tradeLicenseNumber: this.userData.dealerInfo.tradeLicenseNumber || '',
          website: this.userData.dealerInfo.website || ''
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

  saveUserChanges(): void {
    if (!this.userData) {
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

    this.userService.updateUserProfile(this.userData.userId, updateData)
      .pipe(
        catchError(error => {
          console.error('Update error:', error);
          // Show a generic user-friendly message
          this.errorMessage = 'Unable to update profile. Please try again later.';
          return throwError(error);
        }),
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe({
        next: (response: UpdateUserResponse) => {
          if (response && response.success) {
            this.userData = {
              ...this.userData!,
              firstName: response.firstName || updateData.firstName,
              lastName: response.lastName || updateData.lastName,
              email: response.email || updateData.email,
              phone: response.phone || updateData.phone
            };

            localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(this.userData));
            this.originalUserData = { ...this.userSection };
            this.isEditing = false;
            this.successMessage = response.message || 'Profile updated successfully!';
            this.populateFormData();

            setTimeout(() => {
              this.successMessage = '';
            }, 5000);
          } else {
            this.errorMessage = response.message || 'Failed to update profile';
          }
        }
      });
  }

  cancelEdit(): void {
    this.userSection = { ...this.originalUserData };
    this.isEditing = false;
    this.errorMessage = '';
    this.successMessage = '';
  }

  hasDealerRole(): boolean {
    return this.isDealer;
  }

  hasUserRole(): boolean {
    return this.isUser;
  }
}

