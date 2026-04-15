import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SettingsService {
  readonly version = signal('V6.4.1.1');
  readonly tempUnit = signal<'Celsius (°C)' | 'Fahrenheit (°F)'>('Celsius (°C)');
  readonly hourlyInterval = signal<'2-hour interval' | '1-hour interval'>('2-hour interval');
  readonly forecastFormat = signal<'List' | 'Grid'>('List');

  setTempUnit(value: 'Celsius (°C)' | 'Fahrenheit (°F)') {
    this.tempUnit.set(value);
  }

  setHourlyInterval(value: '2-hour interval' | '1-hour interval') {
    this.hourlyInterval.set(value);
  }

  setForecastFormat(value: 'List' | 'Grid') {
    this.forecastFormat.set(value);
  }
}
