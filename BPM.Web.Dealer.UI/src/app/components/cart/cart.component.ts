import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/cart-item';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cart',
  imports: [CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  //constructor
  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.cartItems = this.cartService.getCartItems();
  }
  removeItem(drugId: string): void {
    this.cartService.removeFromCart(drugId);
    this.cartItems = this.cartService.getCartItems();
  }
  increase(drugId: string): void {
    this.cartService.increaseQuantity(drugId);
    this.cartItems = this.cartService.getCartItems();
  }
  decrease(drugId: string): void {
    this.cartService.decreaseQuantity(drugId);
    this.cartItems = this.cartService.getCartItems();
  }
  get totalQuantity(): number {
    return this.cartItems.reduce((sum, item) => sum + item.quantity, 0);
  }
}
