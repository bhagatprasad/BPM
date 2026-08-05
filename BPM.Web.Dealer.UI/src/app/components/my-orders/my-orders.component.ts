import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule, DatePipe, DecimalPipe, FormsModule],
  templateUrl: './my-orders.component.html',
  styleUrl: './my-orders.component.css',
})
export class MyOrdersComponent {
  //orders: any[] = [];
  expandedOrderId: string | null = null;

  selectedOrders: string[] = [];
  isAllSelected = false;

  searchText = '';

  constructor() {}

  orders = [
    {
      poNumber: 'PO-202608-0001',
      orderDate: new Date(),
      supplierName: 'Sun Pharma',
      status: 'Draft',
      totalAmount: 12500,
    },
    {
      poNumber: 'PO-202608-0002',
      orderDate: new Date(),
      supplierName: 'Cipla',
      status: 'Approved',
      totalAmount: 34250,
    },
    {
      poNumber: 'PO-202608-0003',
      orderDate: new Date(),
      supplierName: 'Dr Reddys',
      status: 'Pending',
      totalAmount: 9850,
    },
  ];

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
      this.selectedOrders = this.orders.map((a) => a.poNumber);
    } else {
      this.selectedOrders = [];
    }
    console.log(this.selectedOrders);
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
}
