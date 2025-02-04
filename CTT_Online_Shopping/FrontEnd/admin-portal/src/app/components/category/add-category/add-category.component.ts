import { Component } from '@angular/core';
import { CategoryService } from '../../../services/category.service';
import { FormsModule } from '@angular/forms';

import { CategoryCommand } from '../../../models/category/category.model';
import { Gender, FilterAttributeType } from '../../../models/category/enums';
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
    mainCategory: '',
    subCategories: [],
  };

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
  addCategory(): void {
    // Ensure each filter attribute has options as a string, then split it into an array of strings
    this.category.subCategories.forEach((subCategory) => {
      subCategory.filterAttributes.forEach((attribute) => {
        // Check if options is a string (not already an array)
        console.log('att', attribute);
        console.log('type', typeof attribute.options);
        if (typeof attribute.options === 'string') {
          // Split the comma-separated string into an array
          attribute.options = (attribute.options as string)
            .split(',')
            .map((option) => option.trim())
            .filter((option) => option.length > 0); // Remove empty strings
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
