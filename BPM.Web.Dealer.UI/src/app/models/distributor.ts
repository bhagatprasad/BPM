export interface DistributorInfo {
  distributorId: string;
  distributorCode: string;
  distributorName: string;
  registrationNumber?: string;
  drugLicenseNumber?: string;
  gstNumber?: string;
  contactPerson?: string;
  email?: string;
  phone?: string;
  alternatePhone?: string;
  addressLine1?: string;
  addressLine2?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  website?: string;
  warehouseId?: string;
  isActive: boolean;
  createdBy?: string;
  createdOn: string;
  modifiedBy?: string;
  modifiedOn?: string;
}
