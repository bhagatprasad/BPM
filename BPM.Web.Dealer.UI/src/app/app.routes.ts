import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { authenticationGuard } from './guards/authentication-guard';
import { loginGuard } from './guards/login.guard';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { forgotPasswordGuard } from './guards/forgot-password.guard';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { roleGuard } from './guards/role.guard';

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
  // Admin Routes (Administrator role)
  {
    path: 'admin',
    canActivate: [authenticationGuard, roleGuard],
    data: { roles: ['Administrator', 'Admin'] },
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./components/admin/dashbaord.component').then((m) => m.DashBoardComponent),
      },
      {
        path: 'profile',
        loadComponent: () =>
          import('./components/profile/profile.component').then((m) => m.ProfileComponent),
      },
      {
        path: 'drugs',
        loadComponent: () =>
          import('./components/drugs-catelog/drugs-catelog').then((m) => m.DrugsCatelogComponent),
      },
      {
        path: 'cart',
        loadComponent: () =>
          import('./components/cart/cart.component').then((m) => m.CartComponent),
      },
      {
        path: 'my-orders',
        loadComponent: () =>
          import('./components/my-orders/my-orders.component').then((m) => m.MyOrdersComponent),
      },
      {
        path: 'users',
        loadComponent: () =>
          import('./components/user/user.component').then((m) => m.UserComponent),
      },
      {
        path: 'ware-house',
        loadComponent: () =>
          import('./components/ware-house/ware-house.component').then((a) => a.WarehouseComponent),
      },
      {
        path: '**',
        redirectTo: 'dashboard',
      },
    ],
  },
  // Operator Routes (Operator and User roles)
  {
    path: 'operator',
    canActivate: [authenticationGuard, roleGuard],
    data: { roles: ['Operator', 'User'] },
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./components/operator/dashbaord.component').then((m) => m.DashBoardComponent),
      },
      {
        path: 'profile',
        loadComponent: () =>
          import('./components/profile/profile.component').then((m) => m.ProfileComponent),
      },
      {
        path: 'drugs',
        loadComponent: () =>
          import('./components/drugs-catelog/drugs-catelog').then((m) => m.DrugsCatelogComponent),
      },
      {
        path: 'cart',
        loadComponent: () =>
          import('./components/cart/cart.component').then((m) => m.CartComponent),
      },
      {
        path: 'my-orders',
        loadComponent: () =>
          import('./components/my-orders/my-orders.component').then((m) => m.MyOrdersComponent),
      },
      {
        path: 'ware-house',
        loadComponent: () =>
          import('./components/ware-house/ware-house.component').then((b) => b.WarehouseComponent),
      },
      {
        path: '**',
        redirectTo: 'dashboard',
      },
    ],
  },
  // Fallback routes (for backward compatibility)
  {
    path: 'dashboard',
    canActivate: [authenticationGuard],
    loadComponent: () =>
      import('./components/admin/dashbaord.component').then((m) => m.DashBoardComponent),
  },
  {
    path: 'profile',
    loadComponent: () =>
      import('./components/profile/profile.component').then((m) => m.ProfileComponent),
    canActivate: [authenticationGuard],
  },
  {
    path: 'drugs',
    loadComponent: () =>
      import('./components/drugs-catelog/drugs-catelog').then((m) => m.DrugsCatelogComponent),
    canActivate: [authenticationGuard],
  },
  {
    path: 'cart',
    loadComponent: () => import('./components/cart/cart.component').then((m) => m.CartComponent),
    canActivate: [authenticationGuard],
  },
  {
    path: 'my-orders',
    loadComponent: () =>
      import('./components/my-orders/my-orders.component').then((m) => m.MyOrdersComponent),
    canActivate: [authenticationGuard],
  },
  {
    path: 'users',
    loadComponent: () => import('./components/user/user.component').then((m) => m.UserComponent),
    canActivate: [authenticationGuard],
  },
  {
    path: 'ware-house',
    loadComponent: () =>
      import('./components/ware-house/ware-house.component').then((a) => a.WarehouseComponent),
    canActivate: [authenticationGuard],
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
