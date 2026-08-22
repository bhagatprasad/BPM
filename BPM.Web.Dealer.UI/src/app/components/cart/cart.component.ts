import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/cart-item';
import { CommonModule } from '@angular/common';
import { PurchaseOrderService } from '../../services/purchase-order.service';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { FormsModule } from '@angular/forms';
import { DistributorDto, getDistributorDisplayName, getDistributorFullAddress } from '@app/models/distributor-dto';
import { DistributorService } from '@app/services/distributor.service';

@Component({
  selector: 'app-cart',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent implements OnInit, OnDestroy {
  cartItems: CartItem[] = [];
  isPlacingOrder = false;
  couponCode: string = '';
  discountAmount: number = 0;
  discountPercentage: number = 0;
  
  // Distributors
  distributors: DistributorDto[] = [];
  isLoadingDistributors = false;
  selectedDistributor: DistributorDto | null = null;
  
  // Auto-save
  private autoSaveInterval: any;
  draftPurchaseOrderId: string | null = null;
  private isSavingDraft = false;

  // Order Details
  orderDetails = {
    expectedDeliveryDate: '',
    paymentTerms: '',
    deliveryTerms: '',
    internalNotes: '',
    remarks: '',
    distributorId: '',
  };

  constructor(
    private cartService: CartService,
    private purchaseOrderService: PurchaseOrderService,
    private toasterService: ToastrService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private route: ActivatedRoute,
    private distributorService:DistributorService,
  ) { }

  ngOnInit(): void {
    this.cartItems = this.cartService.getCartItems();
    this.route.queryParams.subscribe((params) => {
      if (params['draftId']) {
        this.draftPurchaseOrderId = params['draftId'];
        console.log('Resuming Draft Purchase Order:', this.draftPurchaseOrderId);
        if (this.draftPurchaseOrderId) {
          this.loadDraft(this.draftPurchaseOrderId);
        }
      }
    });

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

    this.loadDistributors();
    this.startAutoSave();
  }

  ngOnDestroy(): void {
    if (this.autoSaveInterval) {
      clearInterval(this.autoSaveInterval);
      this.autoSaveInterval = null;
    }
  }

  private loadDistributors(): void {
    this.isLoadingDistributors = true;

    this.distributorService.getAllDistributors().subscribe({
      next: (response: DistributorDto[]) => {
        this.distributors = response.filter(d => d.isActive === true);
        this.isLoadingDistributors = false;

        console.log('Distributors loaded:', this.distributors.length);

        if (this.distributors.length === 0) {
          console.warn('⚠️ No active distributors found');
          this.toasterService.warning('No distributors available', 'Warning');
        } else {
          console.log('✅ Please select a distributor from the dropdown');
        }

        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading distributors:', error);
        this.isLoadingDistributors = false;
        this.toasterService.error('Failed to load distributors', 'Error');
      }
    });
  }

  // ✅ Distributor selection is now handled by [(ngModel)] binding
  // No need for onDistributorChange method

  getDistributorDisplay(distributor: DistributorDto): string {
    return getDistributorDisplayName(distributor);
  }

  getDistributorAddress(distributor: DistributorDto): string {
    return getDistributorFullAddress(distributor);
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
    if (!confirmed) return;
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
      if (!confirmed) return;
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

    if (this.couponCode.toUpperCase() === 'SAVE10') {
      this.discountPercentage = 10;
      this.discountAmount = this.subtotal * 0.1;
      this.toasterService.success('Coupon applied successfully! 10% off', 'Success');
    } else if (this.couponCode.toUpperCase() === 'SAVE20') {
      this.discountPercentage = 20;
      this.discountAmount = this.subtotal * 0.2;
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
      this.orderDetails.distributorId &&
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
    // ✅ 1. Prevent multiple submissions
    if (this.isPlacingOrder) {
      console.warn('⚠️ Order already being placed');
      return;
    }

    // ✅ 2. Validate all required fields
    if (!this.isOrderDetailsValid()) {
      this.toasterService.warning('Please fill in all required fields', 'Warning');
      return;
    }

    // ✅ 3. Specifically validate distributor is selected
    if (!this.orderDetails.distributorId) {
      this.toasterService.error('Please select a distributor', 'Error');
      return;
    }

    // ✅ 4. Set placing order flag
    this.isPlacingOrder = true;

    console.log('📦 DRAFT ID BEFORE PLACE ORDER:', this.draftPurchaseOrderId);

    // ✅ 5. Handle draft submission
    if (this.draftPurchaseOrderId) {
      const request = {
        purchaseOrderId: this.draftPurchaseOrderId,
      };

      this.toasterService.info('Submitting purchase order...', 'Please wait');

      this.purchaseOrderService.submitPurchaseOrder(request).subscribe({
        next: (response) => {
          console.log('✅ Draft submitted successfully:', response);
          if (this.autoSaveInterval) {
            clearInterval(this.autoSaveInterval);
            this.autoSaveInterval = null;
          }
          this.isPlacingOrder = false;
          this.toasterService.success(
            `Purchase Order ${response.poNumber || ''} submitted successfully`,
            'Success',
          );
          this.cartService.clearCart();
          this.cartItems = [];
          this.cdr.detectChanges();
          this.router.navigateByUrl('/my-orders');
        },
        error: (error) => {
          console.error('❌ Error submitting draft:', error);
          this.isPlacingOrder = false;
          const message = error?.error?.message || 'Failed to submit purchase order';
          this.toasterService.error(message, 'Error');
        },
      });
      return;
    }

    // ✅ 6. Log distributor details for new order
    console.log('📦 Placing new order with distributor:', {
      distributorId: this.orderDetails.distributorId,
      distributorName: this.selectedDistributor?.distributorName,
      distributorCode: this.selectedDistributor?.distributorCode
    });

    // ✅ 7. Get authenticated user
    const auth = JSON.parse(localStorage.getItem('AuthenticatedUserResponse')!);

    // ✅ 8. Validate auth exists
    if (!auth || !auth.authenticateResponseDto) {
      this.toasterService.error('User not authenticated', 'Error');
      this.isPlacingOrder = false;
      return;
    }

    const deliveryDate = new Date(this.orderDetails.expectedDeliveryDate);

    // ✅ 9. Build request payload
    const request = {
      supplierId: '7c2ef8df-8f70-49f5-aa73-32288f4abda3',
      dealerId: auth.authenticateResponseDto.dealerId,
      distributorId: this.orderDetails.distributorId,
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

    // ✅ 10. Log the final payload for debugging
    console.log('📤 Final order payload:', request);

    // ✅ 11. Create purchase order
    this.purchaseOrderService.createPurchaseOrder(request).subscribe({
      next: (response) => {
        console.log('✅ Order created successfully:', response);
        if (this.autoSaveInterval) {
          clearInterval(this.autoSaveInterval);
          this.autoSaveInterval = null;
        }
        this.isPlacingOrder = false;
        this.toasterService.success('Purchase Order Created Successfully');
        this.cartService.clearCart();
        this.cartItems = [];
        this.cdr.detectChanges();
        this.router.navigateByUrl('/my-orders');
      },
      error: (error) => {
        console.error('❌ Error creating order:', error);
        this.isPlacingOrder = false;
        const errorMessage = error?.error?.message || error?.message || 'Failed to create Purchase Order';
        this.toasterService.error(errorMessage, 'Error');
      },
    });
  }

  private startAutoSave(): void {
    this.autoSaveInterval = setInterval(() => {
      this.saveDraft();
    }, 30000);
  }

  private saveDraft(): void {
    if (this.isSavingDraft || this.cartItems.length === 0) {
      return;
    }

    const auth = JSON.parse(localStorage.getItem('AuthenticatedUserResponse')!);

    const draftRequest = {
      purchaseOrderId: this.draftPurchaseOrderId,
      supplierId: '7c2ef8df-8f70-49f5-aa73-32288f4abda3',
      dealerId: auth.authenticateResponseDto.dealerId,
      distributorId: this.orderDetails.distributorId,
      expectedDeliveryDate: this.orderDetails.expectedDeliveryDate
        ? new Date(this.orderDetails.expectedDeliveryDate).toISOString()
        : null,
      paymentTerms: this.orderDetails.paymentTerms || null,
      deliveryTerms: this.orderDetails.deliveryTerms || null,
      remarks: this.orderDetails.remarks || null,
      internalNotes: this.orderDetails.internalNotes || null,
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

    this.isSavingDraft = true;

    this.purchaseOrderService.savePurchaseOrderDraft(draftRequest).subscribe({
      next: (response) => {
        this.draftPurchaseOrderId = response.id;
        this.isSavingDraft = false;
        console.log('Draft auto-saved successfully:', response.poNumber);
      },
      error: (error) => {
        this.isSavingDraft = false;
        console.error('Error auto-saving draft:', error);
      },
    });
  }

  private loadDraft(draftId: string): void {
    this.purchaseOrderService.getPurchaseOrderById(draftId).subscribe({
      next: (draft) => {
        console.log('Draft loaded:', draft);
        this.draftPurchaseOrderId = draft.id;
        this.orderDetails.expectedDeliveryDate = draft.expectedDeliveryDate
          ? this.formatDateForInput(new Date(draft.expectedDeliveryDate))
          : '';
        this.orderDetails.paymentTerms = draft.paymentTerms || '';
        this.orderDetails.deliveryTerms = draft.deliveryTerms || '';
        this.orderDetails.remarks = draft.remarks || '';
        this.orderDetails.internalNotes = draft.internalNotes || '';
        this.orderDetails.distributorId = draft.distributorId || '';

        // ✅ Update selected distributor when loading draft
        if (this.orderDetails.distributorId) {
          this.selectedDistributor = this.distributors.find(
            d => d.id === this.orderDetails.distributorId
          ) || null;
        }

        this.discountAmount = draft.discountAmount || 0;
        this.cartItems = [...this.cartService.getCartItems()];
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading draft:', error);
        this.toasterService.error('Failed to load draft purchase order', 'Error');
      },
    });
  }
}