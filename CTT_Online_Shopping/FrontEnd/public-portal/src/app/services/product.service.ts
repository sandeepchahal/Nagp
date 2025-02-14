import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ProductFilterView, ProductView } from '../models/product.model';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = environment.productApiUrl;
  constructor(private http: HttpClient) {}

  getCategories(slug: string): Observable<ProductView[]> {
    return this.http.get<ProductView[]>(`${this.apiUrl}/category/${slug}`);
  }
  getProducts(url: string): Observable<ProductFilterView[]> {
    return this.http.get<ProductFilterView[]>(`${this.apiUrl}${url}`);
  }
}
