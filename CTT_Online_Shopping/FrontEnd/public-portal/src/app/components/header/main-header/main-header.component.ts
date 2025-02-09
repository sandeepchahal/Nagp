import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HeaderService } from '../../../services/header.service';
import { CategoryView } from '../../../models/category.model';

@Component({
  selector: 'app-main-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './main-header.component.html',
  styleUrl: './main-header.component.css',
})
export class MainHeaderComponent implements OnInit {
  // Declare categoriesByGender with the correct type
  categoriesByGender: { [gender: string]: CategoryView[] } = {};

  constructor(private headerService: HeaderService) {}

  ngOnInit(): void {
    this.headerService.getCategories().subscribe((data) => {
      // Group categories by gender
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
}
