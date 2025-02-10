import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HeaderService } from '../../../services/header.service';
import { CategoryView } from '../../../models/category.model';
import { Router } from '@angular/router';
import { PopupCartComponent } from '../../cart/popup-cart/popup-cart.component';

@Component({
  selector: 'app-main-header',
  standalone: true,
  imports: [CommonModule, PopupCartComponent],
  templateUrl: './main-header.component.html',
  styleUrl: './main-header.component.css',
})
export class MainHeaderComponent {
  categoriesByGender: { [gender: string]: CategoryView[] } = {};

  constructor(private headerService: HeaderService, private router: Router) {
    this.headerService.getCategories().subscribe((data) => {
      this.categoriesByGender = data.reduce((acc, category) => {
        if (!acc[category.gender]) {
          acc[category.gender] = [];
        }
        acc[category.gender].push(category);
        return acc;
      }, {} as { [gender: string]: CategoryView[] }); // Type assertion here
    });
  }

  // Add a getter to return the keys of categoriesByGender
  get genderKeys(): string[] {
    return Object.keys(this.categoriesByGender);
  }

  showProducts(slug: string) {
    this.router.navigate(['/product/category', slug]);
  }
}
