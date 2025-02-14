import { Component } from '@angular/core';
import { CategoryService } from '../../../services/category.service';
import { FormsModule } from '@angular/forms';
import { CategoryCommand } from '../../../models/category/category.model';
import { Gender, FilterAttributeType } from '../../../models/enums';
import {
  MenCategories,
  WomenCategories,
  MenSubategoryClothings,
  MenSubcategoryAccesstories,
  WomenSubcategoryIndianAndWesternWear,
  WomenSubcategoryWesternWear,
  WomenSubcategoryAccessories,
} from '../../../models/enums';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-category',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './add-category.component.html',
  styleUrl: './add-category.component.css',
})
export class AddCategoryComponent {
  genders = Object.values(Gender);
  filterTypes = Object.values(FilterAttributeType);
  category: CategoryCommand = {
    gender: Gender.Male,
    name: '',
    subCategories: [],
  };

  categoryList: string[] = Object.values(MenCategories); // Default to men's categories
  subCategoryList: string[] = [];

  constructor(
    private categoryService: CategoryService,
    private router: Router
  ) {}

  addSubCategory(): void {
    this.category.subCategories.push({
      name: '',
      slug: '',
      filterAttributes: [],
    });
  }

  addFilterAttribute(index: number): void {
    this.category.subCategories[index].filterAttributes.push({
      name: '',
      type: FilterAttributeType.String,
      options: [],
    });
  }

  updateCategories(): void {
    this.categoryList =
      this.category.gender === Gender.Male
        ? Object.values(MenCategories)
        : Object.values(WomenCategories);
    this.category.name = ''; // Reset category name when gender changes
    // Set the first value by default if the list is not empty
    this.category.name =
      this.categoryList.length > 0 ? this.categoryList[0] : '';

    this.updateSubCategories();
  }

  updateSubCategories(): void {
    if (this.category.gender === Gender.Male) {
      if (this.category.name === MenCategories.Clothing) {
        this.subCategoryList = Object.values(MenSubategoryClothings);
      } else if (this.category.name === MenCategories.Accessories) {
        this.subCategoryList = Object.values(MenSubcategoryAccesstories);
      }
    } else if (this.category.gender === Gender.Female) {
      if (this.category.name === WomenCategories.IndianWesternWear) {
        this.subCategoryList = Object.values(
          WomenSubcategoryIndianAndWesternWear
        );
      } else if (this.category.name === WomenCategories.WesternWear) {
        this.subCategoryList = Object.values(WomenSubcategoryWesternWear);
      } else if (this.category.name === WomenCategories.Accessories) {
        this.subCategoryList = Object.values(WomenSubcategoryAccessories);
      }
    }
  }

  updateSlug(index: number): void {
    const subCategory = this.category.subCategories[index];
    const slug =
      `${this.category.gender.toLowerCase()}-${subCategory.name.toLowerCase()}`
        .replace(/[^a-z0-9]+/g, '-') // Replace non-alphanumeric characters with -
        .replace(/-+/g, '-') // Replace multiple - with single -
        .replace(/^-|-$/g, ''); // Remove leading or trailing -
    subCategory.slug = slug;
  }

  addCategory(): void {
    this.category.subCategories.forEach((subCategory) => {
      subCategory.filterAttributes.forEach((attribute) => {
        if (typeof attribute.options === 'string') {
          attribute.options = (attribute.options as string)
            .split(',')
            .map((option) => option.trim())
            .filter((option) => option.length > 0);
        }
      });
    });

    console.log('submitted the form', this.category);

    this.categoryService.addCategory(this.category).subscribe({
      next: () => {
        alert('Category added successfully');
        this.router.navigate(['/category']);
      },
      error: () => alert('Error adding category'),
    });
  }
}
