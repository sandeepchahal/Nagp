import { Routes } from '@angular/router';
import { AddCategoryComponent } from './components/category/add-category/add-category.component';
import { ListCategoryComponent } from './components/category/list-category/list-category.component';

export const routes: Routes = [
  { path: 'category', component: ListCategoryComponent },
  { path: 'category/add', component: AddCategoryComponent },
  { path: '', redirectTo: '/category', pathMatch: 'full' },
];
