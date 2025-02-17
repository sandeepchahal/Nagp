import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = environment.userApiUrl; // Replace with actual API

  constructor(private http: HttpClient) {}

  getUserDetails(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/details`).pipe(
      catchError((error) => {
        console.error('Error :', error);
        return throwError(() => error);
      })
    );
  }
}
