import { Routes } from '@angular/router';
import { ListProductComponent } from './components/product/list-product/list-product.component';

export const routes: Routes = [
  { path: 'product/category/:slug', component: ListProductComponent },
];
