// src/app/services/user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../reusable/modals/user.modal'; // Assuming you have a User modal

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://dummyjson.com/users';
  private baseUrl = 'http://192.168.1.65:6005/api/User';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http.get<{ users: User[] }>(this.apiUrl).pipe(
      map(response => response.users.slice(0, 10)) // Return the first 10 users
    );
  }

 // Register user
  register(userData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/create`, userData);
  }

  // Login user
  login(email: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/get-detail?userId=${email}`);
  }

  // Get all users
  getAllUsers(): Observable<any> {
    const token = localStorage.getItem('authToken'); // Change 'authToken' to your actual token key
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`, // Add Bearer token
    });
    return this.http.get(`${this.baseUrl}/get-all?isAdmin=true`, { headers });
  }

  deleteUser(userId: string): Observable<any> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`, // Include the token in the headers
    });

    return this.http.delete(`${this.baseUrl}/delete/${userId}`, { headers });
  }
}
