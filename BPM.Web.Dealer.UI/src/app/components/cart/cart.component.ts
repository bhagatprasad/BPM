import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/cart-item';
import { CommonModule } from '@angular/common';
import { PurchaseOrderService } from '../../services/purchase-order.service';

@Component({
  selector: 'app-cart',
  imports: [CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  //constructor
  constructor(
    private cartService: CartService,
    private purchaseOrderService: PurchaseOrderService,
  ) {}

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
  get subtotal(): number {
    return this.cartItems.reduce((sum, item) => sum + (item.packagePrice ?? 0) * item.quantity, 0);
  }
  get gst(): number {
    return this.subtotal * 0.12;
  }
  get grandTotal(): number {
    return this.subtotal + this.gst;
  }
  placeOrder(): void {
    const auth = JSON.parse(localStorage.getItem('AuthenticatedUserResponse')!);

    const request = {
      supplierId: '7c2ef8df-8f70-49f5-aa73-32288f4abda3',
      dealerId: '6d32cd25-aa93-4625-ae69-7e8bdd9caf87',

      expectedDeliveryDate: new Date().toISOString(),

      paymentTerms: 'Net 30',
      deliveryTerms: 'Door Delivery',
      remarks: 'Order from Angular UI',
      internalNotes: 'Angular UI',

      createdBy: auth.authenticateResponseDto.userId,

      items: this.cartItems.map((item) => ({
        drugId: item.drugId,
        packagingId: item.packagingId,
        quantity: item.quantity,
        unitPrice: item.packagePrice,
        discountPercentage: 10,
        taxRate: 12,
        batchNumber: 'B001',
        expiryDate: new Date().toISOString(),
        remarks: '',
      })),
    };

    this.purchaseOrderService.createPurchaseOrder(request).subscribe({
      next: (response) => {
        console.log(response);
        alert('Purchase Order Created Successfully');
      },
      error: (error) => {
        console.error(error);
        alert('Failed to create Purchase Order');
      },
    });
  }
}
