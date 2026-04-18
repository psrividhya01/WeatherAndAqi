import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { WeatherService } from '../../services/weather.service';
import { Favorite } from '../../models/preferences.model';

@Component({
  selector: 'app-favorites-widget',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  providers: [WeatherService],
  template: `
    <div class="favorites-card">
      <h3>Favorite Cities</h3>
      <div *ngIf="favorites.length === 0" class="no-favorites">
        No favorites yet. Add your favorite cities!
      </div>
      <div *ngIf="favorites.length > 0" class="favorites-list">
        <div *ngFor="let fav of favorites" class="favorite-item" (click)="viewCity(fav.cityName)">
          <span class="city-name">{{ fav.cityName }}</span>
          <button (click)="removeFavorite($event, fav.cityName)" class="remove-btn">✕</button>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .favorites-card {
        background: white;
        border-radius: 8px;
        padding: 1.5rem;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
      }
      h3 {
        margin-top: 0;
        color: #333;
      }
      .no-favorites {
        text-align: center;
        color: #999;
        padding: 2rem 0;
        font-size: 0.9rem;
      }
      .favorites-list {
        display: flex;
        flex-wrap: wrap;
        gap: 0.5rem;
      }
      .favorite-item {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        padding: 0.5rem 1rem;
        border-radius: 20px;
        cursor: pointer;
        transition: opacity 0.2s;
      }
      .favorite-item:hover {
        opacity: 0.8;
      }
      .city-name {
        font-weight: 500;
      }
      .remove-btn {
        background: rgba(255, 255, 255, 0.3);
        border: none;
        color: white;
        cursor: pointer;
        border-radius: 50%;
        width: 24px;
        height: 24px;
        display: flex;
        align-items: center;
        justify-content: center;
        transition: background 0.2s;
      }
      .remove-btn:hover {
        background: rgba(255, 255, 255, 0.5);
      }
    `,
  ],
})
export class FavoritesWidgetComponent implements OnInit {
  favorites: Favorite[] = [];

  constructor(private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.loadFavorites();
  }

  loadFavorites(): void {
    // In real app: const userId = this.authService.getCurrentUserId();
    // this.weatherService.getFavorites(userId).subscribe(fav => this.favorites = fav);
    // For now, load from localStorage
    const stored = localStorage.getItem('favorites');
    this.favorites = stored ? JSON.parse(stored) : [];
  }

  viewCity(cityName: string): void {
    console.log('View city:', cityName);
    // Emit or navigate to city detail
  }

  removeFavorite(event: Event, cityName: string): void {
    event.stopPropagation();
    // In real app: this.weatherService.removeFavorite(userId, cityName).subscribe(() => ...)
    this.favorites = this.favorites.filter((f) => f.cityName !== cityName);
    localStorage.setItem('favorites', JSON.stringify(this.favorites));
  }
}
