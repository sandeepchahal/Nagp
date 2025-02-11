import { Component } from '@angular/core';
import { User } from '../../../models/user.model';
import { Router } from '@angular/router';
import { CheckoutService } from '../../../services/checkout.service';
import { CartItem } from '../../../models/cart.model';
import { CartService } from '../../../services/cart.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-review',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './review.component.html',
  styleUrl: './review.component.css',
})
export class ReviewComponent {
  user: User;
  cartItems: CartItem[] = [];
  totalPrice: number = 0;
  shipping: number = 10;
  constructor(
    private router: Router,
    private checkoutService: CheckoutService,
    private cartService: CartService
  ) {
    this.user = this.checkoutService.getUserData();
    console.log('user', this.user);
    if (this.user != null) {
      console.log(this.user);
      this.getCartItems();
    } else {
      this.router.navigate(['cart']);
    }
  }

  getCartItems() {
    this.cartService.getCartItems().subscribe((items) => {
      this.cartItems = items;
      console.log(this.cartItems);
      this.calculateTotalPrice();
    });
  }
  calculateTotalPrice(): void {
    this.totalPrice = this.cartItems.reduce(
      (sum, item) =>
        sum +
        (item.discountedPrice !== 0
          ? item.discountedPrice * item.orderCount
          : item.price * item.orderCount),
      0
    );
  }
  placeOrder() {}
}
