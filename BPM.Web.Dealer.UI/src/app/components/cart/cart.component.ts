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
  isSavingDraft = false;
  couponCode: string = '';
  discountAmount: number = 0;
  discountPercentage: number = 0;

  // Distributors
  distributors: DistributorDto[] = [];
  isLoadingDistributors = false;
  selectedDistributor: DistributorDto | null = null;
  selectedDistributorId: string = '';

  // Draft tracking
  draftPurchaseOrderId: string | null = null;

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
    private distributorService: DistributorService,
  ) { }

  ngOnInit(): void {
    this.cartItems = this.cartService.getCartItems();
    this.route.queryParams.subscribe((params) => {
      if (params['draftId']) {
        this.draftPurchaseOrderId = params['draftId'];
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
  }

  ngOnDestroy(): void {
    // Nothing to clean up
  }

  private loadDistributors(): void {
    this.isLoadingDistributors = true;

    this.distributorService.getAllDistributors().subscribe({
      next: (response: DistributorDto[]) => {
        this.distributors = response.filter(d => d.isActive === true);
        this.isLoadingDistributors = false;

        // Auto-select first distributor if available
        if (this.distributors.length > 0) {
          this.selectDistributor(this.distributors[0]);
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

  getDistributorId(distributor: DistributorDto): string {
    return (distributor as any).id || 
           (distributor as any).distributorId || 
           (distributor as any).distributor_Id ||
           (distributor as any).Id || 
           '';
  }

  isOrderDetailsValid(): boolean {
    const distributorId = this.selectedDistributorId || this.orderDetails.distributorId;
    return !!(
      this.orderDetails.expectedDeliveryDate &&
      this.orderDetails.expectedDeliveryDate.trim() !== '' &&
      this.orderDetails.paymentTerms &&
      this.orderDetails.paymentTerms.trim() !== '' &&
      distributorId &&
      distributorId.trim() !== '' &&
      distributorId !== 'undefined' &&
      distributorId !== 'null' &&
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

  private getAuthOrFail(): any | null {
    const raw = localStorage.getItem('AuthenticatedUserResponse');
    const auth = raw ? JSON.parse(raw) : null;
    if (!auth || !auth.authenticateResponseDto) {
      this.toasterService.error('User not authenticated', 'Error');
      return null;
    }
    return auth;
  }

  private buildItems() {
    return this.cartItems.map((item) => ({
      drugId: item.drugId,
      packagingId: item.packagingId,
      quantity: item.quantity,
      unitPrice: item.packagePrice,
      discountPercentage: this.discountPercentage || 0,
      taxRate: 12,
      batchNumber: 'B001',
      expiryDate: new Date().toISOString(),
      remarks: '',
    }));
  }

  /**
   * Called ONLY when the user explicitly clicks "Save as Draft".
   */
  saveDraftManually(event?: Event): void {
    // Stop event propagation
    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }

    // Prevent multiple clicks
    if (this.isSavingDraft || this.isPlacingOrder) {
      console.log('Operation already in progress');
      return;
    }

    if (this.cartItems.length === 0) {
      this.toasterService.warning('Your cart is empty', 'Warning');
      return;
    }

    const distributorId = this.selectedDistributorId || this.orderDetails.distributorId;
    const trimmedDistributorId = distributorId?.trim() || '';
    
    if (!trimmedDistributorId || trimmedDistributorId === '' || trimmedDistributorId === 'undefined' || trimmedDistributorId === 'null') {
      this.toasterService.warning('Please select a distributor before saving draft', 'Warning');
      return;
    }

    const auth = this.getAuthOrFail();
    if (!auth) return;

    const draftRequest = {
      purchaseOrderId: this.draftPurchaseOrderId,
      supplierId: '7c2ef8df-8f70-49f5-aa73-32288f4abda3',
      dealerId: auth.authenticateResponseDto.dealerId,
      distributorId: trimmedDistributorId,
      expectedDeliveryDate: this.orderDetails.expectedDeliveryDate
        ? new Date(this.orderDetails.expectedDeliveryDate).toISOString()
        : null,
      paymentTerms: this.orderDetails.paymentTerms || null,
      deliveryTerms: this.orderDetails.deliveryTerms || null,
      remarks: this.orderDetails.remarks || null,
      internalNotes: this.orderDetails.internalNotes || null,
      status: 'Draft',
      items: this.buildItems(),
    };

    console.log('Saving draft...', draftRequest);
    this.isSavingDraft = true;

    this.purchaseOrderService.savePurchaseOrderDraft(draftRequest).subscribe({
      next: (response) => {
        this.draftPurchaseOrderId = response.id;
        this.isSavingDraft = false;
        this.toasterService.success(
          `Draft saved${response.poNumber ? ' (' + response.poNumber + ')' : ''}`,
          'Success',
        );
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.isSavingDraft = false;
        const message = error?.error?.message || 'Failed to save draft';
        this.toasterService.error(message, 'Error');
        console.error('Error saving draft:', error);
      },
    });
  }

  /**
   * Called ONLY when the user explicitly clicks "Place Order".
   */
  placeOrder(event?: Event): void {
    // Stop event propagation
    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }

    // Prevent multiple clicks
    if (this.isPlacingOrder || this.isSavingDraft) {
      console.log('Operation already in progress');
      return;
    }

    if (!this.isOrderDetailsValid()) {
      this.toasterService.warning('Please fill in all required fields', 'Warning');
      return;
    }

    const distributorId = this.selectedDistributorId || this.orderDetails.distributorId;
    const trimmedDistributorId = distributorId?.trim() || '';
    
    if (!trimmedDistributorId || trimmedDistributorId === '' || trimmedDistributorId === 'undefined' || trimmedDistributorId === 'null') {
      this.toasterService.error('Please select a valid distributor', 'Error');
      return;
    }

    this.isPlacingOrder = true;

    // If a draft already exists, submit it
    if (this.draftPurchaseOrderId) {
      const request = {
        purchaseOrderId: this.draftPurchaseOrderId,
        distributorId: trimmedDistributorId,
      };

      console.log('Submitting draft...', request);
      this.toasterService.info('Submitting purchase order...', 'Please wait');

      this.purchaseOrderService.submitPurchaseOrder(request).subscribe({
        next: (response) => {
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
          this.isPlacingOrder = false;
          const message = error?.error?.message || 'Failed to submit purchase order';
          this.toasterService.error(message, 'Error');
        },
      });
      return;
    }

    // No draft yet — create a brand-new order
    const auth = this.getAuthOrFail();
    if (!auth) {
      this.isPlacingOrder = false;
      return;
    }

    const deliveryDate = new Date(this.orderDetails.expectedDeliveryDate);

    const request = {
      supplierId: '7c2ef8df-8f70-49f5-aa73-32288f4abda3',
      dealerId: auth.authenticateResponseDto.dealerId,
      distributorId: trimmedDistributorId,
      expectedDeliveryDate: deliveryDate.toISOString(),
      paymentTerms: this.orderDetails.paymentTerms,
      deliveryTerms: this.orderDetails.deliveryTerms || 'Door Delivery',
      remarks: this.orderDetails.remarks || 'Order from Angular UI',
      internalNotes: this.orderDetails.internalNotes || 'Angular UI',
      status: 'Submitted',
      createdBy: auth.authenticateResponseDto.userId,
      items: this.buildItems(),
    };

    console.log('Creating new order...', request);

    this.purchaseOrderService.createPurchaseOrder(request).subscribe({
      next: (response) => {
        this.isPlacingOrder = false;
        this.toasterService.success('Purchase Order Created Successfully');
        this.cartService.clearCart();
        this.cartItems = [];
        this.cdr.detectChanges();
        this.router.navigateByUrl('/my-orders');
      },
      error: (error) => {
        this.isPlacingOrder = false;
        const errorMessage = error?.error?.message || error?.message || 'Failed to create Purchase Order';
        this.toasterService.error(errorMessage, 'Error');
      },
    });
  }

  private loadDraft(draftId: string): void {
    this.purchaseOrderService.getPurchaseOrderById(draftId).subscribe({
      next: (draft) => {
        this.draftPurchaseOrderId = draft.id;
        this.orderDetails.expectedDeliveryDate = draft.expectedDeliveryDate
          ? this.formatDateForInput(new Date(draft.expectedDeliveryDate))
          : '';
        this.orderDetails.paymentTerms = draft.paymentTerms || '';
        this.orderDetails.deliveryTerms = draft.deliveryTerms || '';
        this.orderDetails.remarks = draft.remarks || '';
        this.orderDetails.internalNotes = draft.internalNotes || '';
        
        this.orderDetails.distributorId = draft.distributorId || '';
        this.selectedDistributorId = draft.distributorId || '';

        if (this.orderDetails.distributorId) {
          this.selectedDistributor = this.distributors.find(
            d => this.getDistributorId(d) === this.orderDetails.distributorId
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

  // Method to select a distributor from dropdown
  onDistributorChange(event: any): void {
    const value = event?.target?.value;
    console.log('Distributor selected from dropdown:', value);
    
    if (value && value !== '' && value !== 'undefined' && value !== 'null') {
      const selected = this.distributors.find(d => this.getDistributorId(d) === value);
      if (selected) {
        this.selectDistributor(selected);
      }
    } else {
      this.selectedDistributor = null;
      this.selectedDistributorId = '';
      this.orderDetails.distributorId = '';
    }
    this.cdr.detectChanges();
  }

  // Method to select a distributor from cards
  selectDistributor(distributor: DistributorDto): void {
    const id = this.getDistributorId(distributor);
    
    if (!id) {
      this.toasterService.error('Invalid distributor data', 'Error');
      return;
    }
    
    this.selectedDistributor = distributor;
    this.selectedDistributorId = id;
    this.orderDetails.distributorId = id;
    this.cdr.detectChanges();
  }
}