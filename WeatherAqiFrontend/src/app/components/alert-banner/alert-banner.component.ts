import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { WeatherService } from '../../services/weather.service';
import { Alert } from '../../models/alert.model';

@Component({
  selector: 'app-alert-banner',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  providers: [WeatherService],
  template: `
    <div *ngIf="alerts.length > 0" class="alert-banner">
      <div *ngFor="let alert of alerts" [class]="'alert alert-' + alert.severity.toLowerCase()">
        <div class="alert-header">
          <strong>{{ alert.alertType }}</strong>
          <button class="close-btn" (click)="dismissAlert(alert.alertId)">✕</button>
        </div>
        <p class="alert-message">{{ alert.message }}</p>
        <small class="alert-city">{{ alert.city }} - {{ alert.createdAt | date: 'short' }}</small>
      </div>
    </div>
  `,
  styles: [
    `
      .alert-banner {
        margin-bottom: 1rem;
      }
      .alert {
        padding: 1rem;
        border-radius: 4px;
        margin-bottom: 0.5rem;
        border-left: 4px solid;
      }
      .alert-low {
        background-color: #d4edda;
        border-left-color: #2ecc71;
      }
      .alert-medium {
        background-color: #fff3cd;
        border-left-color: #f39c12;
      }
      .alert-high {
        background-color: #f8d7da;
        border-left-color: #e74c3c;
      }
      .alert-critical {
        background-color: #f5c2c7;
        border-left-color: #c0392b;
      }
      .alert-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 0.5rem;
      }
      .close-btn {
        background: none;
        border: none;
        cursor: pointer;
        font-size: 1.25rem;
        opacity: 0.6;
        transition: opacity 0.2s;
      }
      .close-btn:hover {
        opacity: 1;
      }
      .alert-message {
        margin: 0.5rem 0;
        color: #333;
      }
      .alert-city {
        display: block;
        color: #666;
        margin-top: 0.25rem;
      }
    `,
  ],
})
export class AlertBannerComponent implements OnInit {
  alerts: Alert[] = [];
  private dismissedAlerts = new Set<string>();

  constructor(private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.loadAlerts();
  }

  loadAlerts(): void {
    // In real app: call weatherService.getAlerts()
    // For now, demonstrate with localStorage
    const stored = localStorage.getItem('alerts');
    this.alerts = stored
      ? JSON.parse(stored).filter((a: Alert) => !this.dismissedAlerts.has(a.alertId))
      : [];
  }

  dismissAlert(alertId: string): void {
    this.dismissedAlerts.add(alertId);
    this.alerts = this.alerts.filter((a) => a.alertId !== alertId);
  }
}
