// user.component.ts
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SpinnerLoadingService } from '@app/common/services/spinner-loading-service';
import { userInformation, UserDistributorUpdateDto } from '@app/models/user';
import { UserService } from '@app/services/user.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { UserCreateSidebarComponent } from './user-create.component';
import { UserUpdateDto } from '@app/models/user-update-dto';
import { UserDeactivateDto } from '@app/models/user-deactivate-dto';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, FormsModule, UserCreateSidebarComponent],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
})
export class UserComponent {
  @ViewChild(UserCreateSidebarComponent) sidebarComponent!: UserCreateSidebarComponent;

  // Data properties
  userInformation: userInformation[] = [];
  allUsers: userInformation[] = [];
  paginatedUsers: userInformation[] = [];
  deactivateUser: UserDeactivateDto[] = [];

  // Selection properties
  selectedUserIds: string[] = [];
  isAllSelected: boolean = false;

  // User properties
  dealerId: string = '';
  userData: any;
  userId: string = '';
  isSidebarVisible: boolean = false;
  searchText = '';
  selectedUser: userInformation | null = null;

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;
  pages: number[] = [];
  maxVisiblePages: number = 5;
  startIndex: number = 0;
  endIndex: number = 0;
  pageSizeOptions: number[] = [5, 10, 25, 50, 100];

  constructor(
    private userService: UserService,
    private toastr: ToastrService,
    private loader: SpinnerLoadingService,
    private cdr: ChangeDetectorRef,
  ) {}

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

  // ============ DATA LOADING ============
  loadUsers(): void {
    this.userService.getAllUsersByDealerId(this.dealerId).subscribe({
      next: (response: userInformation[]) => {
        this.userInformation = response || [];
        this.allUsers = [...this.userInformation];
        this.totalItems = this.userInformation.length;
        this.calculatePagination();
        this.updatePaginatedData();
        this.loader.hide();
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error fetching users:', error);
        this.toastr.error('Failed to load users. Please try again.');
        this.userInformation = [];
        this.allUsers = [];
        this.loader.hide();
        this.cdr.detectChanges();
      },
    });
  }

  // ============ SEARCH (Single Implementation) ============
  onSearch(event: Event): void {
    const searchText = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.searchText = searchText;

    if (!searchText) {
      this.userInformation = [...this.allUsers];
    } else {
      this.userInformation = this.allUsers.filter((user) => {
        const fullName = `${user.firstName ?? ''} ${user.lastName ?? ''}`.toLowerCase();
        return (
          fullName.includes(searchText) ||
          (user.firstName ?? '').toLowerCase().includes(searchText) ||
          (user.lastName ?? '').toLowerCase().includes(searchText) ||
          (user.email ?? '').toLowerCase().includes(searchText) ||
          (user.phone ?? '').toLowerCase().includes(searchText) ||
          (user.roleInfo?.name ?? '').toLowerCase().includes(searchText) ||
          (user.isActive ? 'active' : 'inactive').includes(searchText)
        );
      });
    }

    // Reset pagination after search
    this.currentPage = 1;
    this.totalItems = this.userInformation.length;
    this.calculatePagination();
    this.updatePaginatedData();
    this.cdr.detectChanges();
  }

  // ============ PAGINATION ============
  calculatePagination(): void {
    this.totalPages = Math.ceil(this.totalItems / this.itemsPerPage);
    if (this.currentPage > this.totalPages && this.totalPages > 0) {
      this.currentPage = this.totalPages;
    } else if (this.totalPages === 0) {
      this.currentPage = 1;
    }
    this.generatePageNumbers();
  }

  generatePageNumbers(): void {
    const pages: number[] = [];
    const total = this.totalPages;
    const current = this.currentPage;
    const maxVisible = this.maxVisiblePages;

    if (total <= maxVisible) {
      for (let i = 1; i <= total; i++) {
        pages.push(i);
      }
    } else {
      let start = Math.max(1, current - Math.floor(maxVisible / 2));
      let end = Math.min(total, start + maxVisible - 1);

      if (end === total) {
        start = Math.max(1, total - maxVisible + 1);
      }

      if (start > 1) {
        pages.push(1);
        if (start > 2) {
          pages.push(-1);
        }
      }

      for (let i = start; i <= end; i++) {
        pages.push(i);
      }

      if (end < total) {
        if (end < total - 1) {
          pages.push(-1);
        }
        pages.push(total);
      }
    }
    this.pages = pages;
  }

  updatePaginatedData(): void {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = Math.min(start + this.itemsPerPage, this.totalItems);
    this.paginatedUsers = this.userInformation.slice(start, end);
    this.startIndex = this.totalItems > 0 ? start + 1 : 0;
    this.endIndex = end;
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages || page === this.currentPage) {
      return;
    }
    this.currentPage = page;
    this.updatePaginatedData();
    this.generatePageNumbers();
    this.cdr.detectChanges();
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.goToPage(this.currentPage - 1);
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.goToPage(this.currentPage + 1);
    }
  }

  onItemsPerPageChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.itemsPerPage = parseInt(target.value, 10);
    this.currentPage = 1;
    this.calculatePagination();
    this.updatePaginatedData();
    this.cdr.detectChanges();
  }

  getPaginationText(): string {
    const start = this.startIndex;
    const end = this.endIndex;
    const total = this.totalItems;

    if (total === 0) {
      return 'Showing 0 entries';
    }

    return `Showing ${start} to ${end} of ${total} entries`;
  }

  // ============ SELECTION ============
  getUserId(user: any): string {
    return user.userId || user.id || '';
  }

  isUserSelected(userId: string | undefined): boolean {
    if (!userId) return false;
    return this.selectedUserIds.includes(userId);
  }

  toggleAll(event: any): void {
    this.isAllSelected = event.target.checked;
    if (this.isAllSelected) {
      this.selectedUserIds = this.userInformation
        .map((user) => user.userId)
        .filter((id): id is string => !!id);
    } else {
      this.selectedUserIds = [];
    }
  }

  toggleUser(user: any): void {
    const userId = user.userId || user.id;
    if (!userId) return;

    const index = this.selectedUserIds.indexOf(userId);
    if (index > -1) {
      this.selectedUserIds.splice(index, 1);
    } else {
      this.selectedUserIds.push(userId);
    }

    this.isAllSelected = this.selectedUserIds.length === this.userInformation.length;
  }

  // ============ SIDEBAR ============
  openSidebar(): void {
    this.selectedUser = null;
    this.isSidebarVisible = true;
    if (this.sidebarComponent) {
      this.sidebarComponent.reset();
      this.sidebarComponent.isEditMode = false;
    }
  }

  enableEditUser(user: userInformation): void {
    this.selectedUser = user;
    this.isSidebarVisible = true;
    if (this.sidebarComponent) {
      this.sidebarComponent.openEditMode(user);
    }
  }

  closeSidebar(): void {
    this.isSidebarVisible = false;
    this.selectedUser = null;
  }

  // ============ CRUD OPERATIONS ============
  handleFormSubmit(userData: any): void {
    this.createUser(userData);
  }

  handleUserUpdate(updateData: UserUpdateDto): void {
    this.updateUser(updateData);
  }
  handleUserDistributorUpdate(updateData: UserDistributorUpdateDto): void {
    this.updateUserDistributor(updateData);
  }

  createUser(userData: any): void {
    this.loader.show('Creating user...');
    this.userService.insertUserAsync(userData).subscribe({
      next: (response) => {
        this.loader.hide();
        this.toastr.success('User created successfully!');
        this.closeSidebar();
        this.loadUsers();
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.loader.hide();
        console.error('Error creating user:', error);
        this.toastr.error(error.error?.message || 'Failed to create user. Please try again.');
        this.cdr.detectChanges();
      },
    });
  }

  updateUser(updateData: UserUpdateDto): void {
    this.loader.show('Updating user...');

    const updateUserDto: UserUpdateDto = {
      userId: updateData.userId,
      firstName: updateData.firstName,
      lastName: updateData.lastName,
      email: updateData.email,
      phone: updateData.phone,
      isActive: updateData.isActive,
      modifiedBy: this.userId,
    };

    this.userService.updateUserAsync(updateData.userId, updateUserDto).subscribe({
      next: (response: userInformation) => {
        this.loader.hide();
        this.toastr.success('User updated successfully!');
        this.closeSidebar();
        this.loadUsers();
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.loader.hide();
        console.error('Error updating user:', error);
        this.toastr.error(error.error?.message || 'Failed to update user. Please try again.');
        this.cdr.detectChanges();
      },
    });
  }

  updateUserDistributor(updateData: UserDistributorUpdateDto): void {
    this.loader.show('Updating distributor...');

    const distributorUpdateDto: UserDistributorUpdateDto = {
      userId: updateData.userId,
      distributorId: updateData.distributorId,
      modifiedBy: this.userId,
    };

    this.userService.updateUserDistributor(distributorUpdateDto).subscribe({
      next: () => {
        this.loader.hide();
        this.toastr.success('User distributor updated successfully!');
        this.closeSidebar();
        this.loadUsers();
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.loader.hide();
        console.error('Error updating user distributor:', error);
        this.toastr.error(
          error.error?.message || 'Failed to update user distributor. Please try again.',
        );
        this.cdr.detectChanges();
      },
    });
  }
}
