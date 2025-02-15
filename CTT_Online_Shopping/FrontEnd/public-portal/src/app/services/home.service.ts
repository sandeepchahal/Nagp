import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ProductFilterView, ProductView } from '../models/product.model';
import { environment } from '../../environments/environment';
import { HomePage } from '../models/home.model';
@Injectable({
  providedIn: 'root',
})
export class HomeService {
  private apiUrl = environment.homeApiUrl;
  constructor(private http: HttpClient) {}

  getHomePage(): Observable<HomePage> {
    return this.http.get<HomePage>(`${this.apiUrl}/get`);
  }
}
