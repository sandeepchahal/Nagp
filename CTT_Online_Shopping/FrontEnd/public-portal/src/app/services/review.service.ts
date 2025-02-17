import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReviewCommand, ReviewQuery } from '../models/review.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  apiUrl = environment.reviewUrl;
  constructor(private http: HttpClient) {}

  addReview(review: ReviewCommand): Observable<any> {
    return this.http.post(`${this.apiUrl}/add`, review);
  }

  getReviewsByProductId(productId: string): Observable<ReviewQuery[]> {
    return this.http.get<ReviewQuery[]>(`${this.apiUrl}/product/${productId}`);
  }
}
