import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../environments/environment';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.apiUrl;
  private tokenKey = 'authToken';
  constructor(private readonly http: HttpClient) { }

  login(credentials: { username: string, password: string }): Observable<any> {
    return this.http.post(this.baseUrl + '/api/user/login', credentials);
  }

  storeToken(token: string): boolean {
    try {
      localStorage.setItem('authToken', token);
      return localStorage.getItem('authToken') === token;
    } catch {
      return false;
    }
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getCurrentUserId(): string {
    const token = this.getToken();
    if (!token) return '';
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload?.nameid || '';
  }
  getCurrentUserName(): string {
    const token = this.getToken();
    if (!token) return '';
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload?.unique_name || '';
  }
  

  logout() {
    localStorage.removeItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
  isTokenValid(): boolean {

    const token = localStorage.getItem('authToken');
    if (!token) return false;

    try {
      const decoded: DecodedToken = jwtDecode(token);
      const now = Date.now().valueOf() / 1000;
      return decoded.exp > now;
    } catch (e) {
      return false;
    }
  }
}

interface DecodedToken {
  exp: number;
}
