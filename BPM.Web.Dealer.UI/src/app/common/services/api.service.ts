import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject, of } from 'rxjs';
import { filter, map, catchError, switchMap, take, tap, delay, finalize } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

type BodylessMethod = 'GET' | 'HEAD' | 'DELETE' | 'OPTIONS';
type BodyMethod = 'POST' | 'PUT' | 'PATCH';
type HttpMethod = BodylessMethod | BodyMethod;

// Circuit Breaker States
export enum CircuitState {
  CLOSED = 'CLOSED',
  OPEN = 'OPEN',
  HALF_OPEN = 'HALF_OPEN'
}

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
  // Circuit Breaker properties
  private circuitState: CircuitState = CircuitState.CLOSED;
  private failureCount = 0;
  private halfOpenRequestInProgress = false;
  private lastFailureTime = 0;
  private halfOpenAttempts = 0;
  
  private readonly FAILURE_THRESHOLD = 3;
  private readonly RESET_TIMEOUT = 30000; // 30 seconds
  private readonly HALF_OPEN_MAX_ATTEMPTS = 1;
  private readonly MAX_RETRY_ATTEMPTS = 3;
  private retryCount = 0;

  // Token refresh properties
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  // Expose circuit state for UI feedback
  public circuitState$ = new BehaviorSubject<CircuitState>(CircuitState.CLOSED);

  constructor(private http: HttpClient) { }

  //------------------------------------
  // Circuit Breaker Methods
  //------------------------------------
  private canExecute(): boolean {
    if (this.circuitState === CircuitState.CLOSED) {
      return true;
    }

    if (this.circuitState === CircuitState.OPEN) {
      // Check if reset timeout has elapsed
      const now = Date.now();
      if (now - this.lastFailureTime >= this.RESET_TIMEOUT) {
        // Move to half-open state
        this.circuitState = CircuitState.HALF_OPEN;
        this.halfOpenAttempts = 0;
        this.circuitState$.next(CircuitState.HALF_OPEN);
        console.log('Circuit Breaker: Moving to HALF_OPEN state');
        return true;
      }
      console.warn(`Circuit Breaker: OPEN. Retry after ${(this.RESET_TIMEOUT - (now - this.lastFailureTime)) / 1000}s`);
      return false;
    }

    if (this.circuitState === CircuitState.HALF_OPEN) {
      if (this.halfOpenAttempts >= this.HALF_OPEN_MAX_ATTEMPTS) {
        console.warn('Circuit Breaker: HALF_OPEN - max attempts reached');
        return false;
      }
      this.halfOpenAttempts++;
      return true;
    }

    return true;
  }

  private onSuccess(): void {
    if (this.circuitState === CircuitState.HALF_OPEN) {
      // If we succeed in half-open, close the circuit
      this.circuitState = CircuitState.CLOSED;
      this.failureCount = 0;
      this.halfOpenAttempts = 0;
      this.circuitState$.next(CircuitState.CLOSED);
      console.log('Circuit Breaker: CLOSED (success in half-open)');
    } else {
      // Reset failure count on success
      this.failureCount = 0;
    }
  }

  private onFailure(): void {
    this.failureCount++;
    this.lastFailureTime = Date.now();

    if (this.circuitState === CircuitState.HALF_OPEN) {
      // Failure in half-open state -> open circuit
      this.circuitState = CircuitState.OPEN;
      this.circuitState$.next(CircuitState.OPEN);
      console.log('Circuit Breaker: OPEN (failure in half-open)');
      return;
    }

    if (this.failureCount >= this.FAILURE_THRESHOLD) {
      this.circuitState = CircuitState.OPEN;
      this.circuitState$.next(CircuitState.OPEN);
      console.log(`Circuit Breaker: OPEN (${this.failureCount} failures)`);
    }
  }

  //------------------------------------
  // Retry Logic Helper Methods
  //------------------------------------
  private shouldRetry(error: HttpErrorResponse): boolean {
    // Retry on network errors (status 0) and server errors (5xx)
    return error.status === 0 || error.status >= 500;
  }

  private getRetryDelay(attempt: number): number {
    // Exponential backoff: 1s, 2s, 4s, 8s...
    return Math.min(1000 * Math.pow(2, attempt - 1), 10000);
  }

  //------------------------------------
  // Main Request Method
  //------------------------------------
  send<TResponse>(method: BodylessMethod, url: string): Observable<TResponse>;
  send<TResponse>(method: BodyMethod, url: string, body: any): Observable<TResponse>;
  send<TResponse>(method: HttpMethod, url: string, body?: any): Observable<TResponse> {
    // Check circuit breaker before executing
    if (!this.canExecute()) {
      return throwError(() => new Error('Circuit Breaker is OPEN. Service temporarily unavailable.'));
    }

    const headers = this.getDefaultHeaders();
    const fullUrl = this.buildFullUrl(url);
    const request = this.buildRequest(method, fullUrl, body, headers);

    return this.executeWithRetry<TResponse>(request);
  }

  //------------------------------------
  // Private Helper Methods
  //------------------------------------
  private getDefaultHeaders(): HttpHeaders {
    const token = this.getJwtToken();
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }

    return headers;
  }

  private buildFullUrl(endpoint: string): string {
    // Remove leading/trailing slashes to avoid double slashes
    const base = environment.baseUrl.replace(/\/+$/, '');
    const path = endpoint.replace(/^\/+/, '');
    return `${base}/${path}`;
  }

  private buildRequest(method: HttpMethod, url: string, body: any, headers: HttpHeaders): HttpRequest<any> {
    if (method === 'GET' || method === 'HEAD' || method === 'DELETE' || method === 'OPTIONS') {
      return new HttpRequest(method, url, { headers });
    } else {
      // POST, PUT, PATCH
      return new HttpRequest(method, url, JSON.stringify(body), { headers });
    }
  }

  private executeWithRetry<T>(request: HttpRequest<any>): Observable<T> {
    return this.http.request<T>(request).pipe(
      filter(event => event instanceof HttpResponse),
      map((event: any) => {
        const response = event as HttpResponse<T>;
        return this.handleResponse<T>(response);
      }),
      tap({
        next: () => {
          // Success - reset circuit breaker
          this.onSuccess();
          this.retryCount = 0; // Reset retry count on success
        },
        error: (error: HttpErrorResponse) => {
          // Only handle specific errors
          if (error.status === 401) {
            // Token expired - handle refresh
            return this.handle401Error<T>(request);
          }
          
          // Network errors or server errors (5xx)
          if (error.status === 0 || error.status >= 500) {
            this.onFailure();
          }
          
          // Re-throw for other errors
          return throwError(() => error);
        }
      }),
      catchError((error: HttpErrorResponse) => {
        // Check if we should retry
        if (this.shouldRetry(error) && this.retryCount < this.MAX_RETRY_ATTEMPTS) {
          this.retryCount++;
          const delayMs = this.getRetryDelay(this.retryCount);
          console.warn(`Retrying request. Attempt ${this.retryCount} of ${this.MAX_RETRY_ATTEMPTS}`);
          
          return of(error).pipe(
            delay(delayMs),
            switchMap(() => this.executeWithRetry<T>(request))
          );
        }
        
        // Handle 401 with token refresh
        if (error.status === 401) {
          return this.handle401Error<T>(request);
        }
        
        return throwError(() => error);
      })
    );
  }

  //------------------------------------
  // Token Refresh Logic
  //------------------------------------
  private handle401Error<T>(request: HttpRequest<any>): Observable<T> {
    // Don't attempt refresh for login requests or when no refresh token
    if (request.url.includes('/login') || request.url.includes('/refresh-token')) {
      this.handleRefreshTokenFailure();
      return throwError(() => new Error('Authentication required'));
    }

    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      const refreshToken = this.getRefreshToken();
      if (!refreshToken) {
        this.isRefreshing = false;
        this.handleRefreshTokenFailure();
        return throwError(() => new Error('No refresh token available'));
      }

      return this.refreshAccessToken(refreshToken).pipe(
        switchMap((response: RefreshTokenResponse) => {
          this.isRefreshing = false;
          
          // Update tokens
          this.updateJwtToken(response.jwtToken);
          if (response.refreshToken) {
            this.updateRefreshToken(response.refreshToken);
          }
          
          this.refreshTokenSubject.next(response.jwtToken);
          
          // Reset retry count for successful refresh
          this.retryCount = 0;
          
          // Retry original request with new token
          return this.retryOriginalRequest<T>(request);
        }),
        catchError((error) => {
          this.isRefreshing = false;
          this.handleRefreshTokenFailure();
          return throwError(() => error);
        }),
        finalize(() => {
          this.isRefreshing = false;
        })
      );
    } else {
      // Wait for token refresh to complete
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap(() => {
          // Retry with updated token
          return this.retryOriginalRequest<T>(request);
        })
      );
    }
  }

  private retryOriginalRequest<T>(request: HttpRequest<any>): Observable<T> {
    // Clone request with updated headers
    const token = this.getJwtToken();
    const headers = request.headers.set('Authorization', `Bearer ${token}`);
    const newRequest = request.clone({ headers });

    return this.http.request<T>(newRequest).pipe(
      filter(event => event instanceof HttpResponse),
      map((event: any) => {
        const response = event as HttpResponse<T>;
        return this.handleResponse<T>(response);
      }),
      catchError((error: HttpErrorResponse) => {
        // Check if error is 402
        if (error.status === 402) {
          return this.handle402Error<T>(request);
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
      map((event: any) => {
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