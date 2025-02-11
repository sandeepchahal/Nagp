import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'YOUR_BACKEND_API_URL'; // Replace with your backend API URL

  isAuthenticated(): boolean {
    return !!localStorage.getItem('authToken');
  }
  constructor(private http: HttpClient) {}
  // Regular login
  login(credentials: { username: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials);
  }

  // Google login
  googleLogin(idToken: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/validate-social-login`, {
      provider: 'Google',
      idToken: idToken,
    });
  }
}
