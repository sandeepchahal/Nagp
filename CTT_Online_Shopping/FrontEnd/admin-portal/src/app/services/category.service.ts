import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CategoryCommand } from '../models/category/category.model';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private apiUrl = 'http://localhost:5000/api/categories'; // Update with your API URL

  constructor(private http: HttpClient) {}

  addCategory(category: CategoryCommand): Observable<CategoryCommand> {
    return this.http.post<CategoryCommand>(`${this.apiUrl}/add`, category);
  }
}
