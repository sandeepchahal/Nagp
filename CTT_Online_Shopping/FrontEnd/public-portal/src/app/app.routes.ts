import { Routes } from '@angular/router';
import { ListProductComponent } from './components/product/list-product/list-product.component';
import { DetailProductComponent } from './components/product/detail-product/detail-product.component';
import { ListCartComponent } from './components/cart/list-cart/list-cart.component';

export const routes: Routes = [
  { path: 'product/category/:slug', component: ListProductComponent },
  { path: 'product/item/:id', component: DetailProductComponent },
  { path: 'cart', component: ListCartComponent },
];
