import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { CategoryView } from '../models/category.model';

@Injectable({
  providedIn: 'root',
})
export class HeaderService {
  private apiUrl = 'http://localhost:5267/api/product'; // Update with your API URL
  constructor(private http: HttpClient) {}

  getCategories(): Observable<CategoryView[]> {
    return this.http.get<CategoryView[]>(`${this.apiUrl}/categories`);
  }
}
