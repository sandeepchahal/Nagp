import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { SearchResponse } from '../models/searchResponse.model';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  private apiUrl =
    'http://searchapi-public-service.default.svc.cluster.local/api/search'; // Update with your API URL

  private searchQuerySubject = new BehaviorSubject<string>('');
  searchQuery$ = this.searchQuerySubject.asObservable();

  constructor(private http: HttpClient) {}

  search(query: string): Observable<SearchResponse[]> {
    return this.http.get<SearchResponse[]>(`${this.apiUrl}?query=${query}`);
  }

  // Method to update the search query
  updateSearchQuery(query: string): void {
    this.searchQuerySubject.next(query);
  }
}
