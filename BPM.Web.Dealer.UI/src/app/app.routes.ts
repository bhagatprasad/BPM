import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DrugsCatelogComponent } from './components/drugs-catelog/drugs-catelog';
import { CartComponent } from './components/cart/cart.component';
import { authenticationGuard } from './guards/authentication-guard';
import { loginGuard } from './guards/login.guard';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ProfileComponent } from './components/profile/profile.component';


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
    component: ForgotPasswordComponent,
    // No guard needed - App component handles public routes
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent,
    // No guard needed - App component handles public routes
  },
  {
    path:'Profile',
    component: ProfileComponent,
   canActivate: [authenticationGuard]
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
