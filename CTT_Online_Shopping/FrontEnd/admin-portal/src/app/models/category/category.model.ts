import { SubCategoryCommand, SubCategoryViewBase } from './subCategory.model';

export interface FilterAttributeBase {
  name: string;
  type: string;
  options: string[];
}
export interface FilterAttributeView extends FilterAttributeBase {
  id: string;
}

// categories

export interface CategoryBase {
  gender: string;
  name: string;
}
export interface CategoryCommand extends CategoryBase {
  subCategories: SubCategoryCommand[];
}

export interface CategoryViewBase extends CategoryBase {
  id: string;
}
export interface CategoryView extends CategoryViewBase {
  subCategories: SubCategoryViewBase[];
}
