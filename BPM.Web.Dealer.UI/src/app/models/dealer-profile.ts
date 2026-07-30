export interface UpdatedDealerRequest {

    dealershipName? : string;
    contactPerson? : string;
    email : string;
    phone : string;
    alternatePhone : string;
    addressLine1 : string;
    addressLine2 : string;
    city : string;
    state :string;
    country : string;
    postalCode : string;
    gstNumber : string;
    registrationNumber :string;
    tradeLicenseNumber : string;
    website : string;
    modifiedBy: string
}
export interface UpdatedDealerResponse {
  message?: string;
  data?: dealerDto;
}
export interface dealerDto{
    dealershipName? : string;
    contactPerson? : string;
    email : string;
    phone : string;
    alternatePhone : string;
    addressLine1 : string;
    addressLine2 : string;
    city : string;
    state :string;
    country : string;
    postalCode : string;
    gstNumber : string;
    registrationNumber :string;
    tradeLicenseNumber : string;
    website : string;
}