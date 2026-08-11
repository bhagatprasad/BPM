import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
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
export class MyOrdersComponent implements OnInit {
  orders: any[] = [];
  expandedOrderId: string | null = null;

  selectedOrders: string[] = [];
  isAllSelected = false;

  searchText = '';

  constructor(
    private purchaseOrderService: PurchaseOrderService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private toaster: ToastrService,
  ) {}
  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    const auth = JSON.parse(localStorage.getItem('AuthenticatedUserResponse')!);
    const dealerId = auth.authenticateResponseDto.dealerId;
    //const dealerId = 'fb325f30-b6ea-4660-85ba-65a95379874a';
    this.purchaseOrderService.getOrdersByDealer(dealerId).subscribe({
      next: (res) => {
        console.log(res);
        this.orders = res;
        this.cdr.detectChanges();
        console.log('orders length is : ', this.orders.length);
        this.toaster.success('Orders loaded successfully', 'Success');
      },
      error: (err) => {
        console.log(err);
        this.toaster.error('Failed to load orders', 'Error');
      },
    });
  }
  toggleRow(poNumber: string) {
    if (this.expandedOrderId === poNumber) {
      this.expandedOrderId = null;
    } else {
      this.expandedOrderId = poNumber;
    }
  }
  viewOrder(poNumber: string) {
    console.log('View Order:', poNumber);
  }

  downloadPdf(poNumber: string) {
    console.log('Download PDF:', poNumber);
  }

  toggleAll(event: any) {
    this.isAllSelected = event.target.checked;
    if (this.isAllSelected) {
      this.selectedOrders = this.filteredOrders().map((a) => a.poNumber);
    } else {
      this.selectedOrders = [];
    }
  }

  toggleSelection(poNumber: string, event: any) {
    if (event.target.checked) {
      this.selectedOrders.push(poNumber);
    } else {
      this.selectedOrders = this.selectedOrders.filter((b) => b !== poNumber);
    }
    this.isAllSelected = this.selectedOrders.length === this.orders.length;
    console.log(this.selectedOrders);
  }

  filteredOrders() {
    if (!this.searchText) {
      return this.orders;
    }
    return this.orders.filter(
      (order) =>
        order.poNumber.toLowerCase().includes(this.searchText.toLowerCase()) ||
        order.supplierName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        order.status.toLowerCase().includes(this.searchText.toLowerCase()),
    );
  }
  addNewOrder() {
    this.toaster.info('Redirecting to Drugs Catalog...', 'Info');
    this.router.navigate(['/drugs']);
  }
  convertToIST(date: string): Date {
    const utcDate = new Date(date);
    return new Date(utcDate.getTime() + 5.5 * 60 * 60 * 1000);
  }
}
