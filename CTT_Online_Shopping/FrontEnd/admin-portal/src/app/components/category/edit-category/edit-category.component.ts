import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../../../services/category.service';
import {
  CategoryCommand,
  CategoryView,
} from '../../../models/category/category.model';
import { Gender, FilterAttributeType } from '../../../models/category/enums';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

  constructor(
    private categoryService: CategoryService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const categoryId = this.activatedRoute.snapshot.paramMap.get('id'); // Get the category ID from the route
    if (categoryId) {
      this.selectedCategoryId = categoryId;
      this.fetchCategoryDetails(categoryId);
    }
  }

  fetchCategoryDetails(categoryId: string): void {
    this.categoryService.getCategoryById(categoryId).subscribe({
      next: (category) => {
        this.category = category;
      },
      error: (err) => {
        console.error('Error fetching category details:', err);
        this.router.navigate(['/categories']); // Redirect if category not found
      },
    });
  }

  updateCategory() {
    if (this.category) {
      // Process filter attributes just like in the add method
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

      // Call the service to update the category
      this.categoryService
        .updateCategory(this.selectedCategoryId, this.category)
        .subscribe({
          next: (response) => {
            alert('Category updated successfully');
            this.router.navigate(['/category']); // Redirect after successful update
          },
          error: (error) => {
            console.error('Error updating category:', error);
          },
        });
    }
  }

  // Add Subcategory (same as Add Category Component)
  addSubCategory(): void {
    this.category?.subCategories.push({
      name: '',
      slug: '',
      filterAttributes: [],
    });
  }

  // Add Filter Attribute (same as Add Category Component)
  addFilterAttribute(subCategoryIndex: number): void {
    this.category?.subCategories[subCategoryIndex].filterAttributes.push({
      name: '',
      type: FilterAttributeType.String,
      options: [],
    });
  }
}
