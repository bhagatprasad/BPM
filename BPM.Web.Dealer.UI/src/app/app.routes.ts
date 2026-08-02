import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { authenticationGuard } from './guards/authentication-guard';
import { loginGuard } from './guards/login.guard';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { forgotPasswordGuard } from './guards/forgot-password.guard';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [loginGuard],
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent,
    canActivate: [forgotPasswordGuard],
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent,
  },
  {
    path: 'profile',
    loadComponent: () => import('./components/profile/profile.component').then(m => m.ProfileComponent),
    canActivate: [authenticationGuard]
  },
  {
    path: 'drugs',
    loadComponent: () => import('./components/drugs-catelog/drugs-catelog').then(m => m.DrugsCatelogComponent),
    canActivate: [authenticationGuard],
  },
  {
    path: 'cart',
    loadComponent: () => import('./components/cart/cart.component').then(m => m.CartComponent),
    canActivate: [authenticationGuard],
  },
  {
    path: 'my-orders',
    loadComponent: () => import('./components/my-orders/my-orders.component').then(m => m.MyOrdersComponent),
    canActivate: [authenticationGuard],
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];