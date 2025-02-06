import { Component } from '@angular/core';
import { BrandCommand } from '../../../models/brand/brand.model';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BrandService } from '../../../services/brand.service';

@Component({
  selector: 'app-add-brand',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './add-brand.component.html',
  styleUrl: './add-brand.component.css',
})
export class AddBrandComponent {
  brand: BrandCommand = {
    name: '',
    id: '',
  };

  constructor(private brandService: BrandService, private router: Router) {}

  addBrand(): void {
    if (!this.brand.name.trim()) {
      alert('Brand name is required.');
      return;
    }

    this.brandService.addBrand(this.brand).subscribe({
      next: () => {
        alert('Brand added successfully');
        this.router.navigate(['/brands']);
      },
      error: () => alert('Error adding brand'),
    });
  }
}
