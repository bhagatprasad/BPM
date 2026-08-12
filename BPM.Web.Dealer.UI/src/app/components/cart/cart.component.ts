import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/cart-item';
import { CommonModule } from '@angular/common';
import { PurchaseOrderService } from '../../services/purchase-order.service';
import { RouterLink ,Router} from '@angular/router';
import { ToastrService } from '@iqx-limited/ngx-toastr';

@Component({
  selector: 'app-cart',
  imports: [CommonModule, RouterLink],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  isPlacingOrder = false;

  constructor(
    private cartService: CartService,
    private purchaseOrderService: PurchaseOrderService,
    private toasterService: ToastrService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  cartCount = 0;

  ngOnInit(): void {
    this.cartItems = this.cartService.getCartItems();
    this.cartService.cartCount$.subscribe(() => {
      this.cartItems = [...this.cartService.getCartItems()];

      this.cdr.detectChanges();
    });
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
    return this.subtotal + this.gst;
  }

  placeOrder(): void {
    this.isPlacingOrder = true;
    const auth = JSON.parse(localStorage.getItem('AuthenticatedUserResponse')!);
    console.log(auth.authenticateResponseDto);

    const request = {
      supplierId: '7c2ef8df-8f70-49f5-aa73-32288f4abda3',
      dealerId: auth.authenticateResponseDto.dealerId,
      expectedDeliveryDate: new Date().toISOString(),
      paymentTerms: 'Net 30',
      deliveryTerms: 'Door Delivery',
      remarks: 'Order from Angular UI',
      internalNotes: 'Angular UI',
      status:'Submitted',
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
        this.isPlacingOrder = false;
        this.toasterService.success('Purchase Order Created Successfully');
        this.cartService.clearCart();
        this.cartItems = [];
        this.cdr.detectChanges();
        this.router.navigateByUrl('/my-orders');

      },
      error: (error) => {
        console.error(error);
        this.isPlacingOrder = false;
        this.toasterService.error('Failed to create Purchase Order');
      },
    });
  }
}
