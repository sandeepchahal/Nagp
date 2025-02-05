import { Component, OnInit } from '@angular/core';
import { ProductItemService } from '../../../services/productItem.service';
import {
  ProductItemView,
  ProductVariantView,
} from './../../../models/productItem/productItem.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { VariantType } from '../../../models/enums';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-product-item',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './list-product-item.component.html',
  styleUrls: ['./list-product-item.component.css'],
})
export class ListProductItemComponent implements OnInit {
  productItems: ProductItemView[] = [];

  constructor(
    private productItemService: ProductItemService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadProductItems();
  }

  loadProductItems(): void {
    this.productItemService.getProductItems().subscribe({
      next: (data) => {
        this.productItems = data;
      },
      error: (err) => console.error('Error fetching product items:', err),
    });
  }

  getVariantCount(productItemView: ProductItemView): number {
    if (productItemView.variantType == VariantType.Color) {
      return productItemView.variant.colorVariant
        ? productItemView.variant.colorVariant?.length
        : 0;
    } else if (productItemView.variantType == VariantType.Size) {
      return productItemView.variant.sizeVariant
        ? productItemView.variant.sizeVariant?.length
        : 0;
    } else {
      return productItemView.variant.sizeColorVariant
        ? productItemView.variant.sizeColorVariant?.length
        : 0;
    }
  }
  goToDetail(id: string) {
    this.router.navigate(['/product/item/detail', id]);
  }
}
