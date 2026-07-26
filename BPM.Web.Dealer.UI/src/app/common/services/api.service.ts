import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse, HttpErrorResponse, HttpInterceptor, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject, of } from 'rxjs';
import { filter, map, catchError, switchMap, take, tap, finalize, timeout, retry, delay } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

type BodylessMethod = 'GET' | 'HEAD' | 'DELETE' | 'OPTIONS';
type BodyMethod = 'POST' | 'PUT' | 'PATCH';
type HttpMethod = BodylessMethod | BodyMethod;

enum CircuitState {
  CLOSED = 'CLOSED',
  OPEN = 'OPEN',
  HALF_OPEN = 'HALF_OPEN'
}

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

export interface RefreshTokenResponse {
  jwtToken: string;
  refreshToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  //------------------------------------
  // Token Refresh
  //------------------------------------
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
  private readonly TOKEN_KEY = 'AuthenticatedUserResponse';
  private readonly MAX_RETRY_ATTEMPTS = 2;
  private retryCount = 0;

  //------------------------------------
  // Circuit Breaker
  //------------------------------------
  private circuitState: CircuitState = CircuitState.CLOSED;
  private failureCount = 0;
  private halfOpenRequestInProgress = false;
  private lastFailureTime = 0;
  
  private readonly FAILURE_THRESHOLD = 3;
  private readonly RESET_TIMEOUT = 30000; // 30 seconds
  private readonly HALF_OPEN_MAX_ATTEMPTS = 1;
  private halfOpenAttempts = 0;

  // Expose circuit state for UI feedback
  public circuitState$ = new BehaviorSubject<CircuitState>(CircuitState.CLOSED);

  constructor(private http: HttpClient) { }

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
      map(event => {
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
      map(event => {
        const response = event as HttpResponse<T>;
        return this.handleResponse<T>(response);
      }),
      tap(() => {
        this.onSuccess();
        this.retryCount = 0;
      })
    );
  }

  private refreshAccessToken(refreshToken: string): Observable<RefreshTokenResponse> {
    const url = this.buildFullUrl('account/refresh-token');
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    const body = { refreshToken };

    return this.http.post<RefreshTokenResponse>(url, JSON.stringify(body), { headers }).pipe(
      timeout(10000), // 10 second timeout
      catchError((error) => {
        console.error('Refresh token request failed:', error);
        return throwError(() => error);
      })
    );
  }

  //------------------------------------
  // Circuit Breaker Methods
  //------------------------------------
  private canExecute(): boolean {
    const currentState = this.circuitState;

    if (currentState === CircuitState.CLOSED) {
      return true;
    }

    if (currentState === CircuitState.OPEN) {
      const elapsed = Date.now() - this.lastFailureTime;
      if (elapsed >= this.RESET_TIMEOUT) {
        // Transition to HALF_OPEN
        this.transitionToHalfOpen();
        return true;
      }
      return false;
    }

    // HALF_OPEN state
    if (this.halfOpenRequestInProgress) {
      return false;
    }

    if (this.halfOpenAttempts >= this.HALF_OPEN_MAX_ATTEMPTS) {
      return false;
    }

    this.halfOpenRequestInProgress = true;
    this.halfOpenAttempts++;
    return true;
  }

  private onSuccess(): void {
    const currentState = this.circuitState;

    if (currentState === CircuitState.HALF_OPEN) {
      // Success in HALF_OPEN - close circuit
      this.transitionToClosed();
    } else if (currentState === CircuitState.CLOSED) {
      // Reset failure count on success
      this.failureCount = 0;
    }
    // Ignore success in OPEN state
  }

  private onFailure(): void {
    const currentState = this.circuitState;

    if (currentState === CircuitState.CLOSED) {
      this.failureCount++;
      this.lastFailureTime = Date.now();
      
      if (this.failureCount >= this.FAILURE_THRESHOLD) {
        this.transitionToOpen();
      }
    } else if (currentState === CircuitState.HALF_OPEN) {
      // Failure in HALF_OPEN - immediately reopen
      this.transitionToOpen();
    }
    // Ignore failure in OPEN state
  }

  private transitionToOpen(): void {
    this.circuitState = CircuitState.OPEN;
    this.halfOpenRequestInProgress = false;
    this.halfOpenAttempts = 0;
    this.circuitState$.next(CircuitState.OPEN);
    console.warn('🔴 Circuit Breaker: OPEN');
  }

  private transitionToHalfOpen(): void {
    this.circuitState = CircuitState.HALF_OPEN;
    this.halfOpenRequestInProgress = false;
    this.halfOpenAttempts = 0;
    this.circuitState$.next(CircuitState.HALF_OPEN);
    console.warn('🟡 Circuit Breaker: HALF-OPEN');
  }

  private transitionToClosed(): void {
    this.circuitState = CircuitState.CLOSED;
    this.failureCount = 0;
    this.halfOpenRequestInProgress = false;
    this.halfOpenAttempts = 0;
    this.circuitState$.next(CircuitState.CLOSED);
    console.warn('🟢 Circuit Breaker: CLOSED');
  }

  //------------------------------------
  // Response Handling
  //------------------------------------
  private handleResponse<T>(response: HttpResponse<T>): T {
    if (response.status >= 200 && response.status < 300) {
      if (response.body === null && response.status === 204) {
        return true as unknown as T;
      }
      return response.body as T;
    }
    throw new Error(`HTTP error: ${response.status} - ${response.statusText}`);
  }

  //------------------------------------
  // Retry Logic
  //------------------------------------
  private shouldRetry(error: HttpErrorResponse): boolean {
    // Retry on network errors (0) or server errors (5xx) but not on 4xx (except 401 handled separately)
    return error.status === 0 || (error.status >= 500 && error.status < 600);
  }

  private getRetryDelay(attempt: number): number {
    // Exponential backoff: 1s, 2s, 4s
    return Math.min(1000 * Math.pow(2, attempt - 1), 5000);
  }

  private handleRefreshTokenFailure(): void {
    this.clearAuthData();
    this.transitionToOpen();
    console.error('❌ Refresh token failed. Please login again.');
    // Optionally navigate to login page
    // this.router.navigate(['/login']);
  }

  //------------------------------------
  // Storage Management
  //------------------------------------
  private getAuthData(): AuthResponse | null {
    const data = localStorage.getItem(this.TOKEN_KEY);
    if (!data) return null;
    try {
      return JSON.parse(data);
    } catch (e) {
      console.error('Failed to parse auth data:', e);
      return null;
    }
  }

  private getRefreshToken(): string {
    return this.getAuthData()?.refreshToken || '';
  }

  private getJwtToken(): string {
    return this.getAuthData()?.jwtToken || '';
  }

  private updateJwtToken(newJwtToken: string): void {
    const authData = this.getAuthData();
    if (authData) {
      authData.jwtToken = newJwtToken;
      localStorage.setItem(this.TOKEN_KEY, JSON.stringify(authData));
    }
  }

  private updateRefreshToken(newRefreshToken: string): void {
    const authData = this.getAuthData();
    if (authData) {
      authData.refreshToken = newRefreshToken;
      localStorage.setItem(this.TOKEN_KEY, JSON.stringify(authData));
    }
  }

  //------------------------------------
  // Public Methods
  //------------------------------------
  setAuthData(authResponse: AuthResponse): void {
    localStorage.setItem(this.TOKEN_KEY, JSON.stringify(authResponse));
    this.transitionToClosed(); // Reset circuit on login
  }

  clearAuthData(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }

  getCurrentUser(): AuthResponse | null {
    return this.getAuthData();
  }

  isAuthenticated(): boolean {
    const authData = this.getAuthData();
    return authData !== null && !!authData.jwtToken;
  }

  // Reset circuit breaker manually (useful for UI retry buttons)
  resetCircuitBreaker(): void {
    this.transitionToClosed();
  }

  getCircuitState(): Observable<CircuitState> {
    return this.circuitState$.asObservable();
  }

  getCurrentCircuitState(): CircuitState {
    return this.circuitState;
  }
}