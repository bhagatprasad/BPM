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
    const existingItem = this.cartItems.find(
      (x) => x.drugId === item.drugId && x.packagingId === item.packagingId,
    );

    if (existingItem) {
      existingItem.quantity++;
      this.cartItems = [
        existingItem,
        ...this.cartItems.filter(
          (x) => !(x.drugId === existingItem.drugId && x.packagingId === existingItem.packagingId),
        ),
      ];
    } else {
      item.quantity = 1;
      this.cartItems.unshift(item);
    }
    localStorage.setItem(this.CART_KEY, JSON.stringify(this.cartItems));
    this.cartCountSubject.next(this.getCartCount());
  }
  //get total cart count
  getCartCount(): number {
    return this.cartItems.reduce((total, item) => total + item.quantity, 0);
  }
  removeFromCart(drugId: string): void {
    this.cartItems = this.cartItems.filter((item) => item.drugId !== drugId);
    localStorage.setItem(this.CART_KEY, JSON.stringify(this.cartItems));
    this.cartCountSubject.next(this.getCartCount());
  }
  increaseQuantity(drugId: string): void {
    const item = this.cartItems.find((a) => a.drugId === drugId);
    if (item) {
      item.quantity++;
      localStorage.setItem(this.CART_KEY, JSON.stringify(this.cartItems));
      this.cartCountSubject.next(this.getCartCount());
    }
  }
  decreaseQuantity(drugId: string): void {
    const item = this.cartItems.find((b) => b.drugId === drugId);
    if (item) {
      if (item.quantity > 1) {
        item.quantity--;
      } else {
        this.removeFromCart(drugId);
        return;
      }
      localStorage.setItem(this.CART_KEY, JSON.stringify(this.cartItems));
      this.cartCountSubject.next(this.getCartCount());
    }
  }
}
