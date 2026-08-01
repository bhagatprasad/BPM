import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject, timer } from 'rxjs';
import { filter, map, catchError, switchMap, take, tap, finalize } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';
import { RefreshTokenResponse } from '@app/models/refresh-token-response';
import { AuthResponse } from '@app/models/auth-response';

type BodylessMethod = 'GET' | 'HEAD' | 'DELETE' | 'OPTIONS';
type BodyMethod = 'POST' | 'PUT' | 'PATCH';
type HttpMethod = BodylessMethod | BodyMethod;

// Circuit Breaker States
export enum CircuitState {
  CLOSED = 'CLOSED',
  OPEN = 'OPEN',
  HALF_OPEN = 'HALF_OPEN'
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  // Circuit Breaker properties
  private circuitState: CircuitState = CircuitState.CLOSED;
  private failureCount = 0;
  private lastFailureTime = 0;
  private halfOpenAttempts = 0;

  private readonly FAILURE_THRESHOLD = 3;
  private readonly RESET_TIMEOUT = 30000;
  private readonly HALF_OPEN_MAX_ATTEMPTS = 1;
  private readonly MAX_RETRY_ATTEMPTS = 3;

  // Token refresh properties
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  // Expose circuit state for UI feedback
  public circuitState$ = new BehaviorSubject<CircuitState>(CircuitState.CLOSED);

  constructor(private http: HttpClient, private router: Router) { }

  //------------------------------------
  // Circuit Breaker Methods
  //------------------------------------
  private canExecute(): boolean {
    if (this.circuitState === CircuitState.CLOSED) {
      return true;
    }

    if (this.circuitState === CircuitState.OPEN) {
      const now = Date.now();
      if (now - this.lastFailureTime >= this.RESET_TIMEOUT) {
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
      this.circuitState = CircuitState.CLOSED;
      this.failureCount = 0;
      this.halfOpenAttempts = 0;
      this.circuitState$.next(CircuitState.CLOSED);
      console.log('Circuit Breaker: CLOSED (success in half-open)');
    } else {
      this.failureCount = 0;
    }
  }

  private onFailure(): void {
    this.failureCount++;
    this.lastFailureTime = Date.now();

    if (this.circuitState === CircuitState.HALF_OPEN) {
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
  // Main Request Method
  //------------------------------------
  send<TResponse>(method: BodylessMethod, url: string): Observable<TResponse>;
  send<TResponse>(method: BodyMethod, url: string, body: any): Observable<TResponse>;
  send<TResponse>(method: HttpMethod, url: string, body?: any): Observable<TResponse> {
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
    const base = environment.baseUrl.replace(/\/+$/, '');
    const path = endpoint.replace(/^\/+/, '');
    return `${base}/${path}`;
  }

  private buildRequest(method: HttpMethod, url: string, body: any, headers: HttpHeaders): HttpRequest<any> {
    if (method === 'GET' || method === 'HEAD' || method === 'DELETE' || method === 'OPTIONS') {
      return new HttpRequest(method, url, { headers });
    } else {
      return new HttpRequest(method, url, JSON.stringify(body), { headers });
    }
  }

  private executeWithRetry<T>(request: HttpRequest<any>): Observable<T> {
    let retryCount = 0;
    const maxRetries = this.MAX_RETRY_ATTEMPTS;

    return this.http.request<T>(request).pipe(
      filter(event => event instanceof HttpResponse),
      map((event: any) => {
        const response = event as HttpResponse<T>;
        return this.handleResponse<T>(response);
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.handle401Error<T>(request);
        }

        if (error.status === 402) {
          return this.handle402Error<T>(request);
        }

        if (error.status === 0 || error.status >= 500) {
          if (retryCount < maxRetries) {
            retryCount++;
            const delayMs = this.getRetryDelay(retryCount);
            console.warn(`Retrying request. Attempt ${retryCount} of ${maxRetries}`);
            this.onFailure();

            return timer(delayMs).pipe(
              switchMap(() => this.executeWithRetry<T>(request))
            );
          }
        }

        this.onFailure();
        return throwError(() => error);
      }),
      tap({
        next: () => {
          this.onSuccess();
        }
      })
    );
  }

  private getRetryDelay(attempt: number): number {
    return Math.min(1000 * Math.pow(2, attempt - 1), 10000);
  }

  //------------------------------------
  // Token Refresh Logic
  //------------------------------------
  private handle401Error<T>(request: HttpRequest<any>): Observable<T> {
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

          this.updateJwtToken(response.jwtToken);
          if (response.refreshToken) {
            this.updateRefreshToken(response.refreshToken);
          }

          this.refreshTokenSubject.next(response.jwtToken);
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
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap(() => {
          return this.retryOriginalRequest<T>(request);
        })
      );
    }
  }

  private handle402Error<T>(request: HttpRequest<any>): Observable<T> {
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

          this.updateJwtToken(response.jwtToken);
          if (response.refreshToken) {
            this.updateRefreshToken(response.refreshToken);
          }

          this.refreshTokenSubject.next(response.jwtToken);
          return this.retryOriginalRequest<T>(request);
        }),
        catchError((error) => {
          this.isRefreshing = false;
          this.handleRefreshTokenFailure();
          return throwError(() => error);
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap(() => {
          return this.retryOriginalRequest<T>(request);
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

  private retryOriginalRequest<T>(request: HttpRequest<any>): Observable<T> {
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
        if (error.status === 401) {
          this.handleRefreshTokenFailure();
        }
        return throwError(() => error);
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
    localStorage.removeItem('AuthenticatedUserResponse');
    this.router.navigate(['/login']);
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

  private updateJwtToken(newJwtToken: string): void {
    const authData = this.getAuthData();
    if (authData) {
      authData.jwtToken = newJwtToken;
      localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(authData));
    }
  }

  private updateRefreshToken(newRefreshToken: string): void {
    const authData = this.getAuthData();
    if (authData) {
      authData.refreshToken = newRefreshToken;
      localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(authData));
    }
  }

  private updateAuthTokens(authResponse: AuthResponse): void {
    localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(authResponse));
  }

  // Public methods
  setAuthData(authResponse: AuthResponse): void {
    this.updateAuthTokens(authResponse);
  }

  clearAuthData(): void {
    localStorage.removeItem('AuthenticatedUserResponse');
  }

  getCurrentUser(): AuthResponse | null {
    return this.getAuthData();
  }

  isAuthenticated(): boolean {
    const authData = this.getAuthData();
    return authData !== null && !!authData.jwtToken;
  }
}