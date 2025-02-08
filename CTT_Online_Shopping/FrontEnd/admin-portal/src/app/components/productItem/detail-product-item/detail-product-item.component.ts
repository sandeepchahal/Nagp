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
  selectedVariant!: {
    label: string;
    value: string;
    price: number;
    stockQuantity: number;
    discountedPrice: number;
  }; // Track the selected variant
  currentPrice: number | null = null; // Track the current price
  discountPrice: number = 0; // Track the discounted price
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
        console.log('Product item detail', this.productItem);
        this.setVariantImages();
        this.setDefaultPrice();
      },
      error: (err) => console.error('Error fetching product item:', err),
    });
  }

  setVariantImages() {
    if (this.productItem.variantType === VariantType.Color) {
      // Push all images from colorVariant
      if (this.productItem.variant.colorVariant) {
        this.productItem.variant.colorVariant.forEach((variant) => {
          if (variant.image) {
            this.productItem.variant.images.push(variant.image);
          }
        });
      }
    } else if (this.productItem.variantType === VariantType.ColorAndSize) {
      // Push all images from each sizeColorVariant entry
      if (this.productItem.variant.sizeColorVariant) {
        this.productItem.variant.sizeColorVariant.forEach((variant) => {
          this.productItem.variant.images.push(variant.image);
        });
      }
    }
  }

  setDefaultPrice(): void {
    if (!this.productItem || !this.productItem.variant) return;

    switch (this.productItem.variantType) {
      case VariantType.Size:
        this.selectedVariant = {
          label: this.productItem.variant.sizeVariant?.[0]?.size || '',
          value: this.productItem.variant.sizeVariant?.[0]?.id || '',
          price: this.productItem.variant.sizeVariant?.[0]?.price || 0,
          stockQuantity:
            this.productItem.variant.sizeVariant?.[0]?.stockQuantity || 0,
          discountedPrice:
            this.productItem.variant.sizeVariant?.[0]?.discountedPrice || 0,
        };
        this.currentPrice = this.selectedVariant.price;
        this.discountPrice = this.selectedVariant.discountedPrice;
        break;
      case VariantType.Color:
        this.selectedVariant = {
          label: this.productItem.variant.colorVariant?.[0]?.color || '',
          value: this.productItem.variant.colorVariant?.[0]?.id || '',
          price: this.productItem.variant.colorVariant?.[0]?.price || 0,
          stockQuantity:
            this.productItem.variant.colorVariant?.[0]?.stockQuantity || 0,
          discountedPrice:
            this.productItem.variant.colorVariant?.[0]?.discountedPrice || 0,
        };
        this.currentPrice = this.selectedVariant.price;
        this.discountPrice = this.selectedVariant.discountedPrice;
        this.selectedImageIndex = 0; // Set the main image to the first image of the selected color
        break;
      case VariantType.ColorAndSize:
        const firstColorVariant =
          this.productItem.variant.sizeColorVariant?.[0];
        const firstSizeVariant = firstColorVariant?.sizes?.[0];
        this.selectedVariant = {
          label: firstSizeVariant?.size || '',
          value: firstColorVariant?.id || '',
          price: firstSizeVariant?.price || 0,
          stockQuantity: firstSizeVariant?.stockQuantity || 0,
          discountedPrice: firstSizeVariant?.discountedPrice || 0,
        };
        this.currentPrice = this.selectedVariant.price;
        this.discountPrice = this.selectedVariant.discountedPrice;
        this.selectedImageIndex = 0; // Set the main image to the first image of the selected color
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
    discountedPrice: number;
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
            discountedPrice: v.discountedPrice || 0,
          })) || []
        );

      case VariantType.Color:
        return (
          variant.colorVariant?.map((v) => ({
            label: v.color || '',
            value: v.id || '',
            price: v.price || 0,
            stockQuantity: v.stockQuantity || 0,
            discountedPrice: v.discountedPrice || 0,
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
            discountedPrice: s.discountedPrice || 0,
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
    discountedPrice: number;
  }): void {
    this.selectedVariant = button;
    this.currentPrice = button.price;
    this.discountPrice = button.discountedPrice;
    console.log('Selected Variant:', button);

    // check the variant type
    // if color or size&color, then get the index of item and set

    if (this.productItem.variantType === VariantType.Color) {
      // Find index of the selected color variant
      const index = this.productItem.variant.colorVariant?.findIndex(
        (col) => col.id === button.value
      );
      this.selectedImageIndex = index !== undefined && index !== -1 ? index : 0;
    } else if (this.productItem.variantType === VariantType.ColorAndSize) {
      // Find index of the selected size-color variant
      const index = this.productItem.variant.sizeColorVariant?.findIndex(
        (variant) => variant.id === button.value
      );
      this.selectedImageIndex = index !== undefined && index !== -1 ? index : 0;
    }
  }

  onColorSelect(index: number): void {
    console.log(index);
    this.selectedColorIndex = index;

    // Get the selected color variant
    const selectedColorVariant =
      this.productItem.variant.sizeColorVariant?.[index];

    console.log('Selected Color Variant:', selectedColorVariant);

    // Update the main image section with the selected color's images
    if (selectedColorVariant?.image.url != null) {
      const index = this.productItem.variant.sizeColorVariant?.findIndex(
        (col) => col.id === selectedColorVariant.id
      );

      this.selectedImageIndex = index !== undefined && index !== -1 ? index : 0;
    }

    // Update the selected variant with the first size of the selected color
    if (selectedColorVariant?.sizes?.[0]) {
      const firstSizeVariant = selectedColorVariant.sizes[0];
      this.selectedVariant = {
        label: firstSizeVariant.size || '',
        value: selectedColorVariant.id || '',
        price: firstSizeVariant.price || 0,
        stockQuantity: firstSizeVariant.stockQuantity || 0,
        discountedPrice: firstSizeVariant.discountedPrice || 0,
      };
      this.currentPrice = this.selectedVariant.price;
      this.discountPrice = this.selectedVariant.discountedPrice;
    }

    console.log('Updated Selected Variant:', this.selectedVariant);
    console.log('Current Price:', this.currentPrice);
    console.log('Discount Price:', this.discountPrice);
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
        .map((colorVariant: ProductVariantSizeColor) => colorVariant.color) ??
      []
    );
  }
}
