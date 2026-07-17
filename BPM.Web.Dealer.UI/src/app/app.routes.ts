import { Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { DrugsCatelogComponent } from './components/drugs-catelog/drugs-catelog';
import { LoginComponent } from './components/login/login';
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
    path: 'drugs-catelog',
    component: DrugsCatelogComponent,
    canActivate: [authenticationGuard]
  }
  
 
];