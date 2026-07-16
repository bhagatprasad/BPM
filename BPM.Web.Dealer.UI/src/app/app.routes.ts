import { Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { DrugsCatelogComponent } from './components/drugs-catelog/drugs-catelog';


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
    path: 'drugs-catelog',
    component: DrugsCatelogComponent
  }
];