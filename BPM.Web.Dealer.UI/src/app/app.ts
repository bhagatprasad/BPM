import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AsyncPipe, NgIf } from '@angular/common';

import { SidenavComponent } from './common/sidenav';
import { CartService } from './services/cart.service';
import { AccountService } from './services/account.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, SidenavComponent, NgIf,AsyncPipe],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {


  protected readonly title = signal('BPM.Web.Dealer.UI');

  cartCount: number = 0;

isLoggedIn: any;


  isAuthenticated$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(
    private cartService: CartService,
    private router: Router,
    public accountService: AccountService
  ) {}

   async ngOnInit(): Promise<void> {
    const authStatus = await this.accountService.isAuthenticated();
    this.isAuthenticated$.next(authStatus);
    
    // Subscribe to cart count
    this.cartService.cartCount$.subscribe((count) => {
      this.cartCount = count;
    });

    if (!authStatus) {
      this.router.navigateByUrl('/login');
    }
  }

  
  logout() {
         localStorage.removeItem('AuthenticatedUserResponse');
         this.router.navigate(['/login']);;
}

  goToCart(): void {
    this.router.navigate(['/cart']);
  }
}