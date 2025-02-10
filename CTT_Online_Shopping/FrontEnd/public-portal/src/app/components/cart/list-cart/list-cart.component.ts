import { Component } from '@angular/core';
import { CartItem } from '../../../models/cart.model';
import { CartService } from '../../../services/cart.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-list-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './list-cart.component.html',
  styleUrl: './list-cart.component.css',
})
export class ListCartComponent {
  cartItems: CartItem[] = [];
  cartCount: number = 0;
  totalPrice: number = 0;
  shipping: number = 10; // Example shipping cost

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.cartService.getCartItems().subscribe((items) => {
      this.cartItems = items;
      this.cartCount = items.length;
      this.calculateTotal();
    });
  }

  removeItem(item: CartItem): void {
    this.cartService.removeItem(item);
    this.calculateTotal();
  }

  // Increase quantity
  increaseQuantity(item: CartItem): void {
    this.cartService.updateCartItem(item, true); // Increment order count
    this.calculateTotalPrice(); // Recalculate total price
  }

  // Decrease quantity
  decreaseQuantity(item: CartItem): void {
    this.cartService.updateCartItem(item, false); // Decrement order count
    this.calculateTotalPrice(); // Recalculate total price
  }

  // Calculate the total price of all items in the cart
  calculateTotalPrice(): void {
    this.totalPrice = this.cartItems.reduce(
      (acc, item) => acc + item.totalPrice,
      0
    );
  }

  calculateTotal(): void {
    this.totalPrice = this.cartItems.reduce((acc, item) => {
      return acc + item.discountedPrice * item.orderCount;
    }, 0);
  }

  placeOrder(): void {
    console.log('Placing order with the following items:', this.cartItems);
    // Logic to proceed with order placement
  }
}
