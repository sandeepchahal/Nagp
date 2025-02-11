import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-billing-information',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './billing-information.component.html',
  styleUrl: './billing-information.component.css',
})
export class BillingInformationComponent {
  billingForm!: FormGroup;
  shippingForm!: FormGroup;
  isLoggedIn = false;
  guestCheckout = false;
  shippingDifferent = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Check if user is logged in
    this.isLoggedIn = this.authService.isAuthenticated();

    this.billingForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      zip: ['', Validators.required],
      country: ['', Validators.required],
      termsAccepted: [false, Validators.requiredTrue],
    });

    this.shippingForm = this.fb.group({
      shipAddress: ['', Validators.required],
      shipCity: ['', Validators.required],
      shipZip: ['', Validators.required],
      shipCountry: ['', Validators.required],
    });

    // If logged in, fetch user details
    if (this.isLoggedIn) {
      this.userService.getUserDetails().subscribe((user) => {
        this.billingForm.patchValue({
          fullName: user.fullName,
          email: user.email,
          phone: user.phone,
          address: user.address,
          city: user.city,
          zip: user.zip,
          country: user.country,
        });
      });
    }
  }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }

  continueAsGuest(): void {
    this.guestCheckout = true;
  }

  toggleShippingAddress(event: any): void {
    this.shippingDifferent = event.target.checked;
  }

  isFormValid(): boolean {
    if (!this.billingForm.valid || !this.billingForm.value.termsAccepted) {
      return false;
    }

    if (this.shippingDifferent && !this.shippingForm.valid) {
      return false;
    }

    return true;
  }

  proceedToCheckout(): void {
    console.log(
      'Proceeding with:',
      this.billingForm.value,
      this.shippingForm.value
    );
    this.router.navigate(['/order/review']);
  }
  login() {}
}
