import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/cart-item';
import { CommonModule } from '@angular/common';
import { PurchaseOrderService } from '../../services/purchase-order.service';
import { RouterLink, Router } from '@angular/router';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cart',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  isPlacingOrder = false;
  couponCode: string = '';
  discountAmount: number = 0;
  discountPercentage: number = 0;

  // Order Details
  orderDetails = {
    expectedDeliveryDate: '',
    paymentTerms: '',
    deliveryTerms: '',
    internalNotes: '',
    remarks: ''
  };

  constructor(
    private cartService: CartService,
    private purchaseOrderService: PurchaseOrderService,
    private toasterService: ToastrService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.cartItems = this.cartService.getCartItems();
    this.cartService.cartCount$.subscribe(() => {
      this.cartItems = [...this.cartService.getCartItems()];
      this.cdr.detectChanges();
    });

    // Set default delivery date to 3 days from now
    const defaultDate = new Date();
    defaultDate.setDate(defaultDate.getDate() + 3);
    this.orderDetails.expectedDeliveryDate = this.formatDateForInput(defaultDate);
    
    // Set default payment terms
    this.orderDetails.paymentTerms = 'Net 30';
    this.orderDetails.deliveryTerms = 'Door Delivery';
  }

  get minDate(): string {
    const now = new Date();
    return this.formatDateForInput(now);
  }

  private formatDateForInput(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }

  removeItem(drugId: string): void {
    const confirmed = window.confirm(
      'Are you sure you want to remove this medicine from the cart?',
    );

    if (!confirmed) {
      return;
    }
    this.cartService.removeFromCart(drugId);
    this.cartItems = [...this.cartService.getCartItems()];
    this.toasterService.success('Medicine removed from the cart', 'Success');
  }

  increase(drugId: string): void {
    this.cartService.increaseQuantity(drugId);
    this.cartItems = [...this.cartService.getCartItems()];
    this.toasterService.success('Quantity increased', 'Success');
  }

  decrease(drugId: string): void {
    const item = this.cartItems.find((x) => x.drugId === drugId);

    if (item?.quantity === 1) {
      const confirmed = window.confirm(
        'Are you sure you want to remove this medicine from the cart?',
      );

      if (!confirmed) {
        return;
      }
    }

    this.cartService.decreaseQuantity(drugId);
    this.cartItems = [...this.cartService.getCartItems()];

    if (item && item.quantity > 1) {
      this.toasterService.info('Quantity decreased', 'Updated');
    } else {
      this.toasterService.warning('Medicine removed from the cart', 'Removed');
    }
  }

  applyCoupon(): void {
    if (!this.couponCode) {
      this.toasterService.warning('Please enter a coupon code', 'Warning');
      return;
    }

    // Example coupon logic - you can implement your own
    if (this.couponCode.toUpperCase() === 'SAVE10') {
      this.discountPercentage = 10;
      this.discountAmount = this.subtotal * 0.10;
      this.toasterService.success('Coupon applied successfully! 10% off', 'Success');
    } else if (this.couponCode.toUpperCase() === 'SAVE20') {
      this.discountPercentage = 20;
      this.discountAmount = this.subtotal * 0.20;
      this.toasterService.success('Coupon applied successfully! 20% off', 'Success');
    } else {
      this.toasterService.error('Invalid coupon code', 'Error');
      this.discountPercentage = 0;
      this.discountAmount = 0;
    }
  }

  isOrderDetailsValid(): boolean {
    return !!(
      this.orderDetails.expectedDeliveryDate &&
      this.orderDetails.paymentTerms &&
      this.cartItems.length > 0
    );
  }

  get totalQuantity(): number {
    return this.cartItems.reduce((sum, item) => sum + item.quantity, 0);
  }

  get subtotal(): number {
    return this.cartItems.reduce((sum, item) => (item.packagePrice ?? 0) * item.quantity + sum, 0);
  }

  get gst(): number {
    return this.subtotal * 0.12;
  }

  get grandTotal(): number {
    return this.subtotal + this.gst - this.discountAmount;
  }

  placeOrder(): void {
    if (!this.isOrderDetailsValid()) {
      this.toasterService.warning('Please fill in all required fields', 'Warning');
      return;
    }

    this.isPlacingOrder = true;
    const auth = JSON.parse(localStorage.getItem('AuthenticatedUserResponse')!);
    console.log('User:', auth.authenticateResponseDto);

    // Parse the expected delivery date
    const deliveryDate = new Date(this.orderDetails.expectedDeliveryDate);

    const request = {
      supplierId: '7c2ef8df-8f70-49f5-aa73-32288f4abda3',
      dealerId: auth.authenticateResponseDto.dealerId,
      expectedDeliveryDate: deliveryDate.toISOString(),
      paymentTerms: this.orderDetails.paymentTerms,
      deliveryTerms: this.orderDetails.deliveryTerms || 'Door Delivery',
      remarks: this.orderDetails.remarks || 'Order from Angular UI',
      internalNotes: this.orderDetails.internalNotes || 'Angular UI',
      status: 'Submitted',
      createdBy: auth.authenticateResponseDto.userId,
      items: this.cartItems.map((item) => ({
        drugId: item.drugId,
        packagingId: item.packagingId,
        quantity: item.quantity,
        unitPrice: item.packagePrice,
        discountPercentage: this.discountPercentage || 0,
        taxRate: 12,
        batchNumber: 'B001',
        expiryDate: new Date().toISOString(),
        remarks: '',
      })),
    };

    this.purchaseOrderService.createPurchaseOrder(request).subscribe({
      next: (response) => {
        console.log('Order created:', response);
        this.isPlacingOrder = false;
        this.toasterService.success('Purchase Order Created Successfully');
        this.cartService.clearCart();
        this.cartItems = [];
        this.cdr.detectChanges();
        this.router.navigateByUrl('/my-orders');
      },
      error: (error) => {
        console.error('Error creating order:', error);
        this.isPlacingOrder = false;
        this.toasterService.error('Failed to create Purchase Order');
      },
    });
  }
}