import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CategoryCommand,
  CategoryView,
} from '../models/category/category.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private apiUrl = environment.categoryApiUrl;

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
