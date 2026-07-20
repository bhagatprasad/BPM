import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DrugsCatelogComponent } from './components/drugs-catelog/drugs-catelog';
import { CartComponent } from './components/cart/cart';
import { authenticationGuard } from './guards/authentication-guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
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