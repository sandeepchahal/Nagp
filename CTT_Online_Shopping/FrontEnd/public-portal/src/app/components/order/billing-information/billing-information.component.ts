import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { UserService } from '../../../services/user.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import {
  GoogleLoginProvider,
  SocialAuthServiceConfig,
  SocialLoginModule,
} from '@abacritt/angularx-social-login';

import { GoogleSigninButtonModule } from '@abacritt/angularx-social-login';

import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';

@Component({
  selector: 'app-billing-information',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    SocialLoginModule,
    GoogleSigninButtonModule,
  ],
  templateUrl: './billing-information.component.html',
  styleUrl: './billing-information.component.css',
  providers: [
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(
              '2200065800-4qpttr0st49m03cae11a63qniop7pqgn.apps.googleusercontent.com'
            ), // Replace with your Google Client ID
          },
        ],
      } as SocialAuthServiceConfig,
    },
    SocialAuthService, // Provide the SocialAuthService
  ],
})
export class BillingInformationComponent {
  billingForm!: FormGroup;
  shippingForm!: FormGroup;
  isLoggedIn = false;
  guestCheckout = false;
  shippingDifferent = false;
  loginForm!: FormGroup;
  socialUser: SocialUser | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userService: UserService,
    public socialAuthService: SocialAuthService,
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

    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
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
  // Handle Google Sign-In
  handleSocialLogin(user: SocialUser): void {
    this.authService.googleLogin(user.idToken).subscribe(
      (response) => {
        console.log('Google login success', response);
        // Store token and update login state
        localStorage.setItem('token', response.token);
        this.isLoggedIn = true;
      },
      (error) => {
        console.error('Google login error', error);
      }
    );
  }
  // Form-based sign-in
  onSubmit(): void {
    const formData = this.loginForm.value;
    this.authService.login(formData).subscribe(
      (response) => {
        console.log('Login success', response);
        // Store token and update login state
        localStorage.setItem('token', response.token);
        this.isLoggedIn = true;
      },
      (error) => {
        console.error('Login error', error);
      }
    );
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

  // Navigate to login page
  navigateToLogin(): void {
    this.router.navigate(['user/login']);
  }

  socialServiceLogin() {
    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  }
}
