import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input'; // For input elements
import { MatAutocompleteModule } from '@angular/material/autocomplete'; //
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { CategoryService } from '../../../services/category.service';
import { ProductView } from '../../../models/product/product.model';
import { CategoryView } from '../../../models/category/category.model';
@Component({
  selector: 'app-edit-product',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
  ],
  templateUrl: './edit-product.component.html',
  styleUrl: './edit-product.component.css',
})
export class EditProductComponent {
  product!: ProductView;

  categories: CategoryView[] = []; // All categories fetched from the API
  filteredCategories: CategoryView[] = []; // Filtered categories based on user input
  filteredSubCategories: any[] = []; // Filtered subcategories based on selected category

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private categoryService: CategoryService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.loadProduct(id);
    this.loadCategories();
  }

  loadProduct(id: string | null): void {
    if (id) {
      this.productService.getProductById(id).subscribe((data) => {
        this.product = data;
        this.product.category.id = data.category.id;
        this.product.category.subCategory.id = data.category.subCategory.id;
      });
    }
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe((data) => {
      this.categories = data;
      this.filteredCategories = data;
    });
  }
  // Filter categories based on user input
  onCategoryInputChange(): void {
    console.log(this.product.category.id.toLowerCase());
    if (this.product.category.id) {
      // Filter categories locally
      this.filteredCategories = this.categories.filter((category) =>
        category.name
          .toLowerCase()
          .includes(this.product.category.id.toLowerCase())
      );
      console.log(this.filteredCategories);
    } else {
      this.filteredCategories = this.categories; // Show all categories if input is empty
    }
  }

  // Select category from the list
  onCategorySelected(event: any): void {
    this.product.category.id = event.option.value;
    this.filteredSubCategories = this.filterSubCategoriesByCategory(
      this.product.category.id
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
    if (this.product.category.subCategory.id) {
      // Filter subcategories locally based on user input
      this.filteredSubCategories = this.filteredSubCategories.filter(
        (subCategory) =>
          subCategory.name
            .toLowerCase()
            .includes(this.product.category.id.toLowerCase())
      );
    }
  }

  // Select subcategory from the list
  onSubCategorySelected(event: any): void {
    this.product.category.subCategory.id = event.option.value;
  }

  onSubmit(): void {
    this.productService.updateProduct(this.product.id, this.product).subscribe(
      (response) => {
        alert('Product Updated successfully');
        this.router.navigate(['/product']);
      },
      (error) => alert('Error adding product')
    );
  }
}
