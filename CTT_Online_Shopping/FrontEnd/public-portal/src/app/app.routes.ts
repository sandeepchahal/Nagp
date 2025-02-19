import { Routes } from '@angular/router';
import { ListProductComponent } from './components/product/list-product/list-product.component';
import { DetailProductComponent } from './components/product/detail-product/detail-product.component';
import { ListCartComponent } from './components/cart/list-cart/list-cart.component';
import { BillingInformationComponent } from './components/order/billing-information/billing-information.component';
import { PaymentComponent } from './components/order/payment/payment.component';
import { LoginComponent } from './components/user/login/login.component';
import { ReviewComponent } from './components/order/review/review.component';
import { HomeComponent } from './components/home/home/home.component';
import { FilterProductComponent } from './components/product/filter-product/filter-product.component';
import { ConfirmOrderComponent } from './components/order/confirm-order/confirm-order.component';
import { ListOrderComponent } from './components/order/list-order/list-order.component';
import { WishlistComponent } from './components/user/wishlist/wishlist.component';
import { DetailOrderComponent } from './components/order/detail-order/detail-order.component';

export const routes: Routes = [
  { path: 'products', component: FilterProductComponent },
  { path: 'product/category/:slug', component: ListProductComponent },
  { path: 'product/item/:id', component: DetailProductComponent },

  { path: 'cart', component: ListCartComponent },

  { path: 'order/billing', component: BillingInformationComponent },
  { path: 'order/review', component: ReviewComponent },
  { path: 'order/payment', component: PaymentComponent },
  { path: 'order/confirmation', component: ConfirmOrderComponent },
  { path: 'order/list', component: ListOrderComponent },
  { path: 'order/detail/:id', component: DetailOrderComponent },

  { path: 'user/login', component: LoginComponent },
  { path: 'user/wishlist', component: WishlistComponent },

  { path: '', component: HomeComponent },
];
