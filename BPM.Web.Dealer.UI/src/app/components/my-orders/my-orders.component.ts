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
      width: 320,
      align: 'left',
      cellsAlign: 'left',

      dataType: 'string',
    },
    // { label: 'Total Amount', dataField: 'totalAmount', dataType: 'number', cellsFormat: 'c2' },//its showed dollor symbol
    {
      label: 'Total Amount',
      dataField: 'totalAmount',
      align: 'right',
      cellsAlign: 'right',
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
        console.log(this.orders);
        console.log(Array.isArray(response));
        this.totalOrders = this.orders.length;

        this.draftOrders = this.orders.filter((x) => x.status === 'Draft').length;

        this.totalOrderValue = this.orders.reduce((sum, order) => sum + order.totalAmount, 0);
      },

      error: (error) => {
        console.error(error);
      },
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
    const id = target.getAttribute('data-id');

    if (!id) return;

    if (target.classList.contains('view-order')) {
      this.ngZone.run(() => {
        this.viewOrder(id);
      });
    }

    if (target.classList.contains('pdf-order')) {
      this.ngZone.run(() => {
        this.downloadPdf(id);
      });
    }
  };
  downloadPdf(id: string): void {
    console.log('Download PDF:', id);

    // We'll connect the backend API here later.
  }

  ngAfterViewInit(): void {
    document.addEventListener('click', this.clickHandler);
  }

  ngOnDestroy(): void {
    document.removeEventListener('click', this.clickHandler);
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
}
