import { Component } from '@angular/core';
import {
  ProductCategoryView,
  ProductView,
} from '../../../models/product/product.model';
import { ProductService } from '../../../services/product.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

// Angular Material Modules
import { MatTable, MatTableModule } from '@angular/material/table'; // Required for mat-table
import { MatButtonModule } from '@angular/material/button'; // Required for buttons (if used)
import { MatIconModule } from '@angular/material/icon'; // If using icons
import { MatInputModule } from '@angular/material/input'; // If using matInput, etc.
import { MatFormFieldModule } from '@angular/material/form-field'; // For mat-form-field

@Component({
  selector: 'app-list-product',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatTable,
    MatFormFieldModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './list-product.component.html',
  styleUrl: './list-product.component.css',
})
export class ListProductComponent {
  products: ProductCategoryView[] = [];
  // Define the columns to display
  displayedColumns: string[] = [
    'id',
    'brand',
    'name',
    'category',
    'subCategory',
    'actions',
  ];

  constructor(private productService: ProductService, private router: Router) {}

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(): void {
    this.productService
      .getProducts()
      .subscribe((data: ProductCategoryView[]) => {
        this.products = data;
        console.log(this.products);
      });
  }

  viewProductDetails(id: string): void {
    this.router.navigate([`/product/details/`, id]);
  }
  editProduct(id: string): void {
    this.router.navigate(['/product/edit', id]); // Navigate to edit route
  }
  addProductItem(id: string): void {
    this.router.navigate(['/product/item/add', id]); // Navigate to edit route
  }
}
