import { Component, OnInit, OnDestroy, inject, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { Subscription } from 'rxjs';

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
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './topnav.component.html',
  styleUrls: ['./topnav.component.css'],
})
export class TopnavComponent implements OnInit, OnDestroy {
  private accountService = inject(AccountService);
  private router = inject(Router);

  firstName: string = '';
  lastName: string = '';
  dealerShipName: string = '';
  userRole: string = '';
  isAuthenticated = false;
  isDarkMode = false;
  selectedLanguage: string = 'English';
  searchQuery: string = '';
  private authSubscription?: Subscription;
  isScrolled: boolean = false;

  // Languages data
  languages: Language[] = [
    { name: 'English', code: 'en', flag: 'assets/images/usa.png' },
    { name: 'Australia', code: 'au', flag: 'assets/images/australia.png' },
    { name: 'Spanish', code: 'es', flag: 'assets/images/spain.png' },
    { name: 'France', code: 'fr', flag: 'assets/images/france.png' },
    { name: 'Germany', code: 'de', flag: 'assets/images/germany.png' }
  ];

  // Messages data
  messages: Message[] = [
    {
      sender: 'Jacob Liwiski',
      avatar: 'assets/images/user1.jpg',
      time: '35 min ago',
      preview: 'Hey Victor! Could you please review the latest proposal?',
      read: false
    },
    {
      sender: 'Angela Carter',
      avatar: 'assets/images/user2.jpg',
      time: '1 day ago',
      preview: 'How are you Angela? Would you please join the meeting?',
      read: false
    },
    {
      sender: 'Brad Traversy',
      avatar: 'assets/images/user3.jpg',
      time: '2 days ago',
      preview: 'Hey Brad Traversy! Could you please share the files?',
      read: true
    }
  ];

  // Notifications data
  notifications: Notification[] = [
    {
      image: 'ri-sms-line',
      iconClass: 'text-primary',
      message: 'You have requested to withdrawal amount $500',
      time: '2 hrs ago',
      read: false
    },
    {
      image: 'ri-user-line',
      iconClass: 'text-info',
      message: 'A new user "John Doe" added in StarCode',
      time: '3 hrs ago',
      read: false
    },
    {
      image: 'ri-mail-line',
      iconClass: 'text-success',
      message: 'You have received a new message from Sarah',
      time: '1 day ago',
      read: true
    }
  ];

  @HostListener('window:scroll', [])
  onWindowScroll() {
    const scrollPosition = window.pageYOffset || document.documentElement.scrollTop || 0;
    this.isScrolled = scrollPosition > 10;
  }

  ngOnInit(): void {
    // Subscribe to auth state changes
    this.authSubscription = this.accountService.authState$.subscribe(isAuth => {
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
  }

  private loadUserData(): void {
    const user = this.accountService.getCurrentUser();
    if (user) {
      this.firstName = user.authenticateResponseDto?.firstName || '';
      this.lastName = user.authenticateResponseDto?.lastName || '';
      this.dealerShipName = user.authenticateResponseDto?.dealerInfo?.dealershipName || '';
      this.userRole = user.authenticateResponseDto?.roleInfo?.name || '';
    }
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
    if (this.userRole === 'Administrator') {
      return 'bg-danger text-white';
    } else if (this.userRole === 'Operator') {
      return 'bg-warning text-dark';
    } else {
      return 'bg-info text-white';
    }
  }

  getUnreadMessagesCount(): number {
    return this.messages.filter(msg => !msg.read).length;
  }

  getUnreadNotificationsCount(): number {
    return this.notifications.filter(notif => !notif.read).length;
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
    // Implement actual language change logic here
    // You could use a translation service here
  }

  onSearch(query: string): void {
    if (query.trim()) {
      console.log('Searching for:', query);
      // Implement search logic here
      // You could navigate to search results page
      // this.router.navigate(['/search'], { queryParams: { q: query } });
    }
  }

  onSearchInput(query: string): void {
    // Handle real-time search input
    if (query.length > 2) {
      console.log('Searching...', query);
    }
  }

  markAllMessagesAsRead(): void {
    this.messages = this.messages.map(msg => ({ ...msg, read: true }));
  }

  markAllNotificationsAsRead(): void {
    this.notifications = this.notifications.map(notif => ({ ...notif, read: true }));
  }

  clearAllNotifications(): void {
    this.notifications = [];
  }

  goToProfile(): void {
    this.router.navigate(['/profile']);
  }

  goToSettings(): void {
    this.router.navigate(['/settings/account']);
  }

  goToSupport(): void {
    this.router.navigate(['/helpdesk/tickets']);
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

  logout(): void {
    this.accountService.logout();
    this.router.navigate(['/login']);
  }
}