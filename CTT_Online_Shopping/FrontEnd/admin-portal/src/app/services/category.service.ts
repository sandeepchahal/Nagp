import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CategoryCommand,
  CategoryView,
} from '../models/category/category.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private apiUrl = 'http://localhost:5175/api/category'; // Update with your API URL

  constructor(private http: HttpClient) {}

  addCategory(category: CategoryCommand): Observable<CategoryCommand> {
    return this.http.post<CategoryCommand>(`${this.apiUrl}/add`, category, {
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }

  getCategories(): Observable<CategoryView[]> {
    return this.http.get<CategoryView[]>(`${this.apiUrl}/get-all`);
  }
  getCategoryById(categoryId: string): Observable<CategoryView> {
    return this.http.get<CategoryView>(`${this.apiUrl}/get/${categoryId}`);
  }

  updateCategory(id: string, category: CategoryCommand) {
    return this.http.put(`${this.apiUrl}/update/${id}`, category);
  }
}
