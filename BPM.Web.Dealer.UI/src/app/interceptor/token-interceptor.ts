import { HttpInterceptorFn } from '@angular/common/http';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const loggeddata = localStorage.getItem('AuthenticatedUserResponse');
  const authResponse = loggeddata ? JSON.parse(loggeddata) : null;
  const token = authResponse?.jwtToken;

  // Only add Authorization header if token exists
  if (token) {
    const newReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
    return next(newReq);
  }
  
  return next(req);
};