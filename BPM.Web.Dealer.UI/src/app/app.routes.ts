import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DrugsCatelogComponent } from './components/drugs-catelog/drugs-catelog';
import { CartComponent } from './components/cart/cart.component';
import { authenticationGuard } from './guards/authentication-guard';
import { loginGuard } from './guards/login.guard';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { forgotPasswordGuard } from './guards/forgot-password.guard'
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ProfileComponent } from './components/profile/profile.component';
import { MyOrdersComponent } from './components/my-orders/my-orders.component';

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
    component: ProfileComponent,
    canActivate: [authenticationGuard]
  },
  {
    path: 'drugs',
    component: DrugsCatelogComponent,
    canActivate: [authenticationGuard],
  },
  {
    path: 'cart',
    component: CartComponent,
    canActivate: [authenticationGuard],
  },
  {
    path: 'my-orders',
    component: MyOrdersComponent,
    canActivate: [authenticationGuard],
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];