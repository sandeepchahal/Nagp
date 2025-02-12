import { Component } from '@angular/core';
import { SearchService } from '../../../services/search.service';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SearchResponse } from '../../../models/searchResponse.model';

@Component({
  selector: 'app-suggestion',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './suggestion.component.html',
  styleUrl: './suggestion.component.css',
})
export class SuggestionComponent {
  searchQuery = '';
  suggestions: SearchResponse[] = [];
  searchSubject = new Subject<string>();

  constructor(private searchService: SearchService) {
    // Handle input changes with debounce
    this.searchSubject
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap((query) =>
          query.length >= 3 ? this.searchService.search(query) : []
        )
      )
      .subscribe((results) => {
        this.suggestions = results;
        console.log(this.suggestions);
      });
  }

  onInputChange() {
    this.searchSubject.next(this.searchQuery);
  }

  selectSuggestion(suggestion: any) {
    this.searchQuery = suggestion.text;
    console.log(suggestion);
    this.suggestions = [];
  }
  highlightMatch(text: string): string {
    if (!this.searchQuery) return text;
    const regex = new RegExp(`(${this.searchQuery})`, 'gi');
    return text.replace(regex, '<strong>$1</strong>');
  }
}
