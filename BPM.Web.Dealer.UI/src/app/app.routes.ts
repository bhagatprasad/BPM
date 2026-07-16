import { Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { DrugComponent } from './Admin/Components/drug/drug';


export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: HomeComponent
  },
 {
    path: 'drug',
    component: DrugComponent
  }
];