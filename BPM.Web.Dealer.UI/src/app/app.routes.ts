import { Routes } from '@angular/router';
import { authenticationGuard } from './guards/authentication-guard';
import { LoginComponent } from './components/login/login.component';
import { DrugsCatelogComponent } from './components/drugs-catelog/drugs-catelog';

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
    path: '**',
    redirectTo: 'login'
  }
];