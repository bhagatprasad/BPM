import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AccountService } from '../services/account.service';
import { ToastrService } from '@iqx-limited/ngx-toastr';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  const toastr = inject(ToastrService);
  
  // Get token from AccountService
  const token = accountService.getToken();

  // Clone request with Authorization header if token exists
  let authReq = req;
  if (token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json',
      },
    });
  }

  // Handle response errors
  return next(authReq).pipe(
    catchError((error) => {
      // Handle 401 Unauthorized - token expired or invalid
      if (error.status === 401) {
        console.warn('⚠️ Unauthorized request, clearing session...');
        accountService.logout();
        toastr.error('Your session has expired. Please login again.', 'Session Expired');
        router.navigate(['/login']);
      }
      
      // Handle 403 Forbidden
      if (error.status === 403) {
        toastr.error('You do not have permission to perform this action.', 'Access Denied');
      }
      
      // Handle 500 Internal Server Error
      if (error.status === 500) {
        toastr.error('An internal server error occurred. Please try again later.', 'Server Error');
      }
      
      return throwError(() => error);
    })
  );
};