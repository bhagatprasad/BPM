import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { filter, map, catchError, switchMap, take } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

type BodylessMethod = 'GET' | 'HEAD' | 'DELETE' | 'OPTIONS';
type BodyMethod = 'POST' | 'PUT' | 'PATCH';
type HttpMethod = BodylessMethod | BodyMethod;

// Define the authentication response interface based on your response structure
export interface AuthResponse {
  authenticateResponseDto: {
    userId: string;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    isActive: boolean;
    roleId: string;
    dealerId: string;
    dealerInfo: {
      id: string;
      dealershipName: string;
      contactPerson: string;
      email: string;
      phone: string;
      alternatePhone: string;
      addressLine1: string;
      addressLine2: string;
      city: string;
      state: string;
      country: string;
      postalCode: string;
      gstNumber: string;
      registrationNumber: string;
      tradeLicenseNumber: string;
      website: string;
      isActive: boolean;
    };
  };
  jwtToken: string;
  refreshToken: string;
  message: string;
  isValidUser: boolean;
  isValidPassword: boolean;
}

// Interface for refresh token response from backend
export interface RefreshTokenResponse {
  jwtToken: string;
  refreshToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  constructor(private http: HttpClient) { }

  send<TResponse>(method: BodylessMethod, url: string): Observable<TResponse>;
  send<TResponse>(method: BodyMethod, url: string, body: any): Observable<TResponse>;
  send<TResponse>(method: HttpMethod, url: string, body?: any): Observable<TResponse> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    // Prepend the base URL to the endpoint
    const fullUrl = `${environment.baseUrl}/${url}`;

    // Create the appropriate request based on method type
    let req: HttpRequest<any>;
    switch (method) {
      case 'GET':
      case 'HEAD':
      case 'DELETE':
      case 'OPTIONS':
        req = new HttpRequest(method, fullUrl, { headers });
        break;
      case 'POST':
      case 'PUT':
      case 'PATCH':
        req = new HttpRequest(method, fullUrl, JSON.stringify(body), { headers });
        break;
      default:
        throw new Error(`Unsupported HTTP method: ${method}`);
    }

    return this.http.request<TResponse>(req).pipe(
      filter(event => event instanceof HttpResponse),
      map(event => {
        const response = event as HttpResponse<TResponse>;
        return this.handleResponse<TResponse>(response);
      }),
      catchError((error: HttpErrorResponse) => {
        // Check if error is 402
        if (error.status === 402) {
          return this.handle402Error<TResponse>(req);
        }
        return throwError(() => error);
      })
    );
  }

  private handle402Error<T>(request: HttpRequest<any>): Observable<T> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      const refreshToken = this.getRefreshToken();

      // If no refresh token available, fail immediately
      if (!refreshToken) {
        this.isRefreshing = false;
        this.handleRefreshTokenFailure();
        return throwError(() => new Error('No refresh token available'));
      }

      return this.refreshAccessToken(refreshToken).pipe(
        switchMap((response: RefreshTokenResponse) => {
          this.isRefreshing = false;
          
          // Update ONLY the JWT token in storage, keep everything else
          this.updateJwtToken(response.jwtToken);
          
          // Also update refresh token if backend returns a new one
          if (response.refreshToken) {
            this.updateRefreshToken(response.refreshToken);
          }
          
          this.refreshTokenSubject.next(response.jwtToken);
          
          // Retry the original request with new token
          return this.retryRequest<T>(request);
        }),
        catchError((error) => {
          this.isRefreshing = false;
          // Handle refresh token failure - logout user
          this.handleRefreshTokenFailure();
          return throwError(() => error);
        })
      );
    } else {
      // Wait for token refresh to complete
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap(() => {
          // Retry the original request with new token (interceptor will add it)
          return this.retryRequest<T>(request);
        })
      );
    }
  }

  private refreshAccessToken(refreshToken: string): Observable<RefreshTokenResponse> {
    const url = `${environment.baseUrl}/account/refresh-token`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    
    const body = {
      refreshToken: refreshToken
    };

    return this.http.post<RefreshTokenResponse>(url, JSON.stringify(body), { headers });
  }

  private retryRequest<T>(request: HttpRequest<any>): Observable<T> {
    // Clone the request without modifying headers - interceptor will add the token
    const newRequest = request.clone();
    
    return this.http.request<T>(newRequest).pipe(
      filter(event => event instanceof HttpResponse),
      map(event => {
        const response = event as HttpResponse<T>;
        return this.handleResponse<T>(response);
      })
    );
  }

  private handleResponse<T>(response: HttpResponse<T>): T {
    if (response.status >= 200 && response.status < 300) {
      if (response.body === null && response.status === 204) {
        return true as unknown as T;
      }
      return response.body as T;
    } else {
      console.error('Error response:', response);
      throw new Error(`HTTP error: ${response.status} - ${response.statusText}`);
    }
  }

  private handleRefreshTokenFailure(): void {
    // Clear auth data and redirect to login
    localStorage.removeItem('AuthenticatedUserResponse');
    // You can add navigation to login page here if needed
    // this.router.navigate(['/login']);
    console.error('Refresh token failed. Please login again.');
  }

  // Storage helper methods
  private getAuthData(): AuthResponse | null {
    const loggeddata = localStorage.getItem('AuthenticatedUserResponse');
    return loggeddata ? JSON.parse(loggeddata) : null;
  }

  private getRefreshToken(): string {
    const authData = this.getAuthData();
    return authData?.refreshToken || '';
  }

  private getJwtToken(): string {
    const authData = this.getAuthData();
    return authData?.jwtToken || '';
  }

  // Update ONLY the JWT token, preserve everything else
  private updateJwtToken(newJwtToken: string): void {
    const authData = this.getAuthData();
    if (authData) {
      authData.jwtToken = newJwtToken;
      localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(authData));
    }
  }

  // Update refresh token if backend returns a new one
  private updateRefreshToken(newRefreshToken: string): void {
    const authData = this.getAuthData();
    if (authData) {
      authData.refreshToken = newRefreshToken;
      localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(authData));
    }
  }

  // Update auth data completely (used for login)
  private updateAuthTokens(authResponse: AuthResponse): void {
    // Store the complete auth response
    localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(authResponse));
  }

  // Public method to set auth data after login
  setAuthData(authResponse: AuthResponse): void {
    this.updateAuthTokens(authResponse);
  }

  // Public method to clear auth data
  clearAuthData(): void {
    localStorage.removeItem('AuthenticatedUserResponse');
  }

  // Public method to get current user info
  getCurrentUser(): AuthResponse | null {
    return this.getAuthData();
  }

  // Public method to check if user is authenticated
  isAuthenticated(): boolean {
    const authData = this.getAuthData();
    return authData !== null && !!authData.jwtToken;
  }
}