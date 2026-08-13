import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef, AfterViewInit, OnDestroy, Renderer2, ElementRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PurchaseOrderService } from '@app/services/purchase-order.service';
import { Router } from '@angular/router';
import { ToastrService } from '@iqx-limited/ngx-toastr';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule, DatePipe, DecimalPipe, FormsModule],
  templateUrl: './my-orders.component.html',
  styleUrl: './my-orders.component.css',
})
export class MyOrdersComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('processOrderModal') modalElementRef!: ElementRef;
  
  orders: any[] = [];
  expandedOrderId: string | null = null;
  selectedOrders: string[] = [];
  isAllSelected = false;
  searchText = '';
  
  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  
  // Modal properties
  modalOrder: any = null;
  modalAction: string = '';
  modalMessage: string = '';
  processNotes: string = '';
  isModalOpen: boolean = false;

  // Color palette for alternating rows
  rowColors = [
    '#f8f9fa',  // Light gray
    '#ffffff',  // White
    '#f0f4f8',  // Light blue
    '#fafafa',  // Off white
    '#f5f5f5',  // Light gray
    '#faf3e8',  // Cream
    '#f0f0f0',  // Gray
    '#f8f0f0',  // Light pink
    '#f0f8f0',  // Light green
    '#f0f0f8'   // Light purple
  ];

  // PO Status Constants
  PO_STATUS = {
    DRAFT: 'Draft',
    SUBMITTED: 'Submitted',
    ACCEPTED: 'Accepted',
    PENDING_VERIFICATION: 'Pending Verification',
    VERIFIED: 'Verified',
    PENDING_APPROVAL: 'Pending Approval',
    APPROVED: 'Approved',
    REJECTED: 'Rejected',
    CANCELLED: 'Cancelled',
    PROCESSING: 'Processing',
    SENT_TO_INVENTORY: 'Sent to Inventory',
    INVENTORY_CONFIRMED: 'Inventory Confirmed',
    PARTIALLY_AVAILABLE: 'Partially Available',
    OUT_OF_STOCK: 'Out of Stock',
    READY_FOR_DISPATCH: 'Ready for Dispatch',
    DISPATCHED: 'Dispatched',
    IN_TRANSIT: 'In Transit',
    PARTIALLY_DELIVERED: 'Partially Delivered',
    DELIVERED: 'Delivered',
    BILL_GENERATED: 'Bill Generated',
    PAYMENT_PENDING: 'Payment Pending',
    PARTIALLY_PAID: 'Partially Paid',
    PAID: 'Paid',
    PAYMENT_FAILED: 'Payment Failed',
    PAYMENT_OVERDUE: 'Payment Overdue',
    COMPLETED: 'Completed',
    CLOSED: 'Closed'
  };

  constructor(
    private purchaseOrderService: PurchaseOrderService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private toaster: ToastrService,
    private renderer: Renderer2
  ) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  ngAfterViewInit(): void {
    // No need for Bootstrap modal initialization
  }

  ngOnDestroy(): void {
    this.closeModal();
  }

  // ========== MODAL METHODS ==========
  showProcessModal(order: any, action: string): void {
    this.modalOrder = order;
    this.modalAction = action;
    this.processNotes = '';
    this.isModalOpen = true;
    
    const displayName = order.supplierName || 'Supplier';
    const poNumber = order.poNumber || 'N/A';
    const totalAmount = order.totalAmount || 0;
    
    if (action === 'Accept') {
      this.modalMessage = `
        Are you sure you want to accept this purchase order? <br/><br/>
        <strong>PO Number:</strong> ${poNumber}<br/>
        <strong>Supplier/Dealer:</strong> ${displayName}<br/>
        <strong>Total Amount:</strong> $${totalAmount.toFixed(2)}
      `;
    } else if (action === 'Reject') {
      this.modalMessage = `
        Are you sure you want to reject this purchase order? <br/><br/>
        <strong>PO Number:</strong> ${poNumber}<br/>
        <strong>Supplier/Dealer:</strong> ${displayName}<br/>
        <strong>Total Amount:</strong> $${totalAmount.toFixed(2)}
      `;
    }
    
    this.updateModalUI(action);
    
    // Show modal using CSS class
    const modalElement = document.getElementById('processOrderModal');
    if (modalElement) {
      this.renderer.addClass(modalElement, 'show');
      this.renderer.setStyle(modalElement, 'display', 'block');
      this.renderer.addClass(document.body, 'modal-open');
    }
    
    this.cdr.detectChanges();
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.modalOrder = null;
    this.modalAction = '';
    this.processNotes = '';
    
    // Hide modal using CSS class
    const modalElement = document.getElementById('processOrderModal');
    if (modalElement) {
      this.renderer.removeClass(modalElement, 'show');
      this.renderer.setStyle(modalElement, 'display', 'none');
      this.renderer.removeClass(document.body, 'modal-open');
    }
    
    // Remove any overlay
    const overlay = document.querySelector('.modal-overlay');
    if (overlay) {
      this.renderer.removeChild(document.body, overlay);
    }
    
    this.cdr.detectChanges();
  }

  updateModalUI(action: string): void {
    const header = document.getElementById('modalHeader');
    const iconContainer = document.getElementById('modalIconContainer');
    const icon = document.getElementById('modalIcon');
    const message = document.getElementById('processOrderMessage');
    const title = document.getElementById('processOrderModalLabel');
    const subtitle = document.getElementById('modalSubtitle');
    const confirmBtn = document.getElementById('confirmProcessOrder');
    
    if (action === 'Accept') {
      if (header) {
        header.style.borderBottomColor = '#28a745';
        header.style.backgroundColor = '#f0fff4';
      }
      if (iconContainer) {
        iconContainer.style.backgroundColor = '#d4edda';
      }
      if (icon) {
        icon.textContent = 'check_circle';
        icon.style.color = '#28a745';
      }
      if (message) {
        message.style.backgroundColor = '#d4edda';
        message.style.borderLeftColor = '#28a745';
      }
      if (title) title.textContent = 'Accept Purchase Order';
      if (subtitle) subtitle.textContent = 'Please confirm your action';
      if (confirmBtn) {
        confirmBtn.className = 'btn btn-success';
        confirmBtn.innerHTML = '<span>Accept Order</span>';
      }
    } else if (action === 'Reject') {
      if (header) {
        header.style.borderBottomColor = '#ffc107';
        header.style.backgroundColor = '#fff8e1';
      }
      if (iconContainer) {
        iconContainer.style.backgroundColor = '#fff3cd';
      }
      if (icon) {
        icon.textContent = 'warning';
        icon.style.color = '#856404';
      }
      if (message) {
        message.style.backgroundColor = '#fff3cd';
        message.style.borderLeftColor = '#ffc107';
      }
      if (title) title.textContent = 'Reject Purchase Order';
      if (subtitle) subtitle.textContent = 'Please confirm your action';
      if (confirmBtn) {
        confirmBtn.className = 'btn btn-warning';
        confirmBtn.innerHTML = '<span>Reject Order</span>';
      }
    }
  }

  confirmProcessOrder(): void {
    if (!this.modalOrder) {
      this.closeModal();
      return;
    }
    
    const orderId = this.modalOrder.id || this.modalOrder.poNumber;
    const status = this.modalAction === 'Accept' ? this.PO_STATUS.VERIFIED : this.PO_STATUS.REJECTED;
    
    const processDto = {
      PurchaseOrderId: orderId,
      Status: status,
      Notes: this.processNotes || ''
    };
    
    this.toaster.info('Processing order...', 'Info');
    
    this.purchaseOrderService.createPurchaseOrder(processDto).subscribe({
      next: (response) => {
        this.toaster.success(`Order ${this.modalOrder.poNumber} has been ${this.modalAction === 'Accept' ? 'accepted' : 'rejected'} successfully!`, 'Success');
        this.closeModal();
        this.loadOrders();
      },
      error: (err) => {
        console.error('Error processing order:', err);
        const errorMessage = err.error?.message || 'Failed to process order. Please try again.';
        this.toaster.error(errorMessage, 'Error');
        this.closeModal();
      }
    });
  }

  // ========== OTHER METHODS ==========
  loadOrders(): void {
    const auth = JSON.parse(localStorage.getItem('AuthenticatedUserResponse')!);
    const dealerId = auth.authenticateResponseDto.dealerId;
    
    this.purchaseOrderService.getOrdersByDealer(dealerId).subscribe({
      next: (res) => {
        console.log('Orders loaded:', res);
        this.orders = this.mapOrders(res);
        this.totalPages = Math.ceil(this.filteredOrders().length / this.pageSize);
        this.cdr.detectChanges();
        this.toaster.success('Orders loaded successfully', 'Success');
      },
      error: (err) => {
        console.error('Error loading orders:', err);
        this.toaster.error('Failed to load orders', 'Error');
      },
    });
  }

  mapOrders(orders: any[]): any[] {
    return orders.map(order => ({
      ...order,
      poNumber: order.poNumber || order.id || 'N/A',
      supplierName: order.supplierName || order.dealer?.dealershipName || 'Supplier',
      isDealer: !!order.dealer,
      dealer: order.dealer || null,
      subTotal: order.subTotal || 0,
      taxAmount: order.taxAmount || 0,
      totalAmount: order.totalAmount || 0,
      discountAmount: order.discountAmount || 0,
      status: order.status || 'Draft',
      orderDate: order.orderDate || new Date(),
      expectedDeliveryDate: order.expectedDeliveryDate || null,
      actualDeliveryDate: order.actualDeliveryDate || null,
      deliveryTerms: order.deliveryTerms || '',
      paymentTerms: order.paymentTerms || '',
      remarks: order.remarks || '',
      purchaseOrderItemResponse: order.purchaseOrderItemResponse || []
    }));
  }

  filteredOrders(): any[] {
    if (!this.searchText) {
      return this.orders;
    }
    const search = this.searchText.toLowerCase();
    return this.orders.filter(order =>
      order.poNumber?.toLowerCase().includes(search) ||
      order.supplierName?.toLowerCase().includes(search) ||
      order.status?.toLowerCase().includes(search) ||
      order.deliveryTerms?.toLowerCase().includes(search) ||
      order.paymentTerms?.toLowerCase().includes(search) ||
      order.remarks?.toLowerCase().includes(search)
    );
  }

  paginatedOrders(): any[] {
    const filtered = this.filteredOrders();
    this.totalPages = Math.ceil(filtered.length / this.pageSize);
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    return filtered.slice(start, end);
  }

  getStartEntry(): number {
    const total = this.filteredOrders().length;
    return total > 0 ? (this.currentPage - 1) * this.pageSize + 1 : 0;
  }

  getEndEntry(): number {
    return Math.min(this.currentPage * this.pageSize, this.filteredOrders().length);
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPages = 5;
    let start = Math.max(1, this.currentPage - Math.floor(maxPages / 2));
    let end = Math.min(this.totalPages, start + maxPages - 1);
    
    if (end - start < maxPages - 1) {
      start = Math.max(1, end - maxPages + 1);
    }
    
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.scrollToTop();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.scrollToTop();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.scrollToTop();
    }
  }

  scrollToTop(): void {
    const tableArea = document.querySelector('.default-table-area');
    if (tableArea) {
      tableArea.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  onSearch(): void {
    this.currentPage = 1;
    this.totalPages = Math.ceil(this.filteredOrders().length / this.pageSize);
  }

  toggleRow(poNumber: string): void {
    this.expandedOrderId = this.expandedOrderId === poNumber ? null : poNumber;
    setTimeout(() => this.initTooltips(), 100);
  }

  toggleAll(event: any): void {
    this.isAllSelected = event.target.checked;
    const currentPageOrders = this.paginatedOrders();
    if (this.isAllSelected) {
      currentPageOrders.forEach(order => {
        if (!this.selectedOrders.includes(order.poNumber)) {
          this.selectedOrders.push(order.poNumber);
        }
      });
    } else {
      currentPageOrders.forEach(order => {
        this.selectedOrders = this.selectedOrders.filter(id => id !== order.poNumber);
      });
    }
  }

  toggleSelection(poNumber: string, event: any): void {
    if (event.target.checked) {
      if (!this.selectedOrders.includes(poNumber)) {
        this.selectedOrders.push(poNumber);
      }
    } else {
      this.selectedOrders = this.selectedOrders.filter(id => id !== poNumber);
    }
    
    const currentPageOrders = this.paginatedOrders();
    const selectedOnPage = currentPageOrders.filter(o => this.selectedOrders.includes(o.poNumber));
    this.isAllSelected = selectedOnPage.length === currentPageOrders.length && currentPageOrders.length > 0;
  }

  getRowColor(index: number): string {
    const globalIndex = (this.currentPage - 1) * this.pageSize + index;
    const colorIndex = globalIndex % this.rowColors.length;
    return this.rowColors[colorIndex];
  }

  getStatusBadge(status: string): string {
    const statusMap: any = {
      'Draft': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
      'Submitted': { text: 'text-info', bg: 'bg-info', border: 'border-info' },
      'Pending Verification': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
      'Verified': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
      'Pending Approval': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
      'Approved': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
      'Accepted': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
      'Confirmed': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
      'Completed': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
      'Shipped': { text: 'text-primary', bg: 'bg-primary', border: 'border-primary' },
      'Processing': { text: 'text-info', bg: 'bg-info', border: 'border-info' },
      'Pending': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
      'Rejected': { text: 'text-danger', bg: 'bg-danger', border: 'border-danger' },
      'Cancelled': { text: 'text-danger', bg: 'bg-danger', border: 'border-danger' },
      'Dispatched': { text: 'text-primary', bg: 'bg-primary', border: 'border-primary' },
      'Delivered': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
      'In Transit': { text: 'text-info', bg: 'bg-info', border: 'border-info' },
      'Ready for Dispatch': { text: 'text-primary', bg: 'bg-primary', border: 'border-primary' },
      'Bill Generated': { text: 'text-info', bg: 'bg-info', border: 'border-info' },
      'Payment Pending': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
      'Partially Paid': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
      'Paid': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
      'Payment Failed': { text: 'text-danger', bg: 'bg-danger', border: 'border-danger' },
      'Payment Overdue': { text: 'text-danger', bg: 'bg-danger', border: 'border-danger' },
      'Closed': { text: 'text-secondary', bg: 'bg-secondary', border: 'border-secondary' }
    };

    const style = statusMap[status] || { text: 'text-secondary', bg: 'bg-secondary', border: 'border-secondary' };
    return `<span class="${style.text} ${style.bg} bg-opacity-10 fs-15 fw-normal d-inline-block default-badge style-two border ${style.border}">${this.escapeHtml(status)}</span>`;
  }

  formatDate(dateString: string): string {
    if (!dateString) return 'N/A';
    try {
      const date = new Date(dateString);
      return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
    } catch (e) {
      return 'N/A';
    }
  }

  convertToIST(date: string): Date {
    const utcDate = new Date(date);
    return new Date(utcDate.getTime() + 5.5 * 60 * 60 * 1000);
  }

  escapeHtml(text: string): string {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }

  initTooltips(): void {
    setTimeout(() => {
      const tooltipElements = document.querySelectorAll('[data-bs-toggle="tooltip"]');
      tooltipElements.forEach(el => {
        const tooltip = (window as any).bootstrap?.Tooltip?.getInstance(el);
        if (tooltip) {
          tooltip.dispose();
        }
        if ((window as any).bootstrap?.Tooltip) {
          new (window as any).bootstrap.Tooltip(el);
        }
      });
    }, 100);
  }

  viewOrder(poNumber: string): void {
    console.log('View Order:', poNumber);
    this.toaster.info(`Viewing order ${poNumber}`, 'Info');
  }

  downloadPdf(poNumber: string): void {
    console.log('Download PDF:', poNumber);
    this.toaster.info(`Downloading PDF for order ${poNumber}`, 'Info');
  }

  addNewOrder(): void {
    this.toaster.info('Redirecting to Drugs Catalog...', 'Info');
    this.router.navigate(['/drugs']);
  }
}