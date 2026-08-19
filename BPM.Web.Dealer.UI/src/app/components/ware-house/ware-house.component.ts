import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ToastrService } from '@iqx-limited/ngx-toastr';
import { WarehouseService } from '@app/services/warehouse.service';
import { Warehouse, WarehouseCreateDto, WarehouseUpdateDto } from '@app/models/warehouse';

@Component({
  selector: 'app-warehouse',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ware-house.component.html',
  styleUrl: './ware-house.component.css',
})
export class WarehouseComponent implements OnInit {
  warehouses: Warehouse[] = [];
  filteredWarehouses: Warehouse[] = [];

  distributorId = '';
  userId = '';

  searchText = '';
  isLoading = false;

  isFormVisible = false;
  isEditMode = false;

  selectedWarehouse: Warehouse | null = null;

  warehouseForm: WarehouseCreateDto = {
    warehouseCode: '',
    warehouseName: '',
    distributorId: '',
    addressLine1: '',
    addressLine2: '',
    city: '',
    state: '',
    country: '',
    postalCode: '',
    createdBy: '',
  };

  constructor(
    private warehouseService: WarehouseService,
    private toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    this.loadLoggedInUser();
    this.loadWarehouses();
  }

  private loadLoggedInUser(): void {
    const storedData = localStorage.getItem('AuthenticatedUserResponse');

    if (!storedData) {
      return;
    }

    const userData = JSON.parse(storedData);

    this.userId = userData?.authenticateResponseDto?.userId ?? '';

    this.distributorId = userData?.authenticateResponseDto?.distributorId ?? '';
  }

  loadWarehouses(): void {
    this.isLoading = true;

    if (!this.distributorId) {
      this.warehouses = [];
      this.filteredWarehouses = [];
      this.isLoading = false;
      return;
    }

    this.warehouseService.getWarehousesByDistributor(this.distributorId).subscribe({
      next: (response) => {
        this.warehouses = response ?? [];
        this.filteredWarehouses = [...this.warehouses];
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading warehouses:', error);
        this.toastr.error(error?.error?.message || 'Failed to load warehouses.');
        this.warehouses = [];
        this.filteredWarehouses = [];
        this.isLoading = false;
      },
    });
  }

  onSearch(): void {
    const search = this.searchText.trim().toLowerCase();

    if (!search) {
      this.filteredWarehouses = [...this.warehouses];
      return;
    }

    this.filteredWarehouses = this.warehouses.filter(
      (warehouse) =>
        (warehouse.warehouseCode ?? '').toLowerCase().includes(search) ||
        (warehouse.warehouseName ?? '').toLowerCase().includes(search) ||
        (warehouse.city ?? '').toLowerCase().includes(search) ||
        (warehouse.state ?? '').toLowerCase().includes(search),
    );
  }

  openCreateForm(): void {
    this.isEditMode = false;
    this.selectedWarehouse = null;

    this.warehouseForm = {
      warehouseCode: '',
      warehouseName: '',
      distributorId: this.distributorId,
      addressLine1: '',
      addressLine2: '',
      city: '',
      state: '',
      country: '',
      postalCode: '',
      createdBy: this.userId,
    };

    this.isFormVisible = true;
  }

  openEditForm(warehouse: Warehouse): void {
    this.isEditMode = true;
    this.selectedWarehouse = warehouse;

    this.warehouseForm = {
      warehouseCode: warehouse.warehouseCode ?? '',
      warehouseName: warehouse.warehouseName ?? '',
      distributorId: warehouse.distributorId ?? this.distributorId,
      addressLine1: warehouse.addressLine1 ?? '',
      addressLine2: warehouse.addressLine2 ?? '',
      city: warehouse.city ?? '',
      state: warehouse.state ?? '',
      country: warehouse.country ?? '',
      postalCode: warehouse.postalCode ?? '',
      createdBy: warehouse.createdBy ?? this.userId,
    };

    this.isFormVisible = true;
  }

  saveWarehouse(): void {
    if (!this.warehouseForm.warehouseCode.trim()) {
      this.toastr.warning('Warehouse code is required.');
      return;
    }

    if (!this.warehouseForm.warehouseName.trim()) {
      this.toastr.warning('Warehouse name is required.');
      return;
    }

    if (this.isEditMode && this.selectedWarehouse?.id) {
      const updateDto: WarehouseUpdateDto = {
        id: this.selectedWarehouse.id,
        warehouseName: this.warehouseForm.warehouseName,
        distributorId: this.warehouseForm.distributorId,
        addressLine1: this.warehouseForm.addressLine1,
        addressLine2: this.warehouseForm.addressLine2,
        city: this.warehouseForm.city,
        state: this.warehouseForm.state,
        country: this.warehouseForm.country,
        postalCode: this.warehouseForm.postalCode,
        isActive: this.selectedWarehouse.isActive ?? true,
        modifiedBy: this.userId,
      };

      this.warehouseService.updateWarehouse(this.selectedWarehouse.id, updateDto).subscribe({
        next: () => {
          this.toastr.success('Warehouse updated successfully.');
          this.closeForm();
          this.loadWarehouses();
        },
        error: (error) => {
          console.error('Error updating warehouse:', error);
          this.toastr.error(error?.error?.message || 'Failed to update warehouse.');
        },
      });

      return;
    }

    const createDto: WarehouseCreateDto = {
      ...this.warehouseForm,
      distributorId: this.distributorId,
      createdBy: this.userId,
    };

    this.warehouseService.createWarehouse(createDto).subscribe({
      next: () => {
        this.toastr.success('Warehouse created successfully.');
        this.closeForm();
        this.loadWarehouses();
      },
      error: (error) => {
        console.error('Error creating warehouse:', error);
        this.toastr.error(error?.error?.message || 'Failed to create warehouse.');
      },
    });
  }

  deleteWarehouse(warehouse: Warehouse): void {
    if (!warehouse.id) {
      return;
    }

    const confirmed = confirm(`Are you sure you want to deactivate "${warehouse.warehouseName}"?`);

    if (!confirmed) {
      return;
    }

    this.warehouseService.deleteWarehouse(warehouse.id).subscribe({
      next: () => {
        this.toastr.success('Warehouse deactivated successfully.');
        this.loadWarehouses();
      },
      error: (error) => {
        console.error('Error deleting warehouse:', error);
        this.toastr.error(error?.error?.message || 'Failed to deactivate warehouse.');
      },
    });
  }

  closeForm(): void {
    this.isFormVisible = false;
    this.selectedWarehouse = null;
  }
}
