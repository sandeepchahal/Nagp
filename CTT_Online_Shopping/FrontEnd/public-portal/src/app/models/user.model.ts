export interface User {
  personalInformation: PersonalInfomation;
  addressDetail: AddressDetail;
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
}
