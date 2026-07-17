import { Injectable } from '@angular/core';
import { CartItem } from '../models/cart-item';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private readonly CART_KEY = 'cart';
  private cartItems: CartItem[] = [];
  private cartCountSubject = new BehaviorSubject<number>(0);
  cartCount$ = this.cartCountSubject.asObservable();

  //constructor
  constructor() {
    const savedCart = localStorage.getItem(this.CART_KEY);

    if (savedCart) {
      this.cartItems = JSON.parse(savedCart);
    }
    this.cartCountSubject.next(this.getCartCount());
  }

  //getallcartItems
  getCartItems(): CartItem[] {
    return this.cartItems;
  }
  //add item to cart
  addToCart(item: CartItem): void {
    const existingItem = this.cartItems.find((x) => x.drugId === item.drugId);

    if (existingItem) {
      existingItem.quantity++;
    } else {
      item.quantity = 1;
      this.cartItems.push(item);
    }
    localStorage.setItem(this.CART_KEY, JSON.stringify(this.cartItems));
    this.cartCountSubject.next(this.getCartCount());
  }
  //get total cart count
  getCartCount(): number {
    return this.cartItems.reduce((total, item) => total + item.quantity, 0);
  }
}
