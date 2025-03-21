import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import {
  ProductItemCommand,
  ProductItemView,
} from '../models/productItem/productItem.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProductItemService {
  private apiUrl = environment.productItemApiUrl;

  constructor(private http: HttpClient) {}

  addProductItem(productItem: ProductItemCommand): Observable<any> {
    return this.http.post(`${this.apiUrl}/add`, productItem);
  }
  getProductItems(): Observable<ProductItemView[]> {
    return this.http.get<ProductItemView[]>(`${this.apiUrl}/get-all`);
  }
  getProductItemById(productItemId: string): Observable<ProductItemView> {
    return this.http.get<ProductItemView>(
      `${this.apiUrl}/get/${productItemId}`
    );
  }
}
