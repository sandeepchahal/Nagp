import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../../../services/category.service';
import {
  CategoryCommand,
  CategoryView,
} from '../../../models/category/category.model';
import { Gender, FilterAttributeType } from '../../../models/enums';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Mapper } from '../../../helpers/mapper.service';
import {
  MenCategories,
  WomenCategories,
  MenSubategoryClothings,
  MenSubcategoryAccesstories,
  WomenSubcategoryIndianAndWesternWear,
  WomenSubcategoryWesternWear,
  WomenSubcategoryAccessories,
} from '../../../models/enums';

@Component({
  selector: 'app-edit-category',
  imports: [CommonModule, FormsModule],
  standalone: true,
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css'],
})
export class EditCategoryComponent implements OnInit {
  category: CategoryCommand | undefined;
  genders = Object.values(Gender);
  filterTypes = Object.values(FilterAttributeType);
  selectedCategoryId: string = '';
  categoryList: string[] = [];
  subCategoryList: string[] = [];

  constructor(
    private categoryService: CategoryService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private mapper: Mapper
  ) {}

  ngOnInit(): void {
    const categoryId = this.activatedRoute.snapshot.paramMap.get('id');
    if (categoryId) {
      this.selectedCategoryId = categoryId;
      this.fetchCategoryDetails(categoryId);
    }
  }

  fetchCategoryDetails(categoryId: string): void {
    this.categoryService.getCategoryById(categoryId).subscribe({
      next: (category) => {
        this.category = this.mapper.mapCategoryViewToCategoryCommand(category);
        this.updateCategories(); // Initialize category list based on gender
        this.updateSubCategories(); // Initialize subcategory list based on main category
      },
      error: (err) => {
        console.error('Error fetching category details:', err);
        this.router.navigate(['/categories']);
      },
    });
  }

  updateCategories(): void {
    if (this.category) {
      this.categoryList =
        this.category.gender === Gender.Male
          ? Object.values(MenCategories)
          : Object.values(WomenCategories);
      this.updateSubCategories();
    }
  }

  updateSubCategories(): void {
    if (this.category) {
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
  }

  updateSlug(index: number): void {
    if (this.category) {
      const subCategory = this.category.subCategories[index];
      const slug =
        `${this.category.gender.toLowerCase()}-${subCategory.name.toLowerCase()}`
          .replace(/[^a-z0-9]+/g, '-')
          .replace(/-+/g, '-')
          .replace(/^-|-$/g, '');
      subCategory.slug = slug;
    }
  }

  updateCategory() {
    if (this.category) {
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

      this.categoryService
        .updateCategory(this.selectedCategoryId, this.category)
        .subscribe({
          next: (response) => {
            alert('Category updated successfully');
            this.router.navigate(['/category']);
          },
          error: (error) => {
            console.error('Error updating category:', error);
          },
        });
    }
  }

  addSubCategory(): void {
    this.category?.subCategories.push({
      name: '',
      slug: '',
      filterAttributes: [],
    });
  }

  addFilterAttribute(subCategoryIndex: number): void {
    this.category?.subCategories[subCategoryIndex].filterAttributes.push({
      name: '',
      type: FilterAttributeType.String,
      options: [],
    });
  }
}
