export interface DistributorDto {
     id: string;
  distributorCode: string;
  distributorName: string;
  registrationNumber: string;
  drugLicenseNumber: string;
  gstNumber: string;
  contactPerson: string;
  email: string;
  phone: string;
  alternatePhone: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  state: string;
  country: string;
  postalCode: string;
  website: string;
  warehouseId: string | null;
  isActive: boolean;
}
export function getDistributorDisplayName(distributor: DistributorDto): string {
  return `${distributor.distributorName} (${distributor.distributorCode})`;
}

export function getDistributorFullAddress(distributor: DistributorDto): string {
  const parts = [
    distributor.addressLine1,
    distributor.addressLine2,
    distributor.city,
    distributor.state,
    distributor.country,
    distributor.postalCode
  ].filter(part => part);
  return parts.join(', ');
}