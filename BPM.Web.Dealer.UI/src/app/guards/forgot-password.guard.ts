// forgot-password.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const forgotPasswordGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const loggedData = localStorage.getItem('AuthenticatedUserResponse');

  console.log('🔵 forgotPasswordGuard called');
  console.log('Current URL:', state.url);

  if (loggedData) {
    try {
      const authResponse = JSON.parse(loggedData);
      if (authResponse?.jwtToken) {
        // If already logged in, redirect to drugs-catalog
        console.log('✅ User already logged in, redirecting to drugs-catalog');
        router.navigateByUrl('/drugs-catalog');
        return false;
      }
    } catch (e) {
      console.error('Error parsing auth data:', e);
    }
  }

  // Allow access to forgot-password page
  console.log('✅ Allowing access to forgot-password');
  return true;
};