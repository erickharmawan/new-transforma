import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse, User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5056/api';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  private policiesSubject = new BehaviorSubject<string[]>([]);
  
  currentUser$ = this.currentUserSubject.asObservable();
  policies$ = this.policiesSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, credentials)
      .pipe(
        tap(response => {
          this.setSession(response);
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    localStorage.removeItem('policies');
    this.currentUserSubject.next(null);
    this.policiesSubject.next([]);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  hasPolicy(policyName: string): boolean {
    return this.policiesSubject.value.includes(policyName);
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  private setSession(authResult: LoginResponse): void {
    localStorage.setItem('token', authResult.token);
    localStorage.setItem('user', JSON.stringify(authResult.user));
    localStorage.setItem('policies', JSON.stringify(authResult.policies));
    this.currentUserSubject.next(authResult.user);
    this.policiesSubject.next(authResult.policies);
  }

  private loadUserFromStorage(): void {
    const userJson = localStorage.getItem('user');
    const policiesJson = localStorage.getItem('policies');
    
    if (userJson) {
      this.currentUserSubject.next(JSON.parse(userJson));
    }
    if (policiesJson) {
      this.policiesSubject.next(JSON.parse(policiesJson));
    }
  }
}
