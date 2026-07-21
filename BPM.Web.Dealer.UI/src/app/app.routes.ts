import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DrugsCatelogComponent } from './components/drugs-catelog/drugs-catelog';
import { CartComponent } from './components/cart/cart.component';
import { authenticationGuard } from './guards/authentication-guard';
import { loginGuard } from './guards/login.guard';
import { ForgetPasswordComponent } from './components/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/reset-passwor/reset-passwor.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [loginGuard]
  },
  {
    path: 'forgot-password',
    component: ForgetPasswordComponent
  },
  {
    path: 'reset-password/:token',
    component: ResetPasswordComponent
  },
  {
    path: 'drugs-catalog',
    component: DrugsCatelogComponent,
    canActivate: [authenticationGuard]
  },
  {
    path: 'cart',
    component: CartComponent,
    canActivate: [authenticationGuard]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];