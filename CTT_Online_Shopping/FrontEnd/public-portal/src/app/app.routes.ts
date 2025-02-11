import { Routes } from '@angular/router';
import { ListProductComponent } from './components/product/list-product/list-product.component';
import { DetailProductComponent } from './components/product/detail-product/detail-product.component';
import { ListCartComponent } from './components/cart/list-cart/list-cart.component';
import { BillingInformationComponent } from './components/order/billing-information/billing-information.component';
import { PaymentComponent } from './components/order/payment/payment.component';
import { LoginComponent } from './components/user/login/login.component';

export const routes: Routes = [
  { path: 'product/category/:slug', component: ListProductComponent },
  { path: 'product/item/:id', component: DetailProductComponent },
  { path: 'cart', component: ListCartComponent },
  { path: 'order/billing', component: BillingInformationComponent },
  { path: 'order/payment', component: PaymentComponent },
  { path: 'user/login', component: LoginComponent },
];
