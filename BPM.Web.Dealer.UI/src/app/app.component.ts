import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { Router, RouterOutlet, NavigationEnd } from '@angular/router';
import { AsyncPipe, NgIf } from '@angular/common';
import { filter, Subscription, BehaviorSubject } from 'rxjs';
import { CartService } from './services/cart.service';
import { AccountService } from './services/account.service';
import { SpinnerLoadingIndicatorComponent } from './components/spinner-loading-indicator-component/spinner-loading-indicator-component.component';
import { SidenavComponent } from './components/menu/sidenav.component';
import { TopnavComponent } from './components/menu/topnav.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    SidenavComponent,
    TopnavComponent,
    NgIf,
    AsyncPipe,
    SpinnerLoadingIndicatorComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private cartService = inject(CartService);
  public accountService = inject(AccountService);

  cartCount: number = 0;
  isAuthenticated$ = new BehaviorSubject<boolean>(false);
  isInitialized = false;
  
  firstName: string = '';
  lastName: string = '';
  dealerShipName: string = '';
  private routerSubscription?: Subscription;
  private isRedirecting = false;

  // Define public routes that don't require authentication
  private publicRoutes = ['/login', '/forgot-password', '/reset-password'];

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

    // Listen to route changes - optimized with debounce
    this.routerSubscription = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      if (!this.isRedirecting) {
        // Use setTimeout to avoid change detection issues
        setTimeout(() => this.checkAuthStatus(), 0);
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
        this.dealerShipName = '';
      }
    } else {
      this.firstName = '';
      this.lastName = '';
      this.dealerShipName = '';
    }

    this.isAuthenticated$.next(isAuth);
    this.isInitialized = true;
    this.handleRedirect(isAuth);
  }

  private handleRedirect(isAuth: boolean): void {
    // Prevent redirect loops
    if (this.isRedirecting) {
      return;
    }

    const currentUrl = this.router.url;
    const isPublicRoute = this.publicRoutes.some(route => currentUrl.includes(route));
    
    // Case 1: Authenticated user on login page -> redirect to drugs
    if (isAuth && currentUrl === '/login') {
      this.isRedirecting = true;
      this.router.navigateByUrl('/drugs', { replaceUrl: true }).finally(() => {
        this.isRedirecting = false;
      });
      return;
    }

    // Case 2: Unauthenticated user on protected page -> redirect to login
    if (!isAuth && !isPublicRoute && currentUrl !== '/login') {
      this.isRedirecting = true;
      this.router.navigateByUrl('/login', { replaceUrl: true }).finally(() => {
        this.isRedirecting = false;
      });
      return;
    }

    // Case 3: Unauthenticated user on login page -> allow
    // Case 4: Authenticated user on protected route -> allow
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
    return this.dealerShipName || '';
  }

  gotoprofile() {
    this.router.navigate(['/profile']);
  }

  logout() {
    localStorage.removeItem('AuthenticatedUserResponse');
    this.isAuthenticated$.next(false);
    this.firstName = '';
    this.lastName = '';
    this.dealerShipName = '';
    this.router.navigate(['/login'], { replaceUrl: true });
  }

  goToCart(): void {
    this.router.navigate(['/cart']);
  }
}