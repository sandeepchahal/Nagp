import { Component } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DiscountType, VariantType } from '../../../models/enums';
import { ProductItemCommand } from '../../../models/productItem/productItem.model';
import { CommonModule } from '@angular/common';
import { ProductItemService } from '../../../services/productItem.service';

@Component({
  selector: 'app-add-product-item',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './add-product-item.component.html',
  styleUrls: ['./add-product-item.component.css'],
})
export class AddProductItemComponent {
  productItemForm: FormGroup;
  discountTypes = Object.values(DiscountType);
  variantTypes = Object.values(VariantType);
  productId: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private productItemService: ProductItemService,
    private router: Router
  ) {
    this.productId = this.route.snapshot.paramMap.get('id') || ''; // Get product ID from URL
    this.productItemForm = this.fb.group({
      productId: [
        { value: this.productId, disabled: true },
        Validators.required,
      ],
      variantType: [VariantType.Size, Validators.required], // Default to Size
      variant: this.fb.group({
        discount: this.fb.group({
          type: [DiscountType.None, Validators.required],
          value: [0, [Validators.required, Validators.min(0)]],
        }),
        sizeVariant: this.fb.array([]),
        colorVariant: this.fb.array([]),
        sizeColorVariant: this.fb.array([]),
        images: this.fb.array([]), // Add images array at the variant level
      }),
    });
  }

  get variant(): FormGroup {
    return this.productItemForm.get('variant') as FormGroup;
  }

  get images(): FormArray {
    return this.variant.get('images') as FormArray;
  }

  addImage(): void {
    const imageIndex = this.images.length + 1;
    this.images.push(
      this.fb.group({
        url: ['', Validators.required],
        altText: [`image-${imageIndex}`],
        orderNumber: [imageIndex, [Validators.required, Validators.min(1)]],
      })
    );
  }

  get sizeVariants(): FormArray {
    return this.variant.get('sizeVariant') as FormArray;
  }

  addSizeVariant(): void {
    this.sizeVariants.push(
      this.fb.group({
        size: ['', Validators.required],
        stockQuantity: [0, [Validators.required, Validators.min(0)]],
        price: [0, [Validators.required, Validators.min(0)]],
        discount: this.fb.group({
          type: [DiscountType.None, Validators.required],
          value: [0, [Validators.required, Validators.min(0)]],
        }),
      })
    );
  }

  get colorVariants(): FormArray {
    return this.variant.get('colorVariant') as FormArray;
  }

  addColorVariant(): void {
    const colorIndex = this.colorVariants.length + 1;
    this.colorVariants.push(
      this.fb.group({
        color: ['', Validators.required],
        stockQuantity: [0, [Validators.required, Validators.min(0)]],
        price: [0, [Validators.required, Validators.min(0)]],
        discount: this.fb.group({
          type: [DiscountType.None, Validators.required],
          value: [0, [Validators.required, Validators.min(0)]],
        }),
        image: this.fb.group({
          url: ['', Validators.required],
          altText: [`color-${colorIndex}`],
        }),
      })
    );
  }

  get sizeColorVariants(): FormArray {
    return this.variant.get('sizeColorVariant') as FormArray;
  }

  addSizeColorVariant(): void {
    const sizeColorIndex = this.sizeColorVariants.length + 1;
    this.sizeColorVariants.push(
      this.fb.group({
        color: ['', Validators.required],
        image: this.fb.group({
          url: ['', Validators.required], // Ensure URL is required
          altText: [`color-${sizeColorIndex}`], // Default alt text
        }),
        sizes: this.fb.array([]),
      })
    );
    this.addSizeToSizeColorVariant(this.sizeColorVariants.length - 1); // Add one size by default
  }

  getSizes(sizeColorVariantIndex: number): FormArray {
    return this.sizeColorVariants
      .at(sizeColorVariantIndex)
      .get('sizes') as FormArray;
  }

  addSizeToSizeColorVariant(sizeColorVariantIndex: number): void {
    const sizeIndex = this.getSizes(sizeColorVariantIndex).length + 1;
    this.getSizes(sizeColorVariantIndex).push(
      this.fb.group({
        size: ['', Validators.required],
        stockQuantity: [0, [Validators.required, Validators.min(0)]],
        price: [0, [Validators.required, Validators.min(0)]],
        discount: this.fb.group({
          type: [DiscountType.None, Validators.required],
          value: [0, [Validators.required, Validators.min(0)]],
        }),
      })
    );
  }

  onSubmit(): void {
    if (this.productItemForm.valid) {
      const productItem: ProductItemCommand = {
        ...this.productItemForm.getRawValue(), // Include disabled fields
      };
      console.log('Product Item to be submitted:', productItem);
      // Call your API service here to submit the product item
      this.productItemService.addProductItem(productItem).subscribe({
        next: () => {
          alert('Product Item added successfully');
          this.router.navigate(['/product/items']);
        },
        error: () => alert('Error adding Product item'),
      });
    } else {
      console.error('Form is invalid');
    }
  }
}
