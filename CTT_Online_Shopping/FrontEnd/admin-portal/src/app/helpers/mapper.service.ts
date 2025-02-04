import { Injectable } from '@angular/core';
import {
  CategoryCommand,
  CategoryView,
} from '../models/category/category.model';
import {
  SubCategoryCommand,
  SubCategoryViewBase,
} from '../models/category/subCategory.model';

@Injectable({
  providedIn: 'root',
})
export class Mapper {
  mapCategoryViewToCategoryCommand(
    categoryView: CategoryView
  ): CategoryCommand {
    return {
      gender: categoryView.gender,
      name: categoryView.name,
      subCategories: categoryView.subCategories.map((subCategoryView) =>
        this.mapSubCategoryViewToSubCategoryCommand(subCategoryView)
      ),
    };
  }

  // Map SubCategoryView to SubCategoryCommand
  private mapSubCategoryViewToSubCategoryCommand(
    subCategoryView: SubCategoryViewBase
  ): SubCategoryCommand {
    return {
      name: subCategoryView.name,
      slug: subCategoryView.slug,
      filterAttributes: [], // Assuming FilterAttributeView is directly usable
    };
  }
}
