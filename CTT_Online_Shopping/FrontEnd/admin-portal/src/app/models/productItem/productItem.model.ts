import { ImagesBase } from '../product/product.model';

export interface Discount {
  type: DiscountType;
  value: number;
}

export interface ProductFeaturesBase {
  features: { [key: string]: string };
  stockQuantity: number;
  price: number;
}

export interface ProductVariantBase {
  attributes: ProductFeaturesBase[];
  images: ImagesBase[];
  discount?: Discount;
}

export interface ProductVariant extends ProductVariantBase {
  id: string;
  isDiscountApplied: boolean;
}

export interface ProductItemBase {
  productId: string;
  name: string;
  productLevelDiscount?: Discount;
}

export interface ProductItemCommand extends ProductItemBase {
  variants: ProductVariantBase[];
}

export interface ProductItemView extends ProductItemBase, ProductVariant {
  variants: ProductVariant[];
}

// enums
export enum DiscountType {
  None = 'None',
  Fixed = 'Fixed',
  Percentage = 'Percentage',
}
