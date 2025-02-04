import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { importProvidersFrom } from '@angular/core';
import { routes } from './app/app.routes';
import { provideRouter, RouterModule } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

const appConfig = {
  providers: [
    provideRouter(routes),
    importProvidersFrom(RouterModule.forRoot(routes)),
    provideHttpClient(), provideAnimationsAsync(),
  ],
};

bootstrapApplication(AppComponent, appConfig).catch((err) =>
  console.error(err)
);
