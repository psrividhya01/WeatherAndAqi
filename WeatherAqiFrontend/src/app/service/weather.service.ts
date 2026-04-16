import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CurrentWeather, HourlyForecastItem, DailyForecastItem } from '../models/weather.model';

@Injectable({ providedIn: 'root' })
export class WeatherService {
  private baseUrl = 'https://your-api-base-url.com/api'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  getCurrentWeather(): Observable<CurrentWeather> {
    return this.http.get<CurrentWeather>(`${this.baseUrl}/current`);
  }

  getHourlyForecast(): Observable<HourlyForecastItem[]> {
    return this.http.get<HourlyForecastItem[]>(`${this.baseUrl}/hourly`);
  }

  getDailyForecast(): Observable<DailyForecastItem[]> {
    return this.http.get<DailyForecastItem[]>(`${this.baseUrl}/daily`);
  }
}
