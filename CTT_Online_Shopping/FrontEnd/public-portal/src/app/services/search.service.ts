import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { SearchResponse } from '../models/searchResponse.model';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  private apiUrl = 'http://localhost:5204/api/search'; // Update with your API URL
  constructor(private http: HttpClient) {}

  search(query: string): Observable<SearchResponse[]> {
    return this.http.get<SearchResponse[]>(`${this.apiUrl}?query=${query}`);
  }
}
