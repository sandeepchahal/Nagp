import { CategoryViewBase } from '../category/category.model';
import { SubCategoryViewBase } from '../category/subCategory.model';

export interface ProductBase {
  name: string;
  brand: string;
  description: string;
  categoryId: string;
  subCategoryId: string;
}

export interface ProductView extends ProductBase {
  id: string;
}

export interface ProductCommand extends ProductBase {}

// category View for product

export interface ProductCategoryView extends CategoryViewBase {
  SubCategory: SubCategoryViewBase;
}

// Product Details

export interface ProductDetailView extends ProductView {}
