import { Component, OnInit, signal, OnDestroy } from '@angular/core';
import { Router, RouterOutlet, NavigationEnd } from '@angular/router';
import { AsyncPipe, NgIf } from '@angular/common';
import { filter, Subscription, BehaviorSubject } from 'rxjs';
import { CartService } from './services/cart.service';
import { AccountService } from './services/account.service';
import { SpinnerLoadingIndicatorComponent } from './components/spinner-loading-indicator-component/spinner-loading-indicator-component.component';
import { SidenavComponent } from './common/sidenav.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    SidenavComponent,
    NgIf,
    AsyncPipe,
    SpinnerLoadingIndicatorComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  protected readonly title = signal('BPM Medicals');
  cartCount: number = 0;
  
  // Use BehaviorSubject for template
  isAuthenticated$ = new BehaviorSubject<boolean>(false);
  
  firstName: string = '';
  lastName: string = '';
  dealerShipName: string = '';
  private routerSubscription?: Subscription;
  private isRedirecting = false;

  // Define public routes that don't require authentication
  private publicRoutes = ['/login', '/forgot-password', '/reset-password'];

  constructor(
    private cartService: CartService,
    private router: Router,
    public accountService: AccountService
  ) { }

  ngOnInit(): void {
    // Check authentication status on init
    this.checkAuthStatus();

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

    // Listen to route changes
    this.routerSubscription = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      // Only check if not already redirecting
      if (!this.isRedirecting) {
        this.checkAuthStatus();
      }
    });
  }

  ngOnDestroy(): void {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }

  private checkAuthStatus(): void {
    const loggedData = localStorage.getItem('AuthenticatedUserResponse');
    let isAuth = false;

    if (loggedData) {
      try {
        const authResponse = JSON.parse(loggedData);
        if (authResponse?.jwtToken) {
          isAuth = true;
          this.firstName = authResponse.authenticateResponseDto?.firstName || '';
          this.lastName = authResponse.authenticateResponseDto?.lastName || '';
          this.dealerShipName = authResponse.authenticateResponseDto?.dealerInfo?.dealershipName || '';
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

    // Update the observable
    this.isAuthenticated$.next(isAuth);

    // Handle redirects
    this.handleRedirect(isAuth);
  }

  private handleRedirect(isAuth: boolean): void {
    const currentUrl = this.router.url;
    const isPublicRoute = this.publicRoutes.some(route => currentUrl.includes(route));
    
    console.log('🔍 Auth Check:', { isAuth, currentUrl, isPublicRoute });

    // Prevent redirect loops
    if (this.isRedirecting) {
      return;
    }

    // Case 1: Authenticated user on login page -> redirect to drugs
    if (isAuth && currentUrl === '/login') {
      this.isRedirecting = true;
      console.log('✅ Redirecting to /drugs');
      this.router.navigateByUrl('/drugs').finally(() => {
        this.isRedirecting = false;
      });
      return;
    }

    // Case 2: Unauthenticated user on protected page -> redirect to login
    if (!isAuth && !isPublicRoute && currentUrl !== '/login') {
      this.isRedirecting = true;
      console.log('✅ Redirecting to /login');
      this.router.navigateByUrl('/login').finally(() => {
        this.isRedirecting = false;
      });
      return;
    }

    // Case 3: Unauthenticated user on public route -> allow access
    // Case 4: Authenticated user on protected route -> allow access
    console.log('✅ Allowing access to:', currentUrl);
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

  getDealerName() {
    if (this.dealerShipName) {
      return `${this.dealerShipName}`;
    }
    else {
      return ` `;
    }
  }

  gotoprofile() {
    this.router.navigate(['/profile']);
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