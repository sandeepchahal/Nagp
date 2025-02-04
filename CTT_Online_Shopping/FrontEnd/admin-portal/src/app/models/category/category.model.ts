import { FilterAttributeType, Gender } from '../category/enums';

export interface FilterAttribute {
  name: string;
  type: FilterAttributeType;
  options: string[];
}

export interface SubCategoryCommand {
  name: string;
  slug: string;
  filterAttributes: FilterAttribute[];
}

export interface CategoryCommand {
  gender: Gender;
  mainCategory: string;
  subCategories: SubCategoryCommand[];
}
