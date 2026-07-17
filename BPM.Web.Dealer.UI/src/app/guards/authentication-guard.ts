import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authenticationGuard: CanActivateFn = (route, state) => {

const router = inject(Router);
  const loggeddata= localStorage.getItem('user');
  if (!loggeddata) {
    router.navigateByUrl('login');
    return false;
  }
  return true;
};
