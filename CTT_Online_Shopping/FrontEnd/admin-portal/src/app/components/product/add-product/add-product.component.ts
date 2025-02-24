// add-product.component.ts
import { Component, OnInit } from '@angular/core';
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
import { EditorModule } from '@tinymce/tinymce-angular';
import {
  SubCategoryView,
  SubCategoryViewBase,
} from '../../../models/category/subCategory.model';

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    EditorModule,
  ],
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css'],
})
export class AddProductComponent implements OnInit {
  product: ProductCommand = {
    name: '',
    brandId: '',
    description: '',
    categoryId: '',
    subCategoryId: '',
  };
  categoryInput = '';
  subCategoryInput = '';
  brandInput = '';

  categories: CategoryView[] = []; // All categories fetched from the API
  filteredCategories: CategoryView[] = []; // Filtered categories based on user input
  subCategories: SubCategoryViewBase[] = []; // Filtered subcategories based on selected category
  brands: BrandView[] = [];

  // TinyMCE Configuration
  editorConfig = {
    height: 300,
    menubar: false,
    plugins: [
      'advlist autolink lists link image charmap print preview anchor',
      'searchreplace visualblocks code fullscreen',
      'insertdatetime media table paste code help wordcount',
    ],
    toolbar:
      'undo redo | formatselect | bold italic backcolor | \
      alignleft aligncenter alignright alignjustify | \
      bullist numlist outdent indent | removeformat | help', // Add your TinyMCE API key here
  };

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
      console.log('categories', this.categories);
      this.filteredCategories = categories.map((e) => ({
        ...e,
        name: `${e.name} (${e.gender})`,
      }));
    });

    this.brandService.getAll().subscribe((data) => {
      this.brands = data;
      console.log('brand', this.brands);
    });
  }
  onCategoryInputChange() {
    this.filteredCategories = this.categories.filter((category) =>
      category.name.toLowerCase().includes(this.categoryInput.toLowerCase())
    );
  }

  onCategorySelected(category: any) {
    this.product.categoryId = category.id;
    this.categoryInput = category.name;
    this.filteredCategories = [];
    this.subCategories =
      this.categories.find((category) => category.id === category.id)
        ?.subCategories || [];
  }

  onSubCategoryInputChange() {
    this.subCategories = this.subCategories.filter(
      (subCategory) =>
        subCategory.name
          .toLowerCase()
          .includes(this.subCategoryInput.toLowerCase()) &&
        subCategory.id === this.product.categoryId
    );
  }

  onSubCategorySelected(subCategory: any) {
    this.product.subCategoryId = subCategory.id;
    this.subCategoryInput = subCategory.name;
    this.subCategories = [];
  }

  onBrandInputChange() {
    this.brands = this.brands.filter((brand) =>
      brand.name.toLowerCase().includes(this.brandInput.toLowerCase())
    );
  }

  onBrandSelected(brand: any) {
    this.product.brandId = brand.id;
    this.brandInput = brand.name;
    this.brands = [];
  }

  isFormValid() {
    return (
      this.product.categoryId &&
      this.product.subCategoryId &&
      this.product.brandId &&
      this.product.name &&
      this.product.description
    );
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
      product.brandId &&
      product.description &&
      product.categoryId &&
      product.subCategoryId
    );
  }
}
