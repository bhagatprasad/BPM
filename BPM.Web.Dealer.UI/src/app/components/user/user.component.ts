import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SpinnerLoadingService } from '@app/common/services/spinner-loading-service';
import { userInformation } from '@app/models/user';
import { UserService } from '@app/services/user.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { UserCreateSidebarComponent } from './user-create.component';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, FormsModule, UserCreateSidebarComponent],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
})
export class UserComponent {
  
  @ViewChild(UserCreateSidebarComponent) sidebarComponent!: UserCreateSidebarComponent;

  userInformation: userInformation[] = [];
  dealerId: string = '';
  userData: any;
  userId: any;
  isSidebarVisible: boolean = false;
  

  constructor(
    private userService: UserService,
    private toastr: ToastrService,
    private loader: SpinnerLoadingService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loader.show('Loading users, please wait...');
    const storedData = localStorage.getItem('AuthenticatedUserResponse');
    if (storedData) {
      this.userData = JSON.parse(storedData);
      this.userId = this.userData.authenticateResponseDto.userId;
      this.dealerId = this.userData.authenticateResponseDto.dealerId;
    }
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAllUsersByDealerId(this.dealerId).subscribe({
      next: (response: userInformation[]) => {
        this.userInformation = response || [];
        this.loader.hide();
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error fetching users:', error);
        this.toastr.error('Failed to load users. Please try again.');
        this.userInformation = [];
        this.loader.hide();
        this.cdr.detectChanges();
      }
    });
  }

  openSidebar(): void {
    this.isSidebarVisible = true;
    // Reset the form when opening
    if (this.sidebarComponent) {
      this.sidebarComponent.reset();
    }
  }

  closeSidebar(): void {
    this.isSidebarVisible = false;
  }

  // Handle form submission from sidebar
  handleFormSubmit(userData: any): void {
    // Here you handle the submission in the parent
    this.createUser(userData);
  }

  // Parent handles the API call
  createUser(userData: any): void {
    this.loader.show('Creating user...');

    this.userService.insertUserAsync(userData).subscribe({
      next: (response) => {
        this.loader.hide();
        this.toastr.success('User created successfully!');
        this.closeSidebar(); // Close sidebar on success
        this.loadUsers(); // Refresh the user list
      },
      error: (error) => {
        this.loader.hide();
        console.error('Error creating user:', error);
        this.toastr.error(error.error?.message || 'Failed to create user. Please try again.');
      }
    });
  }
}