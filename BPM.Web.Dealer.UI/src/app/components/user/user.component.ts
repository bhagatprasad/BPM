import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SpinnerLoadingService } from '@app/common/services/spinner-loading-service';
import { userInformation } from '@app/models/user';
import { UserDetailsService } from '@app/services/user.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
})
export class UserComponent {
  userInformation: userInformation[] = [];
  dealerId: string = '';
  error: string | null = null;
  userData: any;
  userId: any;
  constructor(private userService: UserDetailsService) { }

  ngOnInit(): void {
     const storedData = localStorage.getItem('AuthenticatedUserResponse');
    if (storedData) {
      this.userData = JSON.parse(storedData);
      console.log('Full userData:', this.userData);
      this.userId = this.userData.authenticateResponseDto.userId;
      this.dealerId=this.userData.authenticateResponseDto.dealerId;
    }
    this.loadUsers();
  }
  loadUsers(): void {
    
    this.userService.getAllUsersByDealerId(this.dealerId).subscribe({
      next: (response: userInformation[]) => {
        console.log('Users fetched successfully:', response);
        this.userInformation = response || [];
      },
      error: (error) => {
        console.error('Error fetching users:', error);
        this.error = 'Failed to load users. Please try again.';
        this.userInformation = [];
      }
    });
  }
}
