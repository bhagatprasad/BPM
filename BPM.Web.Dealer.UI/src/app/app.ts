import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { SidenavComponent } from './common/sidenav';
import { CartService } from './services/cart.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SidenavComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  protected readonly title = signal('BPM.Web.Dealer.UI');
  cartCount: number = 0;
  //constructor
  constructor(
    private cartService: CartService,
    private router: Router,
  ) {}

  goToCart(): void {
    this.router.navigate(['./cart']);
  }
  ngOnInit(): void {
    this.cartService.cartCount$.subscribe((count) => {
      this.cartCount = count;
    });
  }
}
