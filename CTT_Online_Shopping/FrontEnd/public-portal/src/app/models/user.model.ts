export interface User {
  personalInformation: PersonalInfomation;
  addressDetail: AddressDetail;
  shippingInformation?: ShippingInformation;
}

export interface PersonalInfomation {
  name: string;
  email: string;
  phone: number;
}

export interface AddressDetail {
  streetAddress: string;
  city: string;
  zipCode: number;
  country: string;
  isShippingDifferent: boolean;
}

export interface ShippingInformation {
  streetAddress: string;
  city: string;
  zipCode: number;
  country: string;
}
