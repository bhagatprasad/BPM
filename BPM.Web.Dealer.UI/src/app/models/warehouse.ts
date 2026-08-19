export interface Warehouse {
  id?: string;
  warehouseCode?: string;
  warehouseName?: string;
  distributorId?: string;
  addressLine1?: string;
  addressLine2?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  isActive?: boolean;
  createdBy?: string;
  createdOn?: string;
  modifiedBy?: string;
  modifiedOn?: string;
}

export interface WarehouseCreateDto {
  warehouseCode: string;
  warehouseName: string;
  distributorId?: string;
  addressLine1?: string;
  addressLine2?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  createdBy?: string;
}

export interface WarehouseUpdateDto {
  id: string;
  warehouseName: string;
  distributorId?: string;
  addressLine1?: string;
  addressLine2?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  isActive: boolean;
  modifiedBy?: string;
}
