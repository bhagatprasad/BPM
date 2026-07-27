import { Component, OnInit, AfterViewInit, OnDestroy, NgZone } from '@angular/core';
import { PurchaseOrderService } from '../../services/purchase-order.service';
import { CommonModule } from '@angular/common';
import { GridModule } from 'smart-webcomponents-angular/grid';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule, GridModule, FormsModule],
  templateUrl: './my-orders.component.html',
  styleUrl: './my-orders.component.css',
})
export class MyOrdersComponent implements OnInit, AfterViewInit, OnDestroy {
  orders: any[] = [];
  filteredOrders: any[] = [];
  searchText = '';
  selectedStatus = 'All';

  totalOrders = 0;
  draftOrders = 0;
  totalOrderValue = 0;

  selectedOrder: any = null;
  showViewModal = false;

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
    {
      label: 'PO Number',
      dataField: 'poNumber',
      align: 'center',
      cellsAlign: 'center',
      dataType: 'string',
    },

    {
      label: 'Supplier',
      dataField: 'supplierId',
      align: 'center',
      cellsAlign: 'center',
      dataType: 'string',
    },
    // { label: 'Total Amount', dataField: 'totalAmount', dataType: 'number', cellsFormat: 'c2' },//its showed dollor symbol
    {
      label: 'Total Amount',
      dataField: 'totalAmount',
      align: 'center',
      cellsAlign: 'center',
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
    {
      label: 'Order Date',
      dataField: 'orderDate',
      align: 'center',
      cellsAlign: 'center',
      cellsFormat: 'dd-MMM-yyyy',
    },
    {
      label: 'Status',
      align: 'center',
      cellsAlign: 'center',
      dataField: 'status',
      dataType: 'string',
      formatFunction(settings: any) {
        switch (settings.value) {
          case 'Draft':
            settings.template = `<span class="badge bg-warning text-dark px-3 py-2">Draft</span>`;
            break;

          case 'Submitted':
            settings.template = `<span class="badge bg-primary">Submitted</span>`;
            break;

          case 'Approved':
            settings.template = `<span class="badge bg-success">Approved</span>`;
            break;

          case 'Rejected':
            settings.template = `<span class="badge bg-danger">Rejected</span>`;
            break;

          case 'Delivered':
            settings.template = `<span class="badge bg-info text-dark">Delivered</span>`;
            break;

          case 'Cancelled':
            settings.template = `<span class="badge bg-secondary">Cancelled</span>`;
            break;

          default:
            settings.template = `<span class="badge bg-light text-dark">${settings.value}</span>`;
        }
      },
    },
    {
      label: 'Actions',
      dataField: 'id',
      width: 140,
      align: 'center',
      cellsAlign: 'center',
      allowSort: false,
      allowFilter: false,
      formatFunction(settings: any) {
        settings.template = `
      <div class="d-flex justify-content-center gap-3">

        <span
          class="view-order"
          data-id="${settings.value}"
          title="View"
          style="cursor:pointer;font-size:18px;">
          👁
        </span>

        <span
          class="print-order"
          data-id="${settings.value}"
          title="Print"
          style="cursor:pointer;font-size:18px;">
          🖨️
        </span>

      </div>
    `;
      },
    },
  ];

  constructor(
    private purchaseOrderServive: PurchaseOrderService,
    private ngZone: NgZone,
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
  viewOrder(id: string): void {
    this.purchaseOrderServive.getPurchaseOrderById(id).subscribe({
      next: (response) => {
        this.selectedOrder = response;
        this.showViewModal = true;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  private clickHandler = (event: any) => {
    const target = event.target as HTMLElement;

    if (target.classList.contains('view-order')) {
      const id = target.getAttribute('data-id');

      if (id) {
        this.ngZone.run(() => {
          this.viewOrder(id);
        });
      }
    }
  };

  ngAfterViewInit(): void {
    document.addEventListener('click', this.clickHandler);
  }

  ngOnDestroy(): void {
    document.removeEventListener('click', this.clickHandler);
  }
}
