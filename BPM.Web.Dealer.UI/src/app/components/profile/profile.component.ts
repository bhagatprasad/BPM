import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UpdateUserRequest, UpdateUserResponse } from '../../models/user-profile';
import { UserService } from '../../services/profile.service';
import { ResetPasswordComponent } from '../reset-password/reset-password.component';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { SpinnerLoadingService } from '../../common/services/spinner-loading-service';
import { DealerService } from '../../services/dealer.service';
import { UpdatedDealerResponse } from '../../models/dealer-profile';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [FormsModule, CommonModule, ResetPasswordComponent],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {



  userData: any = null;
  dealerData: any = null;
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
  isDealerEditing : boolean = false;
  originalUserData: any = {};
  originalDealerData: any = {};

  errorMessage: string = '';
  successMessage: string = '';

  userId: string = '';


  constructor(
    private userService: UserService,
    private dealerService:DealerService,
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
      this.originalDealerData = {...this.dealerSection};
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

   enableDealerEdit():void {
    this.isDealerEditing = true;
    this.originalDealerData = {...this.dealerSection};
    this.clearMessages();
  }
  cancelDealerEdit() {
    this.originalDealerData={...this.dealerSection};
 this.isDealerEditing = false;
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

  private updatedUserData = {
    
      firstName: this.userSection.firstName.trim(),
      lastName: this.userSection.lastName.trim(),
      email: this.userSection.email.trim(),
      phone: this.userSection.phone.trim(),
      userId: this.userId,
      modifiedBy: this.userId
    
  };
 
  private validateDealerData(): boolean {
    if(!this.dealerSection?.dealershipName?.trim())
    {
      this.errorMessage ='Please enter dealer name';
      return false;
    }
    if(!this.dealerSection?.contactPerson?.trim())
    {
      this.errorMessage='please enter contactPerson';
       return false;
    }
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if(emailRegex.test(this.dealerSection?.email))
    {
      this.errorMessage='please enter valid email address';
      return false;
    }
    if(!this.dealerSection?.phone.trim()||this.dealerSection?.phone.length<10)
    {
      this.errorMessage = 'please enter valid phone number(minumum 10 digits)';
      return false;
    }
    if(!this.dealerSection.alternatePhone?.trim() || this.dealerSection?.alternatePhone.length<10 )
    {
      this.errorMessage ='please enter valid phone number(minumum 10 digits)';
      return false;
    }
    if(!this.dealerSection?.addressLine1?.trim())
    {
      this.errorMessage ='please enter addressLine1';
      return false
    }
    if(!this.dealerSection?.addressLine2?.trim())
    {
      this.errorMessage ='please enter addressLine2';
      return false;
    }
    if(!this.dealerSection?.city?.trim())
    {
      this.errorMessage ='please enter city name';
      return false;
    }
    if(!this.dealerSection?.state?.trim())
    {
      this.errorMessage ='please enter state name';
      return false;
    }
    if(!this.dealerSection?.country?.trim())
    {
      this.errorMessage ='please enter country name';
      return false;
    }
    if(!this.dealerSection?.postalCode?.trim())
    {
      this.errorMessage ='please enter postalCode name';
      return false;
    }
    if(!this.dealerSection?.gstNumber?.trim())
    {
      this.errorMessage ='please enter gstNumber name';
      return false;
    }
    if(!this.dealerSection?.registrationNumber?.trim())
    {
      this.errorMessage ='please enter registrationNumber name';
      return false;
    }
    if(!this.dealerSection?.tradeLicenseNumber?.trim())
    {
      this.errorMessage ='please enter tradeLicenseNumber name';
      return false;
    }
    const websiteRegex = /^(https?:\/\/)?([\w-]+\.)+[\w-]{2,}(\/.*)?$/i;

    if (!websiteRegex.test(this.dealerSection.website.trim()))
    {
       this.errorMessage = 'Please enter a valid website';
       return false;
    }
    return true

  }
   private updatedDealer={
          dealershipName :this.dealerSection.dealershipName.trim(),
          contactPerson : this.dealerSection.contactPerson.trim(),
          email : this.dealerSection.contactPerson.trim(),
          phone : this.dealerSection.phone.trim(),
          alternatePhone : this.dealerSection.alternatePhone.trim(),
          addressLine1 : this.dealerSection.addressLine1.trim(),
          addressLine2 : this.dealerSection.addressLine2.trim(),
          city : this.dealerSection.city.trim(),
          state : this.dealerSection.state.trim(),
          country : this.dealerSection.country.trim(),
          postalCode : this.dealerSection.postalCode.trim(),
          gstNumber : this.dealerSection.gstNumber.trim(),
          registrationNumber : this.dealerSection.gstNumber.trim(),
          tradeLicenseNumber : this.dealerSection.tradeLicenseNumber.trim(),
          website :this.dealerSection.website.trim(),
          modifiedBy : this.userId
  }


  // ============ UPDATE METHODS ============

  saveUserChanges(): void {
    this.loader.show();
    if (!this.validateUserData()) {
      this.loader.hide();
      return;
    }
    const updateData = this.updatedUserData;
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
saveDealerChanges() {
this.loader.show();
if(!this.validateDealerData())
{
  this.loader.hide();
  return ;
}
const updatedDealerData = this.updatedDealer
this.dealerService.updateDealerAsync(this.userId,updatedDealerData).subscribe({
  next:(response) => {
console.log(response.message);
this.toastr.success(response.message);
this.updateDealerDataInStorage(response);
this.clearMessages();
this.loader.hide();
this.isDealerEditing=false
this.cdr.detectChanges();
  }
})
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
updateDealerDataInStorage(responseData: UpdatedDealerResponse) {
    this.userData.authenticateResponseDto.dealerInfo.dealershipName =responseData.data?.dealershipName;
    this.userData.authenticateResponseDto.dealerInfo.contactPerson =responseData.data?.contactPerson;
    this.userData.authenticateResponseDto.dealerInfo.email =responseData.data?.email;
    this.userData.authenticateResponseDto.dealerInfo.phone =responseData.data?.phone;
    this.userData.authenticateResponseDto.dealerInfo.alternatePhone =responseData.data?.alternatePhone;
    this.userData.authenticateResponseDto.dealerInfo.addressLine1 =responseData.data?.addressLine1;
    this.userData.authenticateResponseDto.dealerInfo.addressLine2 =responseData.data?.addressLine2;
    this.userData.authenticateResponseDto.dealerInfo.city =responseData.data?.city;
    this.userData.authenticateResponseDto.dealerInfo.state =responseData.data?.state;
    this.userData.authenticateResponseDto.dealerInfo.country =responseData.data?.country;
    this.userData.authenticateResponseDto.dealerInfo.postalCode =responseData.data?.postalCode;
    this.userData.authenticateResponseDto.dealerInfo.gstNumber =responseData.data?.gstNumber;
    this.userData.authenticateResponseDto.dealerInfo.registrationNumber =responseData.data?.registrationNumber;
    this.userData.authenticateResponseDto.dealerInfo.tradeLicenseNumber =responseData.data?.tradeLicenseNumber;
    this.userData.authenticateResponseDto.dealerInfo.website =responseData.data?.website;

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
