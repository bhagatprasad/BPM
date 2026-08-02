import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const forgotPasswordGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const loggedData = localStorage.getItem('AuthenticatedUserResponse');

  console.log('🔵 forgotPasswordGuard called');
  console.log('Current URL:', state.url);

  // Check if there's any auth data
  if (loggedData) {
    try {
      const authResponse = JSON.parse(loggedData);
      
      // Validate that the token exists and is not expired
      if (authResponse?.jwtToken) {
        // Optional: Add token expiration check
        // You can decode JWT and check exp claim if needed
        
        // If already logged in, redirect to drugs-catalog
        console.log('✅ User already logged in, redirecting to drugs-catalog');
        router.navigateByUrl('/drugs-catalog');
        return false;
      } else {
        // Invalid auth data - clear it
        console.warn('⚠️ Invalid auth data found, clearing...');
        localStorage.removeItem('AuthenticatedUserResponse');
      }
    } catch (e) {
      console.error('❌ Error parsing auth data:', e);
      // Clear invalid data
      localStorage.removeItem('AuthenticatedUserResponse');
    }
  }

  // Allow access to forgot-password page
  console.log('✅ Allowing access to forgot-password');
  return true;
};