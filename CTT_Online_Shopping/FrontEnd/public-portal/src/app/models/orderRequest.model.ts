import { CartItem } from './cart.model';

export interface OrderRequest extends CartItem {
  orderCount: number;
}
