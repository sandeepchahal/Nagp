import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CartItem } from '../models/cart.model';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private cartItems: CartItem[] = [];
  private cartCountSubject = new BehaviorSubject<number>(0);
  private cartItemsSubject = new BehaviorSubject<CartItem[]>([]);

  // Observable streams for the components to subscribe to
  cartCount$ = this.cartCountSubject.asObservable();
  cartItems$ = this.cartItemsSubject.asObservable();

  // New BehaviorSubject for popup state and data
  private showPopupSubject = new BehaviorSubject<boolean>(false);
  private popupDataSubject = new BehaviorSubject<CartItem | null>(null);

  showPopup$ = this.showPopupSubject.asObservable(); // Observable for popup visibility
  popupData$ = this.popupDataSubject.asObservable(); // Observable for popup data

  constructor() {}

  // Add item to the cart
  addToCart(item: CartItem) {
    this.cartItems.push(item);
    this.cartCountSubject.next(this.cartItems.length);
    this.cartItemsSubject.next(this.cartItems);
    this.showPopup(item);
  }
  // Show the popup with data
  showPopup(item: CartItem) {
    this.popupDataSubject.next(item); // Set the popup data
    this.showPopupSubject.next(true); // Show the popup

    // Auto-hide the popup after 3 seconds
    setTimeout(() => {
      this.hidePopup();
    }, 3000);
  }

  // Hide the popup
  hidePopup() {
    this.showPopupSubject.next(false);
  }
}
