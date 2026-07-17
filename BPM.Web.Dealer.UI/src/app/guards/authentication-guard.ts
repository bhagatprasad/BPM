import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authenticationGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);
  const loggeddata = localStorage.getItem('AuthenticatedUserResponse');

  const authResponse = loggeddata ? JSON.parse(loggeddata) : null;
  if (!authResponse || !authResponse.jwtToken || authResponse.authenticateResponseDto.roleId !==
    "c0053da1-b23b-49ff-bf56-ea5db20949b1") {
    router.navigateByUrl('login');
    return false;
  }
  return true;
};
// import { CanActivateFn } from '@angular/router';

// export const authenticationGuard: CanActivateFn = () => {
//   return true;
// };
