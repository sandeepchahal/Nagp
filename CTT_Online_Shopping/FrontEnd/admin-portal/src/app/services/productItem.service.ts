import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ProductItemCommand } from '../models/productItem/productItem.model';
@Injectable({
  providedIn: 'root',
})
export class ProductItemService {
  private apiUrl = 'http://localhost:5175/api/product/item'; // Update with your API URL
  constructor(private http: HttpClient) {}

  addProductItem(productItem: ProductItemCommand): Observable<any> {
    return this.http.post(`${this.apiUrl}/add`, productItem);
  }
}
