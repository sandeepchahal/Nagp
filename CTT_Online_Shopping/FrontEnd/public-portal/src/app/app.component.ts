import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { MainHeaderComponent } from './components/header/main-header/main-header.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MainHeaderComponent, CommonModule, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'public-portal';
}
