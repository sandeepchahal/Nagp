import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BrandCommand, BrandView } from '../models/brand/brand.model';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BrandService {
  private apiUrl = environment.brandApiUrl; // Update with your API URL

  constructor(private http: HttpClient) {}

  addBrand(product: BrandCommand): Observable<BrandCommand> {
    return this.http.post<BrandCommand>(`${this.apiUrl}/add`, product, {
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }
  getAll(): Observable<BrandView[]> {
    return this.http.get<BrandView[]>(`${this.apiUrl}/get-all`);
  }
}
