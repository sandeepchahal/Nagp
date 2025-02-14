import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { CategoryView } from '../models/category.model';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class HeaderService {
  private apiUrl = environment.productApiUrl; // Update with your API URL
  constructor(private http: HttpClient) {}

  getCategories(): Observable<CategoryView[]> {
    return this.http.get<CategoryView[]>(`${this.apiUrl}/categories`);
  }
}
