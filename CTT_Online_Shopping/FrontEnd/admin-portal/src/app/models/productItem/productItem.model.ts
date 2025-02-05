import { DiscountType, VariantType } from '../enums';
import { ImagesBase, ProductView } from '../product/product.model';

export interface Discount {
  type: DiscountType;
  value: number;
}

export interface ProductItemBase {
  productId: string;
  variantType: VariantType;
}

export interface ProductVariantBase {
  images?: ImagesBase[];
  discount?: Discount;
}

export interface ProductVariant extends ProductVariantBase {
  id: string;
  isDiscountApplied: boolean;
  sizeVariant?: ProductVariantSizeBase[];
  colorVariant?: ProductVariantColorBase[];
  sizeColorVariant?: ProductVariantSizeColorBase[];
}

export interface ProductVariantSizeColorBase {
  colors: string; // for each color, we have multiple size options
  sizes: ProductVariantSizeBase[];
}

export interface ProductVariantColorBase {
  color: string;
  stockQuantity: number;
  price: number;
  discount?: Discount;
  image: ImagesBase;
}

export interface ProductVariantSizeBase {
  size: string;
  stockQuantity: number;
  price: number;
  discount?: Discount;
}

export interface ProductVariantSizeColor extends ProductVariantSizeColorBase {
  id: string;
}

export interface ProductVariantColor extends ProductVariantColorBase {
  id: string;
}

export interface ProductVariantSize extends ProductVariantSizeBase {
  id: string;
}

// post
export interface ProductVariantCommand extends ProductVariantBase {
  sizeVariant?: ProductVariantSizeBase[];
  colorVariant?: ProductVariantColorBase[];
  sizeColorVariant?: ProductVariantSizeColorBase[];
}

export interface ProductItemCommand extends ProductItemBase {
  variant: ProductVariantCommand;
}

// view
export interface ProductVariantView extends ProductVariantBase {
  id: string;
  isDiscountApplied: boolean;
  sizeVariant?: ProductVariantSize[];
  colorVariant?: ProductVariantColor[];
  sizeColorVariant?: ProductVariantSizeColor[];
}

export interface ProductItemView extends ProductItemBase {
  id: string;
  variant: ProductVariantView;
  product: ProductView;
}
