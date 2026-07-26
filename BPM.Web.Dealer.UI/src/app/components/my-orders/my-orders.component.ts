import { Component, OnInit } from '@angular/core';
import { PurchaseOrderService } from '../../services/purchase-order.service';
import { CommonModule } from '@angular/common';
import { GridModule } from 'smart-webcomponents-angular/grid';
//import { ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule, GridModule, FormsModule],
  templateUrl: './my-orders.component.html',
  styleUrl: './my-orders.component.css',
})
export class MyOrdersComponent implements OnInit {
  orders: any[] = [];
  filteredOrders: any[] = [];
  searchText = '';
  selectedStatus = 'All';

  totalOrders = 0;
  draftOrders = 0;
  totalOrderValue = 0;

  dataSourceSettings = {
    id: 'id',
    dataFields: [
      { name: 'id', dataType: 'string' },
      { name: 'poNumber', dataType: 'string' },
      { name: 'orderDate', dataType: 'date' },
      { name: 'status', dataType: 'string' },
      { name: 'supplierId', dataType: 'string' },
      { name: 'totalAmount', dataType: 'number' },
    ],
  };

  columns = [
    { label: 'PO Number', dataField: 'poNumber', dataType: 'string' },
    { label: 'Order Date', dataField: 'orderDate', cellsFormat: 'dd-MMM-yyyy' },
    {
      label: 'Status',
      dataField: 'status',
      dataType: 'string',
      formatFunction(settings: any) {
        if (settings.value === 'Draft') {
          settings.template = `
        <span class="badge bg-warning text-dark">
          Draft
        </span>
      `;
        }
      },
    },
    { label: 'Supplier', dataField: 'supplierId', dataType: 'string' },
    // { label: 'Total Amount', dataField: 'totalAmount', dataType: 'number', cellsFormat: 'c2' },//its showed dollor symbol
    {
      label: 'Total Amount',
      dataField: 'totalAmount',
      dataType: 'number',
      formatFunction(settings: any) {
        settings.value =
          '₹' +
          Number(settings.value).toLocaleString('en-IN', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
          });
      },
    },
  ];

  constructor(
    private purchaseOrderServive: PurchaseOrderService,
    //private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    const authData = JSON.parse(localStorage.getItem('AuthenticatedUserResponse')!);

    const dealerId = authData.authenticateResponseDto.dealerId;

    this.purchaseOrderServive.getOrdersByDealer(dealerId).subscribe({
      next: (response) => {
        this.orders = response;

        // Keep this - useful for checking how many orders were returned
        console.log(this.orders);
        console.log(Array.isArray(response));

        // Apply initial filter (Status = All, Search = '')
        this.filterOrders();

        // Summary cards (always based on all orders)
        this.totalOrders = this.orders.length;

        this.draftOrders = this.orders.filter((x) => x.status === 'Draft').length;

        this.totalOrderValue = this.orders.reduce((sum, order) => sum + order.totalAmount, 0);
      },

      error: (error) => {
        console.error(error);
      },
    });
  }

  filterOrders(): void {
    const search = this.searchText.trim().toLowerCase();

    this.filteredOrders = this.orders.filter((order) => {
      const matchesSearch =
        !search ||
        order.poNumber?.toLowerCase().includes(search) ||
        order.status?.toLowerCase().includes(search) ||
        order.supplierId?.toLowerCase().includes(search);

      const matchesStatus = this.selectedStatus === 'All' || order.status === this.selectedStatus;

      return matchesSearch && matchesStatus;
    });
  }
}
