import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authenticationGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const loggeddata = localStorage.getItem('AuthenticatedUserResponse');

  const authResponse = loggeddata ? JSON.parse(loggeddata) : null;
 if (
  !authResponse ||
  !authResponse.jwtToken ||
  (
    authResponse.authenticateResponseDto.roleId !== 'c0053da1-b23b-49ff-bf56-ea5db20949b1' &&
    authResponse.authenticateResponseDto.roleId !== '3170212b-4d51-4690-ab1e-c96fd4859bca' &&
    authResponse.authenticateResponseDto.roleId !== '03bcb674-1347-4a33-8ded-d933e135d14d'
  ))
   {
    router.navigateByUrl('login');
    return false;
  }
  return true;
};
// import { CanActivateFn } from '@angular/router';

// export const authenticationGuard: CanActivateFn = () => {
//   return true;
// };
