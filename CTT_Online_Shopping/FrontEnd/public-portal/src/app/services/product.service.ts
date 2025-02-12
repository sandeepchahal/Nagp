import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ProductFilterView, ProductView } from '../models/product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = 'http://localhost:5267/api/product'; // Update with your API URL
  constructor(private http: HttpClient) {}

  getCategories(slug: string): Observable<ProductView[]> {
    return this.http.get<ProductView[]>(`${this.apiUrl}/category/${slug}`);
  }
  getProducts(url: string): Observable<ProductFilterView[]> {
    return this.http.get<ProductFilterView[]>(`${this.apiUrl}${url}`);
  }
}
