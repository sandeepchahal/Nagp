import { Component } from '@angular/core';
import { ProductService } from '../../../services/product.service';
import { ProductView } from '../../../models/product.model';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-list-product',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './list-product.component.html',
  styleUrl: './list-product.component.css',
})
export class ListProductComponent {
  products: ProductView[] = [];
  constructor(
    private productService: ProductService,
    private route: ActivatedRoute
  ) {
    //get the  <a [routerLink]="'/product/category/' + sub.slug">{{ sub.name }}</a> from url
    const slug = this.route.snapshot.paramMap.get('slug') || '';
    this.GetProducts(slug);
  }

  GetProducts(slug: string) {
    this.productService.getCategories(slug).subscribe((data) => {
      this.products = data;
    });
  }

  // Calculate discount percentage
  calculateDiscount(price: {
    originalPrice: number;
    discountPrice: number;
  }): number {
    if (price.discountPrice > 0) {
      return Math.round(
        ((price.originalPrice - price.discountPrice) / price.originalPrice) *
          100
      );
    }
    return 0;
  }
}
