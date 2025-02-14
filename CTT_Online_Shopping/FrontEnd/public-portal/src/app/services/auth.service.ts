import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.authApiUrl;

  private userInfoSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
    null
  );

  userInfo$ = this.userInfoSubject.asObservable();

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
  public setUserInfo(token: string) {
    const decodedToken = this.decodeToken(token);
    this.userInfoSubject.next(decodedToken);
  }
  getUserInfo(): any {
    return this.userInfoSubject.value;
  }

  public decodeToken(token: string): any {
    localStorage.setItem('authToken', token); // Save the JWT token
    return jwtDecode(token); // Decode the JWT token
  }
  logout(): void {
    this.userInfoSubject.next(null); // Clear user info
    localStorage.removeItem('authToken');
  }
}
