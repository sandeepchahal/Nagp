import { Routes } from '@angular/router';
import { AddCategoryComponent } from './components/category/add-category/add-category.component';
import { ListCategoryComponent } from './components/category/list-category/list-category.component';
import { EditCategoryComponent } from './components/category/edit-category/edit-category.component';
import { AddProductComponent } from './components/product/add-product/add-product.component';
import { ListProductComponent } from './components/product/list-product/list-product.component';
import { EditProductComponent } from './components/product/edit-product/edit-product.component';
import { DetailProductComponent } from './components/product/detail-product/detail-product.component';
import { AddProductItemComponent } from './components/productItem/add-product-item/add-product-item.component';
import { ListProductItemComponent } from './components/productItem/list-product-item/list-product-item.component';
import { DetailProductItemComponent } from './components/productItem/detail-product-item/detail-product-item.component';
import { ListBrandComponent } from './components/brand/list-brand/list-brand.component';
import { AddBrandComponent } from './components/brand/add-brand/add-brand.component';

export const routes: Routes = [
  { path: 'brands', component: ListBrandComponent },
  { path: 'brand/add', component: AddBrandComponent },

  { path: 'category', component: ListCategoryComponent },
  { path: 'category/add', component: AddCategoryComponent },
  { path: 'edit-category/:id', component: EditCategoryComponent },

  { path: 'product', component: ListProductComponent },
  { path: 'product/add', component: AddProductComponent },
  { path: 'product/edit/:id', component: EditProductComponent },
  { path: 'product/details/:id', component: DetailProductComponent },

  { path: 'product/item/add/:id', component: AddProductItemComponent },
  { path: 'product/items', component: ListProductItemComponent },
  { path: 'product/item/detail/:id', component: DetailProductItemComponent },

  { path: '', redirectTo: '/category', pathMatch: 'full' },
];
