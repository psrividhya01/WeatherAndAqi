import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AqiCategoryPipe } from '../../pipes/aqi-category.pipe';

@Component({
  selector: 'app-aqi-score-card',
  standalone: true,
  imports: [CommonModule, AqiCategoryPipe],
  template: `
    <div class="aqi-card" [ngStyle]="{ 'border-left-color': categoryInfo.color }">
      <h3>Air Quality Index</h3>
      <div class="score-display">
        <svg class="gauge" viewBox="0 0 100 100">
          <circle cx="50" cy="50" r="45" fill="none" stroke="#e0e0e0" stroke-width="8" />
          <circle
            cx="50"
            cy="50"
            r="45"
            fill="none"
            [attr.stroke]="categoryInfo.color"
            stroke-width="8"
            stroke-dasharray="282.74"
            [style.stroke-dashoffset]="getStrokeDashoffset(aqiScore)"
          />
          <text x="50" y="50" text-anchor="middle" dy=".3em" font-size="24" font-weight="bold">
            {{ aqiScore }}
          </text>
        </svg>
      </div>
      <div class="category" [style.color]="categoryInfo.color">
        {{ (aqiScore | aqiCategory).category }}
      </div>
      <div class="details">
        <p>PM2.5: {{ pm25 }} µg/m³</p>
        <p>PM10: {{ pm10 }} µg/m³</p>
        <p>NO₂: {{ no2 }} ppb</p>
      </div>
    </div>
  `,
  styles: [
    `
      .aqi-card {
        background: white;
        border-left: 4px solid #667eea;
        border-radius: 8px;
        padding: 1.5rem;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
      }
      h3 {
        margin-top: 0;
        color: #333;
      }
      .score-display {
        display: flex;
        justify-content: center;
        margin: 1.5rem 0;
      }
      .gauge {
        width: 120px;
        height: 120px;
        transform: rotate(-90deg);
      }
      .category {
        text-align: center;
        font-size: 1.25rem;
        font-weight: bold;
        margin: 1rem 0;
      }
      .details {
        background: #f5f5f5;
        padding: 1rem;
        border-radius: 4px;
        font-size: 0.9rem;
      }
      .details p {
        margin: 0.5rem 0;
        color: #666;
      }
    `,
  ],
})
export class AqiScoreCardComponent {
  @Input() aqiScore: number = 0;
  @Input() pm25: number = 0;
  @Input() pm10: number = 0;
  @Input() no2: number = 0;

  categoryInfo = { color: '#2ecc71', bgColor: '#d4edda' };

  getStrokeDashoffset(score: number): number {
    const maxScore = 300;
    const circumference = 282.74;
    const offset = (score / maxScore) * circumference;
    return circumference - offset;
  }
}
