import { Component, OnInit } from '@angular/core';
import {
  ProductItemView,
  ProductVariantSizeColor,
} from '../../../models/productItem/productItem.model';
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
  styleUrls: ['./detail-product-item.component.css'],
})
export class DetailProductItemComponent implements OnInit {
  productItem!: ProductItemView;
  selectedImageIndex = 0; // Track the currently selected image
  selectedVariant: {
    label: string;
    value: string;
    price: number;
    stockQuantity: number;
  } | null = null; // Track the selected variant
  currentPrice: number | null = null; // Track the current price
  selectedColorIndex = 0; // Track the selected color index for ColorAndSize variant
  isImageZoomed = false; // Track if the main image is zoomed

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
        this.setDefaultPrice();
      },
      error: (err) => console.error('Error fetching product item:', err),
    });
  }

  setDefaultPrice(): void {
    if (!this.productItem || !this.productItem.variant) return;

    switch (this.productItem.variantType) {
      case VariantType.Size:
        this.currentPrice =
          this.productItem.variant.sizeVariant?.[0]?.price || null;
        break;
      case VariantType.Color:
        this.currentPrice =
          this.productItem.variant.colorVariant?.[0]?.price || null;
        break;
      case VariantType.ColorAndSize:
        this.currentPrice =
          this.productItem.variant.sizeColorVariant?.[0]?.sizes?.[0]?.price ||
          null;
        break;
      default:
        this.currentPrice = null;
    }
  }

  selectImage(index: number): void {
    this.selectedImageIndex = index;
  }

  getVariantType(): string {
    return this.productItem?.variantType || '';
  }

  getVariantButtons(): {
    label: string;
    value: string;
    price: number;
    stockQuantity: number;
  }[] {
    if (!this.productItem || !this.productItem.variant) return [];

    const variant = this.productItem.variant;

    switch (this.productItem.variantType) {
      case VariantType.Size:
        return (
          variant.sizeVariant?.map((v) => ({
            label: v.size || '',
            value: v.id || '',
            price: v.price || 0,
            stockQuantity: v.stockQuantity || 0,
          })) || []
        );

      case VariantType.Color:
        return (
          variant.colorVariant?.map((v) => ({
            label: v.color || '',
            value: v.id || '',
            price: v.price || 0,
            stockQuantity: v.stockQuantity || 0,
          })) || []
        );

      case VariantType.ColorAndSize:
        const selectedColorVariant =
          variant.sizeColorVariant?.[this.selectedColorIndex];
        return (
          selectedColorVariant?.sizes?.map((s) => ({
            label: s.size || '',
            value: selectedColorVariant.id || '',
            price: s.price || 0,
            stockQuantity: s.stockQuantity || 0,
          })) || []
        );

      default:
        return [];
    }
  }

  onVariantSelect(button: {
    label: string;
    value: string;
    price: number;
    stockQuantity: number;
  }): void {
    this.selectedVariant = button;
    this.currentPrice = button.price;
    console.log('Selected Variant:', button);
  }

  onColorSelect(index: number): void {
    this.selectedColorIndex = index;
    this.selectedVariant = null; // Reset selected variant when color changes
    const selectedColorVariant =
      this.productItem.variant.sizeColorVariant?.[index];
    this.currentPrice = selectedColorVariant?.sizes?.[0]?.price || null;
  }

  toggleImageZoom(): void {
    this.isImageZoomed = !this.isImageZoomed;
  }

  getAvailableColorsForSize(size: string): string[] {
    if (
      !this.productItem ||
      !this.productItem.variant ||
      this.productItem.variantType !== VariantType.ColorAndSize
    )
      return [];

    return (
      this.productItem.variant.sizeColorVariant
        ?.filter((colorVariant: ProductVariantSizeColor) =>
          colorVariant.sizes.some((s) => s.size === size && s.stockQuantity > 0)
        )
        .map((colorVariant: ProductVariantSizeColor) => colorVariant.colors) ??
      []
    );
  }
}
