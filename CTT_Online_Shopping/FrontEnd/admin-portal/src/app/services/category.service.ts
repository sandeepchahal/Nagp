import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryCommand } from '../models/category/category.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private apiUrl = 'http://localhost:5175/api/category'; // Update with your API URL

  constructor(private http: HttpClient) {}

  addCategory(category: CategoryCommand): Observable<CategoryCommand> {
    return this.http.post<CategoryCommand>(`${this.apiUrl}/add`, category);
  }
}
