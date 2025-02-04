import { Component } from '@angular/core';
import { CategoryService } from '../../../services/category.service';
import { FormsModule } from '@angular/forms';

import { CategoryCommand } from '../../../models/category/category.model';
import { Gender, FilterAttributeType } from '../../../models/category/enums';
import { CommonModule } from '@angular/common';

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

  constructor(private categoryService: CategoryService) {
    console.log('in Constructor');
  }

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
    console.log('submitted the form', this.category);
    this.categoryService.addCategory(this.category).subscribe({
      next: () => alert('Category added successfully'),
      error: () => alert('Error adding category'),
    });
  }
}
