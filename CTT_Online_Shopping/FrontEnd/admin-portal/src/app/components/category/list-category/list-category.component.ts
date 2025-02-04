import { Component } from '@angular/core';
import {
  CategoryCommand,
  CategoryView,
} from '../../../models/category/category.model';
import { CategoryService } from '../../../services/category.service';
import { CommonModule } from '@angular/common';
import { FilterAttributeType, Gender } from '../../../models/category/enums';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-category',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './list-category.component.html',
  styleUrl: './list-category.component.css',
})
export class ListCategoryComponent {
  categories: CategoryView[] = [];
  selectedCategory: CategoryView | null = null;
  genders = Object.values(Gender);
  filterTypes = Object.values(FilterAttributeType);
  categoryId: string = '';
  constructor(
    private categoryService: CategoryService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getCategories();
  }
  // Fetch categories from the backend
  getCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
      },
      error: (error) => {
        console.error('Error fetching categories', error);
      },
    });
  }

  // Show details of a specific category
  showCategoryDetails(category: CategoryView): void {
    this.selectedCategory = category;
  }

  // Hide the details view
  hideCategoryDetails(): void {
    this.selectedCategory = null;
  }

  editCategory(categoryId: string): void {
    this.router.navigate(['/edit-category', categoryId]); // Navigate to the Edit Category Component with categoryId
  }
}
