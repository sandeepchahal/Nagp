import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { importProvidersFrom } from '@angular/core';
import { routes } from './app/app.routes';
import { RouterModule } from '@angular/router';

const appConfig = {
  providers: [
    importProvidersFrom(RouterModule.forRoot(routes)), // Adding RouterModule with routes
    // Add other necessary providers here
  ],
};

bootstrapApplication(AppComponent, appConfig).catch((err) =>
  console.error(err)
);
