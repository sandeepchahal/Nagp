import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormArray,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ProductItemService } from '../../../services/productItem.service';
import { CommonModule } from '@angular/common';
import {
  DiscountType,
  ProductItemCommand,
} from '../../../models/productItem/productItem.model';

@Component({
  selector: 'app-add-product-item',
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule],
  templateUrl: './add-product-item.component.html',
  styleUrls: ['./add-product-item.component.css'],
})
export class AddProductItemComponent {
  productForm: FormGroup;
  discountTypes = Object.values(DiscountType);

  constructor(private fb: FormBuilder, private route: ActivatedRoute) {
    this.productForm = this.fb.group({
      productId: ['', Validators.required],
      name: ['', Validators.required],
      productLevelDiscount: this.fb.group({
        type: [DiscountType.None, Validators.required],
        value: [0, [Validators.required, Validators.min(0)]],
      }),
      variants: this.fb.array([]),
    });

    this.addVariant(); // Add one variant by default
  }
  ngOnInit(): void {
    const productId = this.route.snapshot.paramMap.get('id'); // Get 'id' from URL
    if (productId) {
      this.productForm.get('productId')?.setValue(productId); // Set Product ID
      this.productForm.get('productId')?.disable(); // Disable the field
    }
  }

  get variants(): FormArray {
    return this.productForm.get('variants') as FormArray;
  }

  addVariant(): void {
    const variantGroup = this.fb.group({
      attributes: this.fb.array([]),
      images: this.fb.array([]),
      discount: this.fb.group({
        type: [DiscountType.None, Validators.required],
        value: [0, [Validators.required, Validators.min(0)]],
      }),
    });

    this.variants.push(variantGroup);
    this.addAttribute(this.variants.length - 1); // Add one attribute by default
    this.addImage(this.variants.length - 1); // Add one image by default
  }

  getAttributes(variantIndex: number): FormArray {
    return this.variants.at(variantIndex).get('attributes') as FormArray;
  }

  addAttribute(variantIndex: number): void {
    const attributeGroup = this.fb.group({
      features: this.fb.array([this.createFeature()]), // Add one feature input by default
      stockQuantity: [0, [Validators.required, Validators.min(0)]],
      price: [0, [Validators.required, Validators.min(0)]],
    });

    this.getAttributes(variantIndex).push(attributeGroup);
  }

  createFeature(): FormGroup {
    return this.fb.group({
      value: [''],
    });
  }

  getFeatures(variantIndex: number, attributeIndex: number): FormArray {
    return this.getAttributes(variantIndex)
      .at(attributeIndex)
      .get('features') as FormArray;
  }

  addFeature(variantIndex: number, attributeIndex: number): void {
    this.getFeatures(variantIndex, attributeIndex).push(this.createFeature());
  }

  getImages(variantIndex: number): FormArray {
    return this.variants.at(variantIndex).get('images') as FormArray;
  }

  addImage(variantIndex: number): void {
    this.getImages(variantIndex).push(
      this.fb.group({
        url: ['', Validators.required],
        altText: [''],
        orderNumber: [0, [Validators.required, Validators.min(0)]],
      })
    );
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      const product: ProductItemCommand = this.productForm.value;
      console.log('Product to be submitted:', product);
      // Call your API service here to submit the product
    } else {
      console.error('Form is invalid');
    }
  }
}
