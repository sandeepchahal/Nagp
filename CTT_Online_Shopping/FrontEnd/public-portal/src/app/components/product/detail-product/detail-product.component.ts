import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import {
  ProductItemView,
  ProductVariantSizeColor,
} from '../../../models/productItem.model';
import { ActivatedRoute } from '@angular/router';
import { VariantType } from '../../../models/enums';
import { ProductItemService } from '../../../services/productItem.service';
import { CartService } from '../../../services/cart.service';
import { CartItem } from '../../../models/cart.model';

@Component({
  selector: 'app-detail-product',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './detail-product.component.html',
  styleUrl: './detail-product.component.css',
})
export class DetailProductComponent {
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
  selectedColorId: string = '';
  cartItem: CartItem = {
    brand: '',
    imgUrl: '',
    name: '',
    discountedPrice: 0,
    price: 0,
    variantType: '',
    colorId: '',
    sizeId: '',
    sizeLabel: '',
  };

  constructor(
    private route: ActivatedRoute,
    private productItemService: ProductItemService,
    private cartService: CartService
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
        console.log('Product item detail', data);
        this.cartItem.brand = this.productItem.product.brand.name;
        this.cartItem.name = this.productItem.product.name;
        this.cartItem.variantType = this.productItem.variantType;
        this.setVariantImages();
        this.setDefaultPrice();
      },
      error: (err) => console.error('Error fetching product item:', err),
    });
  }

  setVariantImages() {
    if (this.productItem.variantType === VariantType.Color) {
      // Push all images from colorVariant
      if (this.productItem.variants.colorVariant) {
        this.productItem.variants.colorVariant.forEach((variant) => {
          if (variant.image) {
            this.productItem.variants.images.push(variant.image);
          }
        });
      }
    } else if (this.productItem.variantType === VariantType.ColorAndSize) {
      // Push all images from each sizeColorVariant entry
      if (this.productItem.variants.sizeColorVariant) {
        this.productItem.variants.sizeColorVariant.forEach((variant) => {
          this.productItem.variants.images.push(variant.image);
        });
      }
    }
  }

  setDefaultPrice(): void {
    if (!this.productItem || !this.productItem.variants) return;

    switch (this.productItem.variantType) {
      case VariantType.Size:
        this.selectedVariant = {
          label: this.productItem.variants.sizeVariant?.[0]?.size || '',
          value: this.productItem.variants.sizeVariant?.[0]?.id || '',
          price: this.productItem.variants.sizeVariant?.[0]?.price || 0,
          stockQuantity:
            this.productItem.variants.sizeVariant?.[0]?.stockQuantity || 0,
          discountedPrice:
            this.productItem.variants.sizeVariant?.[0]?.discountedPrice || 0,
        };
        this.currentPrice = this.selectedVariant.price;
        this.discountPrice = this.selectedVariant.discountedPrice;
        this.cartItem.price = this.currentPrice;
        this.cartItem.discountedPrice = this.discountPrice;
        this.cartItem.sizeId = this.productItem.variants.sizeVariant?.[0]?.id;
        this.cartItem.sizeLabel =
          this.productItem.variants.sizeVariant?.[0]?.size ?? '';

        this.cartItem.imgUrl = this.productItem.variants.images[0].url;

        break;
      case VariantType.Color:
        this.selectedVariant = {
          label: this.productItem.variants.colorVariant?.[0]?.color || '',
          value: this.productItem.variants.colorVariant?.[0]?.id || '',
          price: this.productItem.variants.colorVariant?.[0]?.price || 0,
          stockQuantity:
            this.productItem.variants.colorVariant?.[0]?.stockQuantity || 0,
          discountedPrice:
            this.productItem.variants.colorVariant?.[0]?.discountedPrice || 0,
        };
        this.currentPrice = this.selectedVariant.price;
        this.cartItem.price = this.currentPrice;
        this.cartItem.discountedPrice = this.selectedVariant.discountedPrice;
        this.cartItem.imgUrl =
          this.productItem.variants.colorVariant![0].image.url;
        this.cartItem.colorId = this.productItem.variants.colorVariant?.[0]?.id;
        this.discountPrice = this.selectedVariant.discountedPrice;
        this.selectedImageIndex = 0; // Set the main image to the first image of the selected color
        break;
      case VariantType.ColorAndSize:
        const firstColorVariant =
          this.productItem.variants.sizeColorVariant?.[0];

        const firstSizeVariant = firstColorVariant?.sizes?.[0];
        this.selectedVariant = {
          label: firstSizeVariant?.size || '',
          value: firstSizeVariant?.id || '',
          price: firstSizeVariant?.price || 0,
          stockQuantity: firstSizeVariant?.stockQuantity || 0,
          discountedPrice: firstSizeVariant?.discountedPrice || 0,
        };
        this.currentPrice = this.selectedVariant.price;
        this.cartItem.sizeId = this.selectedVariant.value;
        this.cartItem.sizeLabel = firstSizeVariant?.size ?? '';
        this.cartItem.colorId = firstColorVariant?.id;
        this.cartItem.price = this.currentPrice;
        this.cartItem.discountedPrice = this.selectedVariant.discountedPrice;
        this.cartItem.imgUrl = firstColorVariant?.image.url ?? '';
        this.productItem.variants.sizeColorVariant;
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
    if (!this.productItem || !this.productItem.variants) return [];

    const variant = this.productItem.variants;
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
            value: s.id || '',
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
    this.cartItem.sizeId = button.value;
    this.cartItem.sizeLabel = button.label;
    this.cartItem.discountedPrice = button.discountedPrice;
    this.cartItem.price = button.price;

    // check the variant type
    // if color or size&color, then get the index of item and set

    if (this.productItem.variantType === VariantType.Color) {
      // Find index of the selected color variant
      const index = this.productItem.variants.colorVariant?.findIndex(
        (col) => col.id === button.value
      );
      this.selectedImageIndex = index !== undefined && index !== -1 ? index : 0;
      this.cartItem.sizeId = button.value;
      this.cartItem.sizeLabel = button.label;
      this.cartItem.imgUrl =
        this.productItem.variants.colorVariant?.[index ?? 0].image.url ?? '';
    } else if (this.productItem.variantType === VariantType.ColorAndSize) {
      // Find index of the selected size-color variant
      const index = this.productItem.variants.sizeColorVariant?.findIndex(
        (variant) => variant.id === button.value
      );
      this.cartItem.imgUrl =
        this.productItem.variants.colorVariant?.[index ?? 0].image.url ?? '';
      //this.selectedImageIndex = index !== undefined && index !== -1 ? index : 0;
    } else {
      this.cartItem.sizeId = button.value;
      this.cartItem.sizeLabel = button.label;
    }
  }

  onColorSelect(index: number): void {
    console.log(index);
    this.selectedColorIndex = index;

    // Get the selected color variant
    const selectedColorVariant =
      this.productItem.variants.sizeColorVariant?.[index];
    this.selectedColorId = selectedColorVariant?.id ?? '';
    this.cartItem.colorId = selectedColorVariant?.id;

    // Update the main image section with the selected color's images

    if (selectedColorVariant?.image.url != null) {
      const index = this.productItem.variants.sizeColorVariant?.findIndex(
        (col) => col.id === selectedColorVariant.id
      );

      this.selectedImageIndex = index !== undefined && index !== -1 ? index : 0;
    }

    // Update the selected variant with the first size of the selected color
    if (selectedColorVariant?.sizes?.[0]) {
      const firstSizeVariant = selectedColorVariant.sizes[0];
      this.cartItem.sizeId = firstSizeVariant.id;
      this.cartItem.sizeLabel = firstSizeVariant.size;
      this.selectedVariant = {
        label: firstSizeVariant.size || '',
        value: firstSizeVariant.id || '',
        price: firstSizeVariant.price || 0,
        stockQuantity: firstSizeVariant.stockQuantity || 0,
        discountedPrice: firstSizeVariant.discountedPrice || 0,
      };
      this.currentPrice = this.selectedVariant.price;
      this.discountPrice = this.selectedVariant.discountedPrice;
      this.cartItem.price = this.selectedVariant.price;
      this.cartItem.discountedPrice = this.selectedVariant.discountedPrice;
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
      !this.productItem.variants ||
      this.productItem.variantType !== VariantType.ColorAndSize
    )
      return [];

    return (
      this.productItem.variants.sizeColorVariant
        ?.filter((colorVariant: ProductVariantSizeColor) =>
          colorVariant.sizes.some((s) => s.size === size && s.stockQuantity > 0)
        )
        .map((colorVariant: ProductVariantSizeColor) => colorVariant.color) ??
      []
    );
  }
  addToBag() {
    const cartItemCopy = { ...this.cartItem }; // Creates a shallow copy
    console.log(cartItemCopy);
    this.cartService.addToCart(cartItemCopy);
  }

  addToWishlist() {
    console.log(this.selectedVariant);
  }
}
