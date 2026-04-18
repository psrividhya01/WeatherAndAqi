import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UnitConvertPipe } from '../../pipes/unit-convert.pipe';

@Component({
  selector: 'app-forecast-cards',
  standalone: true,
  imports: [CommonModule, UnitConvertPipe],
  template: `
    <div class="forecast-container">
      <h3>7-Day Forecast</h3>
      <div class="cards-scroll">
        <div *ngFor="let day of forecast" class="forecast-card">
          <div class="date">{{ day.dt | date: 'EEE, MMM d' }}</div>
          <div class="icon">{{ day.weather[0]?.main }}</div>
          <div class="temp">
            <span class="high">{{ day.temp.max | unitConvert: 'celsius' : unit }}</span
            >° <span class="low">{{ day.temp.min | unitConvert: 'celsius' : unit }}</span
            >°
          </div>
          <div class="precipitation">💧 {{ day.pop * 100 }}%</div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .forecast-container {
        margin-top: 2rem;
      }
      h3 {
        margin-bottom: 1rem;
        color: #333;
      }
      .cards-scroll {
        display: flex;
        overflow-x: auto;
        gap: 1rem;
        padding-bottom: 1rem;
      }
      .forecast-card {
        flex: 0 0 120px;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        padding: 1rem;
        border-radius: 8px;
        text-align: center;
        cursor: pointer;
        transition: transform 0.2s;
      }
      .forecast-card:hover {
        transform: translateY(-4px);
      }
      .date {
        font-size: 0.75rem;
        opacity: 0.9;
        margin-bottom: 0.5rem;
      }
      .icon {
        font-size: 1.5rem;
        margin: 0.5rem 0;
      }
      .temp {
        font-size: 1rem;
        font-weight: bold;
      }
      .high {
        color: #ffeb3b;
      }
      .low {
        opacity: 0.8;
      }
      .precipitation {
        font-size: 0.75rem;
        margin-top: 0.5rem;
      }
    `,
  ],
})
export class ForecastCardsComponent {
  @Input() forecast: any[] = [];
  @Input() unit: 'metric' | 'imperial' = 'metric';
}
