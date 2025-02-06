import { Component, OnInit } from '@angular/core';
import { BrandService } from '../../../services/brand.service';
import { BrandView } from '../../../models/brand/brand.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-list-brand',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './list-brand.component.html',
  styleUrl: './list-brand.component.css',
})
export class ListBrandComponent implements OnInit {
  brands: BrandView[] = [];

  constructor(private brandService: BrandService) {}

  ngOnInit(): void {
    this.loadBrands();
  }

  loadBrands(): void {
    this.brandService.getAll().subscribe({
      next: (data) => {
        this.brands = data;
      },
      error: () => alert('Error fetching brands'),
    });
  }
}
