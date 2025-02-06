// add-product.component.ts
import { Component } from '@angular/core';
import { ProductCommand } from '../../../models/product/product.model';
import { ProductService } from '../../../services/product.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input'; // For input elements
import {
  MatAutocompleteModule,
  MatAutocompleteSelectedEvent,
} from '@angular/material/autocomplete'; // For autocomplete
import { CategoryService } from '../../../services/category.service';
import { CategoryView } from '../../../models/category/category.model';
import { BrandCommand, BrandView } from '../../../models/brand/brand.model';
import { Router } from '@angular/router';
import { BrandService } from '../../../services/brand.service';

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
  ],
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css'],
})
export class AddProductComponent {
  product: ProductCommand = {
    name: '',
    brand: { name: '', id: '' },
    description: '',
    categoryId: '',
    subCategoryId: '',
  };

  categories: CategoryView[] = []; // All categories fetched from the API
  filteredCategories: CategoryView[] = []; // Filtered categories based on user input
  filteredSubCategories: any[] = []; // Filtered subcategories based on selected category
  brands: BrandView[] = [];
  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private brandService: BrandService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Fetch all categories when the component initializes
    this.categoryService.getCategories().subscribe((categories) => {
      this.categories = categories;
      this.filteredCategories = categories;
    });

    this.brandService.getAll().subscribe((data) => {
      this.brands = data;
    });
  }

  // Filter categories based on user input
  onCategoryInputChange(): void {
    console.log(this.product.categoryId.toLowerCase());
    if (this.product.categoryId) {
      // Filter categories locally
      this.filteredCategories = this.categories.filter((category) =>
        category.name
          .toLowerCase()
          .includes(this.product.categoryId.toLowerCase())
      );
      console.log(this.filteredCategories);
    } else {
      this.filteredCategories = this.categories; // Show all categories if input is empty
    }
  }

  // Select category from the list
  onCategorySelected(event: any): void {
    this.product.categoryId = event.option.value;
    this.filteredSubCategories = this.filterSubCategoriesByCategory(
      this.product.categoryId
    ); // Filter subcategories based on selected category
  }
  // Filter subcategories based on selected category
  filterSubCategoriesByCategory(categoryId: string): any[] {
    // Assuming subcategories data is available for each category, you may have to modify this based on your data structure
    return (
      this.categories.find((category) => category.id === categoryId)
        ?.subCategories || []
    );
  }

  // Filter subcategories based on user input
  onSubCategoryInputChange(): void {
    if (this.product.subCategoryId) {
      // Filter subcategories locally based on user input
      this.filteredSubCategories = this.filteredSubCategories.filter(
        (subCategory) =>
          subCategory.name
            .toLowerCase()
            .includes(this.product.subCategoryId.toLowerCase())
      );
    }
  }

  // Select subcategory from the list
  onSubCategorySelected(event: any): void {
    this.product.subCategoryId = event.option.value;
  }

  onBrandInputChange() {
    this.brands = this.brands.filter((brand) =>
      brand.name
        .toLowerCase()
        .includes(this.product.brand.id?.toString().toLowerCase() || '')
    );
  }

  onBrandSelected(event: MatAutocompleteSelectedEvent) {
    const selectedBrand = this.brands.find(
      (brand) => brand.id === event.option.value
    );
    if (selectedBrand) {
      this.product.brand.id = selectedBrand.id;
      this.product.brand.name = selectedBrand.name;
    }
  }

  onSubmit() {
    if (this.isValidProduct(this.product)) {
      this.productService.addProduct(this.product).subscribe(
        (response) => {
          alert('Product added successfully');
          this.router.navigate(['/product']);
        },
        (error) => alert('Error adding product')
      );
    } else {
      alert('All fields are required');
    }
  }

  private isValidProduct(product: ProductCommand): boolean {
    return !!(
      product.name &&
      product.brand &&
      product.description &&
      product.categoryId &&
      product.subCategoryId
    );
  }
}
