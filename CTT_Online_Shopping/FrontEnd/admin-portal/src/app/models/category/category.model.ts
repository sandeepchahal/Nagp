import { FilterAttributeType, Gender } from '../category/enums';

export interface FilterAttribute {
  name: string;
  type: string;
  options: string[];
}

export interface SubCategoryCommand {
  name: string;
  slug: string;
  filterAttributes: FilterAttribute[];
}

export interface CategoryCommand {
  gender: string;
  mainCategory: string;
  subCategories: SubCategoryCommand[];
}

// Query Models

export interface FilterAttributeView {
  id: string;
  name: string;
  type: string;
  options: string[];
}

export interface SubCategoryView {
  id: string;
  name: string;
  slug: string;
  filterAttributes: FilterAttributeView[];
}

export interface CategoryView {
  id: string;
  gender: string;
  mainCategory: string;
  subCategories: SubCategoryView[];
}
