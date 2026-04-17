import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

// Define the shape of our settings
export interface WidgetConfig {
  showAqiGauge: boolean;
  showHealthAdvisory: boolean;
  showPollutantBreakdown: boolean;
  showAqiTrendChart: boolean;
  showForecastCard: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class WeatherStateService {
  
  // 1. Initial Default State
  private defaultConfig: WidgetConfig = {
    showAqiGauge: true,
    showHealthAdvisory: true,
    showPollutantBreakdown: true,
    showAqiTrendChart: true,
    showForecastCard: true
  };

  // 2. The BehaviorSubject holds the current state and emits it to subscribers
  private widgetConfigSubject = new BehaviorSubject<WidgetConfig>(this.defaultConfig);
  
  // 3. Components will subscribe to this Observable
  widgetConfig$ = this.widgetConfigSubject.asObservable();

  constructor() {}

  // 4. Method to update specific settings
  updateWidgetConfig(newConfig: Partial<WidgetConfig>) {
    const currentState = this.widgetConfigSubject.value;
    // Merge the old state with the new changes, then emit it
    this.widgetConfigSubject.next({ ...currentState, ...newConfig });
  }
}