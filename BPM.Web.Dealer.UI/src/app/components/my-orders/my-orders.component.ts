import { Component, OnInit, ChangeDetectorRef, ViewChild } from '@angular/core';
import { PurchaseOrderService } from '../../services/purchase-order.service';
import { CommonModule } from '@angular/common';
import { GridModule, GridComponent } from 'smart-webcomponents-angular/grid';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule, GridModule, FormsModule],
  templateUrl: './my-orders.component.html',
  styleUrl: './my-orders.component.css',
})
export class MyOrdersComponent implements OnInit {
  constructor(
    private purchaseOrderServive: PurchaseOrderService,
    private cdr: ChangeDetectorRef,
    private router: Router,
  ) {}

  orders: any[] = [];

  totalOrders = 0;
  draftOrders = 0;
  totalOrderValue = 0;

  selectedOrder: any = null;
  showViewModal = false;

  isLoading = false;

  @ViewChild('grid', { static: false })
  grid!: GridComponent;

  dataSourceSettings = {
    id: 'id',
    dataFields: [
      { name: 'id', dataType: 'string' },
      { name: 'poNumber', dataType: 'string' },
      { name: 'orderDate', dataType: 'date' },
      { name: 'expectedDeliveryDate', dataType: 'date' },
      { name: 'subTotal', dataType: 'number' },
      { name: 'taxAmount', dataType: 'number' },
      { name: 'totalAmount', dataType: 'number' },
      { name: 'status', dataType: 'string' },
    ],
  };

  columns = [
    {
      label: 'PO Number',
      dataField: 'poNumber',
      align: 'left',
      cellsAlign: 'center',
      dataType: 'string',
    },
    {
      label: 'Order Date',
      dataField: 'orderDate',
      align: 'left',
      cellsAlign: 'center',
      cellsFormat: 'dd-MMM-yyyy',
    },
    {
      label: 'Expected Delivery',
      width: 208,
      dataField: 'expectedDeliveryDate',
      align: 'left',
      cellsAlign: 'center',
      cellsFormat: 'dd-MMM-yyyy',
    },

    {
      label: 'Sub Total',
      dataField: 'subTotal',
      align: 'left',
      cellsAlign: 'center',
      dataType: 'number',
      formatFunction: this.currencyFormatter,
    },
    {
      label: 'Tax Amount',
      dataField: 'taxAmount',
      align: 'left',
      cellsAlign: 'center',
      dataType: 'number',
      formatFunction: this.currencyFormatter,
    },
    // { label: 'Total Amount', dataField: 'totalAmount', dataType: 'number', cellsFormat: 'c2' },//its showed dollor symbol
    {
      label: 'Total Amount',
      dataField: 'totalAmount',
      align: 'left',
      cellsAlign: 'center',
      dataType: 'number',
      formatFunction: this.currencyFormatter,
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
      align: 'center',
      cellsAlign: 'center',
      allowSort: false,
      allowFilter: false,
      formatFunction(settings: any) {
        settings.template = `
      <div class="d-flex justify-content-center gap-3">

           <i
          class="bi bi-eye-fill text-primary view-order"
          data-id="${settings.value}"
          title="View"
          style="cursor:pointer;font-size:18px;">
        </i>

        <i
          class="bi bi-file-earmark-pdf-fill text-danger pdf-order"
          data-id="${settings.value}"
          title="Download PDF"
          style="cursor:pointer;font-size:18px;">
        </i>

      </div>
    `;
      },
    },
  ];

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.isLoading = true;
    const authData = JSON.parse(localStorage.getItem('AuthenticatedUserResponse')!);

    const dealerId = authData.authenticateResponseDto.dealerId;

    this.purchaseOrderServive.getOrdersByDealer(dealerId).subscribe({
      next: (response) => {
        this.orders = response;
        console.log(this.orders);
        console.log(Array.isArray(response));
        this.totalOrders = this.orders.length;

        this.draftOrders = this.orders.filter((x) => x.status === 'Draft').length;

        this.totalOrderValue = this.orders.reduce((sum, order) => sum + order.totalAmount, 0);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);
        this.isLoading = false;
      },
    });
  }

  viewOrder(id: string): void {
    this.purchaseOrderServive.getPurchaseOrderById(id).subscribe({
      next: (response) => {
        this.selectedOrder = response;
        this.showViewModal = true;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  onCellClick(event: any): void {
    const target = event.detail.originalEvent.target as HTMLElement;

    if (!target) {
      return;
    }

    const id = event.detail.id;

    if (!id) {
      return;
    }

    if (target.classList.contains('view-order')) {
      this.viewOrder(id);
      console.log('👁️ purchase_orders id: ', id);
    }

    if (target.classList.contains('pdf-order')) {
      this.downloadPdf(id);
      console.log(id);
    }
  }

  downloadPdf(id: string): void {
    console.log('Download PDF:', id);

    // We'll connect the backend API here later.
  }
  appearancegrid = {
    showColumnHeaderLines: true,
    showColumnLines: true,
    showRowLines: true,
    alternationCount: 2,
    allowHover: true,
    showRowHeader: false,
  };

  selectiongrid = {
    enabled: true,
    checkBoxes: {
      enabled: true,
    },
  };

  filtering_grid = {
    enabled: true,
  };

  navigateToDrugCatalog(): void {
    this.router.navigate(['/drugs-catalog']);
  }

  currencyFormatter(settings: any) {
    settings.value =
      '₹' +
      Number(settings.value).toLocaleString('en-IN', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      });
  }

  exportData(): void {
    if (!this.orders.length) {
      return;
    }
    const headers = [
      'PO Number',
      'Order Date',
      'Expected Delivery',
      'Sub Total',
      'Tax Amount',
      'Total Amount',
      'Status',
    ];

    const rows = this.orders.map((order) => [
      order.poNumber,
      new Date(order.orderDate).toLocaleDateString('en-GB'),
      new Date(order.expectedDeliveryDate).toLocaleDateString('en-GB'),
      order.subTotal,
      order.taxAmount,
      order.totalAmount,
      order.status,
    ]);

    const csvContent = [headers.join(','), ...rows.map((row) => row.join('    '))].join('\n');

    const blob = new Blob([csvContent], {
      type: 'text/csv;charset=utf-8;',
    });

    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    const today = new Date().toISOString().split('T')[0];
    link.download = `PurchaseOrders_${today}.csv`;
    // link.download = `PurchaseOrders_${today}.txt`;
    link.click();

    URL.revokeObjectURL(link.href);
  }
}
