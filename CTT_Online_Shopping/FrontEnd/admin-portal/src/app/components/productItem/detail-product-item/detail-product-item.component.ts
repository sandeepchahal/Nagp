import { Component } from '@angular/core';
import { ProductItemView } from '../../../models/productItem/productItem.model';
import { ActivatedRoute } from '@angular/router';
import { VariantType } from '../../../models/enums';
import { ProductItemService } from '../../../services/productItem.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-detail-product-item',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './detail-product-item.component.html',
  styleUrl: './detail-product-item.component.css',
})
export class DetailProductItemComponent {
  productItem!: ProductItemView;
  selectedImageIndex = 0; // Track the currently selected image

  constructor(
    private route: ActivatedRoute,
    private productItemService: ProductItemService
  ) {}

  ngOnInit(): void {
    const productId = this.route.snapshot.paramMap.get('id');
    if (productId) {
      this.loadProductItem(productId);
    }
  }

  loadProductItem(productId: string): void {
    this.productItemService.getProductItemById(productId).subscribe({
      next: (data) => {
        this.productItem = data;
        console.log('id', productId);
        console.log('Product Item', data);
      },
      error: (err) => console.error('Error fetching product item:', err),
    });
  }

  selectImage(index: number): void {
    this.selectedImageIndex = index;
  }

  getVariantType(): string {
    return this.productItem?.variantType || '';
  }
  getVariantButtons(): { label: string; value: string }[] {
    if (!this.productItem || !this.productItem.variant) return [];

    const variant = this.productItem.variant;

    switch (this.productItem.variantType) {
      case VariantType.Size:
        return (
          variant.sizeVariant?.map((v) => ({
            label: v.size || '', // Display the size as the label
            value: v.id || '', // Use the id as the value
          })) || []
        );

      case VariantType.Color:
        return (
          variant.colorVariant?.map((v) => ({
            label: v.color || '', // Display the color as the label
            value: v.id || '', // Use the id as the value
          })) || []
        );

      case VariantType.ColorAndSize:
        return (
          variant.sizeColorVariant?.map((sc) => ({
            label: `${sc.colors} - ${sc.sizes?.[0]?.size || ''}`, // Combine color and size
            value: sc.id || '', // Use the id as the value
          })) || []
        );

      default:
        return [];
    }
  }

  onVariantSelect(button: { label: string; value: string }): void {
    // this.selectedVariant = button;
    console.log('Selected Variant:', button);
    // You can make an API call here later to fetch additional details based on the selected variant
  }
}
