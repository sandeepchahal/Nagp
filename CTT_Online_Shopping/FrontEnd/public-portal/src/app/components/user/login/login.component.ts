import { Component } from '@angular/core';
import {
  SocialAuthService,
  SocialUser,
  GoogleLoginProvider,
  SocialAuthServiceConfig,
  GoogleSigninButtonModule,
} from '@abacritt/angularx-social-login';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    GoogleSigninButtonModule,
  ],
  templateUrl: './login.component.html',
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
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginForm!: FormGroup;
  socialUser: SocialUser | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    public socialAuthService: SocialAuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });

    // Subscribe to social auth state changes
    this.socialAuthService.authState.subscribe((user: SocialUser) => {
      this.socialUser = user;
      if (user) {
        this.handleSocialLogin(user);
      }
    });
  }

  // Handle Google Sign-In
  handleSocialLogin(user: SocialUser): void {
    this.authService.googleLogin(user.idToken).subscribe(
      (response) => {
        console.log('Google login success', response);
        // Store token and navigate user
        localStorage.setItem('token', response.token); // Save the JWT token
        this.router.navigate(['order/billing']); // Navigate to billing page validate-social-login
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
        // Store token and navigate user
        localStorage.setItem('token', response.token); // Save the JWT token
        this.authService.googleLogin(response.token);
        this.router.navigate(['/billing']); // Navigate to billing page
      },
      (error) => {
        console.error('Login error', error);
      }
    );
  }
  socialServiceLogin() {
    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  }
}
