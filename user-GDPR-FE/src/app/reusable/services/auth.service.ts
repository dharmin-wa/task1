import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthResponse } from '../modals/auth.model';
import { Observable } from 'rxjs';
import { User } from '../modals/user.modal';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = 'http://192.168.1.65:6005/api/User';

  constructor(private http: HttpClient) { }

  getToken(): string | null {
    return localStorage.getItem('authToken'); // Retrieve the token from localStorage or another source
  }

  setToken(authResponse: AuthResponse): void {
    localStorage.setItem('authToken', authResponse.token); // Store the token
    localStorage.setItem('authUser', JSON.stringify(authResponse.user)); // Store user
  }

  removeToken(): void {
    localStorage.removeItem('authToken'); // Remove the token
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/Login`, { email, password });
  }

  register(registerRequest: User): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/Create`, registerRequest);
  }

}
