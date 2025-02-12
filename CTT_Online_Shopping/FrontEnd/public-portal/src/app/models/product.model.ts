export interface ProductView {
  id: string;
  productItemId: string;
  name: string;
  description: string;
  brand: Brand;
  price: PriceBase;
  images: ImagesBase[];
}

export interface PriceBase {
  originalPrice: number;
  discountPrice: number;
  discount: Discount;
}

export interface ImagesBase {
  url: string;
  altText: string;
  isPrimary: boolean;
  orderNumber: number;
}

export interface Brand {
  id: string;
  name: string;
}

export interface Discount {
  type: string;
  value: number;
  startDate: string; // You can use 'string' to represent ISO 8601 date format (e.g., '2025-02-09T00:00:00Z')
  endDate: string; // Same here for consistency
}

export interface ProductItemFilterContents {
  id: string;
  price: PriceBase;
  images: ImagesBase[];
}

export interface ProductFilterView {
  id: string;
  name: string;
  description: string;
  brand: Brand;
  productItems: ProductItemFilterContents[];
}
