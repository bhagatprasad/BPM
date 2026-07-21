import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterOutlet, NavigationEnd } from '@angular/router';
import { AsyncPipe, NgIf } from '@angular/common';
import { filter } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';

import { SidenavComponent } from './common/sidenav';
import { CartService } from './services/cart.service';
import { AccountService } from './services/account.service';
import { SpinnerLoadingIndicatorComponent } from './components/spinner-loading-indicator-component/spinner-loading-indicator-component.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet, 
    SidenavComponent, 
    NgIf, 
    AsyncPipe,
    SpinnerLoadingIndicatorComponent  // 👈 Add this
  ],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {

  protected readonly title = signal('BPM.Web.Dealer.UI');
  cartCount: number = 0;
  isAuthenticated$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  firstName: string = '';
  lastName: string = '';

  constructor(
    private cartService: CartService,
    private router: Router,
    public accountService: AccountService
  ) { }

  async ngOnInit(): Promise<void> {
    // Check authentication status on init
    await this.checkAuthStatus();

    // Subscribe to cart count
    this.cartService.cartCount$.subscribe((count) => {
      this.cartCount = count;
    });

    // Listen for storage changes (login/logout from other tabs)
    window.addEventListener('storage', (event) => {
      if (event.key === 'AuthenticatedUserResponse') {
        this.checkAuthStatus();
      }
    });

    // Listen to route changes to protect login route
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(async () => {
      await this.checkAuthStatus();
      
      // If user is authenticated and tries to access login page, redirect to drugs-catalog
      if (this.isAuthenticated$.value && this.router.url === '/login') {
        this.router.navigateByUrl('/drugs-catalog');
      }
    });
  }

  private async checkAuthStatus(): Promise<void> {
    const loggedData = localStorage.getItem('AuthenticatedUserResponse');
    let isAuth = false;
    
    if (loggedData) {
      try {
        const authResponse = JSON.parse(loggedData);
        if (authResponse?.jwtToken) {
          isAuth = true;
          // Set first name and last name from auth response
          this.firstName = authResponse.authenticateResponseDto.firstName || '';
          this.lastName = authResponse.authenticateResponseDto.lastName || '';
        }
      } catch (e) {
        isAuth = false;
        this.firstName = '';
        this.lastName = '';
      }
    } else {
      this.firstName = '';
      this.lastName = '';
    }
    
    this.isAuthenticated$.next(isAuth);
    
    // Redirect logic
    if (isAuth && this.router.url === '/login') {
      // If authenticated and on login page, redirect to drugs-catalog
      this.router.navigateByUrl('/drugs-catalog');
    } else if (!isAuth && this.router.url !== '/login') {
      // If not authenticated and not on login page, redirect to login
      this.router.navigateByUrl('/login');
    }
  }

  getFullName(): string {
    if (this.firstName && this.lastName) {
      return `${this.firstName} ${this.lastName}`;
    } else if (this.firstName) {
      return this.firstName;
    } else if (this.lastName) {
      return this.lastName;
    }
    return 'User';
  }

  logout() {
    localStorage.removeItem('AuthenticatedUserResponse');
    this.isAuthenticated$.next(false);
    this.firstName = '';
    this.lastName = '';
    this.router.navigate(['/login']);
  }

  goToCart(): void {
    this.router.navigate(['/cart']);
  }
}