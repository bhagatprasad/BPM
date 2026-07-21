import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authenticationGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const loggeddata = localStorage.getItem('AuthenticatedUserResponse');

  if (loggeddata) {
    try {
      const authResponse = JSON.parse(loggeddata);
      if (authResponse?.jwtToken) {
        return true;
      }
    } catch (e) {
      console.error('Error parsing auth data:', e);
    }
  }

  router.navigateByUrl('login');
  return false;
};