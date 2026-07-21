import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const loginGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const loggeddata = localStorage.getItem('AuthenticatedUserResponse');

  if (loggeddata) {
    try {
      const authResponse = JSON.parse(loggeddata);
      if (authResponse?.jwtToken) {
        // User is already authenticated, redirect to drugs-catalog
        router.navigateByUrl('/drugs-catalog');
        return false;
      }
    } catch (e) {
      console.error('Error parsing auth data:', e);
    }
  }

  // Allow access to login page
  return true;
};