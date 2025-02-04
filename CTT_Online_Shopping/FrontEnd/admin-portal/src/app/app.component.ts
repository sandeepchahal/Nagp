import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLinkActive, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'admin-portal';
}
