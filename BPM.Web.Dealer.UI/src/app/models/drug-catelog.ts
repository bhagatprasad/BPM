export interface drugCatelog {
  drugId: string;
  drugCode: string;
  drugName: string;
  genericName?: string;
  brandName?: string;
  manufacturer?: string;
  category?: string;
  hsnCode?: string;
  scheduleType?: string;
  packing?: string;
  strength?: string;
  isActive: boolean;
  createdBy?: string;
  createdOn: Date;
  modifiedBy?: string;
  modifiedOn?: Date;
  imageUrl?: string;
  drugUoms?: DrugUom[];
  drugPackagings?: DrugPackaging[];
}
export interface DrugUom {
  uomId: string;
  uomCode: string;
  uomName: string;
  conversionFactor: number;
  isBaseUnit: boolean;
  isPurchaseUom: boolean;
  isSalesUom: boolean;
  isInventoryUom: boolean;
}

export interface DrugPackaging {
  packagingId: string;
  packageUomId: string;
  containsUomId: string;
  quantity: number;
  totalUnits: number;
  unitPrice: number;
  packagePrice: number;
  packageUomName: string;
  packageUomCode: string;
  containsUomName: string;
  containsUomCode: string;
  displayName: string;
}
