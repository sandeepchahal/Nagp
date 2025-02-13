import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import {
  ProductCategoryView,
  ProductCommand,
  ProductView,
} from '../models/product/product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl =
    'http://productapi-admin-service.default.svc.cluster.local/api/product';

  constructor(private http: HttpClient) {}

  addProduct(product: ProductCommand): Observable<ProductCommand> {
    return this.http.post<ProductCommand>(`${this.apiUrl}/add`, product, {
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }
  getProducts(): Observable<ProductView[]> {
    return this.http.get<ProductView[]>(`${this.apiUrl}/get-all`);
  }

  getProductById(id: string): Observable<ProductView> {
    return this.http.get<ProductView>(`${this.apiUrl}/get/${id}`);
  }
  updateProduct(id: string, product: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/update/${id}`, product);
  }
}
