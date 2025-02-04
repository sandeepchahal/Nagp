import { FilterAttributeBase, FilterAttributeView } from './category.model';

export interface SubCategoryBase {
  name: string;
  slug: string;
}
export interface SubCategoryCommand extends SubCategoryBase {
  filterAttributes: FilterAttributeBase[];
}

export interface SubCategoryView extends SubCategoryBase {
  filterAttributes: FilterAttributeView[];
}

export interface SubCategoryViewBase extends SubCategoryBase {
  id: string;
}

export interface SubCategoryDetailView extends SubCategoryView {}
