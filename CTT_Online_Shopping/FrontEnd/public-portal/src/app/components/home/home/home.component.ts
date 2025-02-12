import { Component } from '@angular/core';
import { SuggestionComponent } from '../../search/suggestion/suggestion.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SuggestionComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {}
