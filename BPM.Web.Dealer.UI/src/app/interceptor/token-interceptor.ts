import { HttpInterceptorFn } from '@angular/common/http';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const loggeddata = localStorage.getItem('AuthenticatedUserResponse');
  const authResponse = loggeddata ? JSON.parse(loggeddata) : null;
  const token = authResponse?.jwtToken;

  const newReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
    },
  });
  return next(newReq); 
};
