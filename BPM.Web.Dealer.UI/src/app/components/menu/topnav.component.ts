import {
  Component,
  OnInit,
  OnDestroy,
  inject,
  HostListener,
  ChangeDetectorRef,
  NgZone,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { Subscription, interval } from 'rxjs';
import { CartService } from '@app/services/cart.service';

interface Language {
  name: string;
  code: string;
  flag: string;
}

interface Message {
  sender: string;
  avatar: string;
  time: string;
  preview: string;
  read: boolean;
}

interface Notification {
  image: string;
  iconClass: string;
  message: string;
  time: string;
  read: boolean;
}

@Component({
  selector: 'app-topnav',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './topnav.component.html',
  styleUrls: ['./topnav.component.css'],
})
export class TopnavComponent implements OnInit, OnDestroy {
  private accountService = inject(AccountService);
  private router = inject(Router);
  private cartService = inject(CartService);
  private cdr = inject(ChangeDetectorRef);
  private ngZone = inject(NgZone);

  firstName: string = '';
  lastName: string = '';
  dealerShipName: string = '';
  userRole: string = '';
  userRoleDisplay: string = '';
  userEmail: string = '';
  isAuthenticated = false;
  isDarkMode = false;
  selectedLanguage: string = 'English';
  isScrolled: boolean = false;
  cartCount = 0;
  currentDateTime: Date = new Date();
  private authSubscription?: Subscription;
  private clockSubscription?: Subscription;

  // Languages data
  languages: Language[] = [
    { name: 'English', code: 'en', flag: 'assets/images/usa.png' },
    { name: 'Australia', code: 'au', flag: 'assets/images/australia.png' },
    { name: 'Spanish', code: 'es', flag: 'assets/images/spain.png' },
    { name: 'France', code: 'fr', flag: 'assets/images/france.png' },
    { name: 'Germany', code: 'de', flag: 'assets/images/germany.png' },
  ];

  // Messages data
  messages: Message[] = [
    {
      sender: 'Jacob Liwiski',
      avatar: 'assets/images/user1.jpg',
      time: '35 min ago',
      preview: 'Hey Victor! Could you please review the latest proposal?',
      read: false,
    },
    {
      sender: 'Angela Carter',
      avatar: 'assets/images/user2.jpg',
      time: '1 day ago',
      preview: 'How are you Angela? Would you please join the meeting?',
      read: false,
    },
    {
      sender: 'Brad Traversy',
      avatar: 'assets/images/user3.jpg',
      time: '2 days ago',
      preview: 'Hey Brad Traversy! Could you please share the files?',
      read: true,
    },
  ];

  // Notifications data
  notifications: Notification[] = [
    {
      image: 'ri-sms-line',
      iconClass: 'text-primary',
      message: 'You have requested to withdrawal amount $500',
      time: '2 hrs ago',
      read: false,
    },
    {
      image: 'ri-user-line',
      iconClass: 'text-info',
      message: 'A new user "John Doe" added in StarCode',
      time: '3 hrs ago',
      read: false,
    },
    {
      image: 'ri-mail-line',
      iconClass: 'text-success',
      message: 'You have received a new message from Sarah',
      time: '1 day ago',
      read: true,
    },
  ];

  @HostListener('window:scroll', [])
  onWindowScroll() {
    const scrollPosition = window.pageYOffset || document.documentElement.scrollTop || 0;
    this.isScrolled = scrollPosition > 10;
  }

  ngOnInit(): void {
    // Start real-time clock
    this.startClock();

    // Subscribe to cart count
    this.cartService.cartCount$.subscribe((count) => {
      this.cartCount = count;
      this.cdr.detectChanges();
    });

    // Subscribe to auth state changes
    this.authSubscription = this.accountService.authState$.subscribe((isAuth) => {
      this.isAuthenticated = isAuth;
      if (isAuth) {
        this.loadUserData();
      }
    });

    // Load initial user data
    if (this.accountService.isAuthenticatedSync) {
      this.loadUserData();
    }

    // Check dark mode preference
    this.isDarkMode = localStorage.getItem('darkMode') === 'true';
    this.applyDarkMode();

    // Load saved language preference
    const savedLanguage = localStorage.getItem('selectedLanguage');
    if (savedLanguage) {
      this.selectedLanguage = savedLanguage;
    }
  }

  ngOnDestroy(): void {
    if (this.authSubscription) {
      this.authSubscription.unsubscribe();
    }
    if (this.clockSubscription) {
      this.clockSubscription.unsubscribe();
    }
  }

  private startClock(): void {
    this.ngZone.runOutsideAngular(() => {
      this.clockSubscription = interval(1000).subscribe(() => {
        this.ngZone.run(() => {
          this.currentDateTime = new Date();
        });
      });
    });
  }

  private loadUserData(): void {
    const user = this.accountService.getCurrentUser();
    if (user) {
      // Get user details from authenticateResponseDto
      const authDto = user.authenticateResponseDto;
      
      this.firstName = authDto?.firstName || '';
      this.lastName = authDto?.lastName || '';
      this.userEmail = authDto?.email || '';
      
      // Get dealership name from dealerInfo
      this.dealerShipName = authDto?.dealerInfo?.dealershipName || '';
      
      // Get role name from roleInfo
      this.userRole = authDto?.roleInfo?.name || '';
      this.userRoleDisplay = this.getRoleDisplayName(this.userRole);
      
      console.log('User Data Loaded:', {
        firstName: this.firstName,
        lastName: this.lastName,
        dealerShipName: this.dealerShipName,
        userRole: this.userRole,
        userEmail: this.userEmail
      });
    }
  }

  /**
   * Get formatted role display name with icon
   */
  getRoleDisplayName(role: string): string {
    if (!role) return 'User';
    
    const roleMap: { [key: string]: string } = {
      'Administrator': '👑 Admin',
      'Admin': '👑 Admin',
      'Operator': '⚙️ Operator',
      'User': '👤 User',
      'Dealer': '🏪 Dealer'
    };
    
    return roleMap[role] || role;
  }

  getGreeting(): string {
    const hour = new Date().getHours();
    let greeting = 'Welcome';

    if (hour < 12) {
      greeting = 'Good Morning';
    } else if (hour < 17) {
      greeting = 'Good Afternoon';
    } else if (hour < 21) {
      greeting = 'Good Evening';
    } else {
      greeting = 'Good Night';
    }

    return `Welcome | ${greeting}`;
  }

  getFullName(): string {
    if (this.firstName && this.lastName) {
      return `${this.firstName} ${this.lastName}`;
    }
    return this.firstName || this.lastName || 'User';
  }

  getInitials(): string {
    const firstName = this.firstName?.charAt(0) || '';
    const lastName = this.lastName?.charAt(0) || '';
    return (firstName + lastName).toUpperCase() || 'U';
  }

  getRoleBadgeClass(): string {
    const role = this.userRole?.toLowerCase() || '';
    
    if (role === 'administrator' || role === 'admin') {
      return 'bg-danger text-white px-2 py-1 rounded';
    } else if (role === 'operator') {
      return 'bg-warning text-dark px-2 py-1 rounded';
    } else if (role === 'dealer') {
      return 'bg-success text-white px-2 py-1 rounded';
    } else {
      return 'bg-info text-white px-2 py-1 rounded';
    }
  }

  getRoleIcon(): string {
    const role = this.userRole?.toLowerCase() || '';
    
    if (role === 'administrator' || role === 'admin') {
      return 'ri-admin-line';
    } else if (role === 'operator') {
      return 'ri-settings-5-line';
    } else if (role === 'dealer') {
      return 'ri-store-3-line';
    } else {
      return 'ri-user-3-line';
    }
  }

  getUnreadMessagesCount(): number {
    return this.messages.filter((msg) => !msg.read).length;
  }

  getUnreadNotificationsCount(): number {
    return this.notifications.filter((notif) => !notif.read).length;
  }

  toggleDarkMode(): void {
    this.isDarkMode = !this.isDarkMode;
    localStorage.setItem('darkMode', String(this.isDarkMode));
    this.applyDarkMode();
  }

  private applyDarkMode(): void {
    if (this.isDarkMode) {
      document.documentElement.setAttribute('data-bs-theme', 'dark');
    } else {
      document.documentElement.removeAttribute('data-bs-theme');
    }
  }

  toggleSidebar(): void {
    const sidebar = document.getElementById('sidebar-area');
    if (sidebar) {
      sidebar.classList.toggle('active');
    }
  }

  changeLanguage(language: Language): void {
    this.selectedLanguage = language.name;
    localStorage.setItem('selectedLanguage', language.name);
    console.log('Language changed to:', language.name);
  }

  markAllMessagesAsRead(): void {
    this.messages = this.messages.map((msg) => ({ ...msg, read: true }));
  }

  markAllNotificationsAsRead(): void {
    this.notifications = this.notifications.map((notif) => ({ ...notif, read: true }));
  }

  clearAllNotifications(): void {
    this.notifications = [];
  }

  // Navigation methods with role-based routing
  goToProfile(): void {
    const basePath = this.accountService.getBasePath();
    this.router.navigate([basePath + '/profile']);
  }

  goToSettings(): void {
    const basePath = this.accountService.getBasePath();
    this.router.navigate([basePath + '/settings/account']);
  }

  goToSupport(): void {
    const basePath = this.accountService.getBasePath();
    this.router.navigate([basePath + '/helpdesk/tickets']);
  }

  goToMessages(): void {
    this.router.navigate(['/apps/chat']);
  }

  goToNotifications(): void {
    this.router.navigate(['/pages/notifications']);
  }

  goToCalendar(): void {
    this.router.navigate(['/apps/calendar']);
  }

  goToCart(): void {
    const basePath = this.accountService.getBasePath();
    this.router.navigate([basePath + '/cart']);
  }

  goToDashboard(): void {
    const basePath = this.accountService.getBasePath();
    this.router.navigate([basePath + '/dashboard']);
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigate(['/login']);
  }
  /**
 * Get avatar gradient based on user role
 */
getAvatarGradient(): string {
  const role = this.userRole?.toLowerCase() || '';
  
  if (role === 'administrator' || role === 'admin') {
    return 'linear-gradient(135deg, #dc3545, #c82333)'; // Red gradient for Admin
  } else if (role === 'operator') {
    return 'linear-gradient(135deg, #ffc107, #e0a800)'; // Yellow gradient for Operator
  } else if (role === 'dealer') {
    return 'linear-gradient(135deg, #28a745, #1e7e34)'; // Green gradient for Dealer
  } else {
    return 'linear-gradient(135deg, #0d9488, #0e7490)'; // Teal gradient for default
  }
}
}