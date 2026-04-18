import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { WeatherService } from '../../services/weather.service';

@Component({
  selector: 'app-city-search',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  providers: [WeatherService],
  template: `
    <div class="search-container">
      <input
        type="text"
        placeholder="Search city..."
        [(ngModel)]="searchTerm"
        (keyup)="onSearch()"
        class="search-input"
        [disabled]="!isOnline"
      />
      <div *ngIf="!isOnline" class="offline-badge">Offline</div>
      <div *ngIf="results.length > 0" class="search-results">
        <div *ngFor="let result of results" (click)="selectCity(result)" class="result-item">
          {{ result }}
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .search-container {
        position: relative;
        margin-bottom: 1rem;
      }
      .search-input {
        width: 100%;
        padding: 0.75rem;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 1rem;
      }
      .search-input:disabled {
        background-color: #f5f5f5;
        cursor: not-allowed;
      }
      .offline-badge {
        position: absolute;
        right: 0.75rem;
        top: 50%;
        transform: translateY(-50%);
        background: #ff6b6b;
        color: white;
        padding: 0.25rem 0.75rem;
        border-radius: 4px;
        font-size: 0.75rem;
      }
      .search-results {
        position: absolute;
        top: 100%;
        left: 0;
        right: 0;
        background: white;
        border: 1px solid #ddd;
        max-height: 200px;
        overflow-y: auto;
        z-index: 10;
      }
      .result-item {
        padding: 0.75rem;
        cursor: pointer;
        border-bottom: 1px solid #eee;
      }
      .result-item:hover {
        background-color: #f0f0f0;
      }
    `,
  ],
})
export class CitySearchComponent implements OnInit {
  searchTerm = '';
  results: string[] = [];
  isOnline = navigator.onLine;
  private searchSubject = new Subject<string>();

  @Output() citySelected = new EventEmitter<string>();

  constructor(private weatherService: WeatherService) {
    window.addEventListener('online', () => (this.isOnline = true));
    window.addEventListener('offline', () => (this.isOnline = false));
  }

  ngOnInit(): void {
    this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe((term) => {
      if (term.length > 0 && this.isOnline) {
        this.results = [term]; // Simplified - could call API for suggestions
      }
    });
  }

  onSearch(): void {
    this.searchSubject.next(this.searchTerm);
  }

  selectCity(city: string): void {
    this.searchTerm = city;
    this.results = [];
    this.citySelected.emit(city);
  }
}
