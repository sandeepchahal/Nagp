import { BrandCommand, BrandView } from '../brand/brand.model';
import { CategoryViewBase } from '../category/category.model';
import { SubCategoryViewBase } from '../category/subCategory.model';

export interface ProductBase {
  name: string;
  brandId: string;
  description: string;
  categoryId: string;
  subCategoryId: string;
}

export interface ProductCommand extends ProductBase {}

export interface ProductViewBase {
  name: string;
  description: string;
  id: string;
  brand: BrandView;
}
export interface ProductView extends ProductViewBase {
  // discountPrice: number;
  // originalPrice: number;
  // overallRating: string;
  // numberOfReviews: number;
  category: ProductCategoryView;
  images: ImagesBase[];
}

// category View for product

export interface ProductCategoryView extends CategoryViewBase {
  subCategory: SubCategoryViewBase;
}

// Product Details

export interface ProductDetailView extends ProductView {}

export interface ImagesBase {
  url: string;
  altText: string;
  orderNumber?: number;
}
