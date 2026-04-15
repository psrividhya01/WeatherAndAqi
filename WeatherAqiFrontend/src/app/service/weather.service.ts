import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class WeatherService {
  private baseUrl = 'https://your-api-base-url.com/api'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  getCurrentWeather(): Observable<any> {
    return this.http.get(`${this.baseUrl}/current`);
    // Expected response:
    // { city, currentTemp, condition, tempMin, tempMax, feelsLike }
  }

  getHourlyForecast(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/hourly`);
    // Expected response:
    // [{ time: '14:00', icon: '⛅', active: true }, ...]
  }

  getDailyForecast(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/daily`);
    // Expected response:
    // [{ label: 'Today', icon: '☀️', min: 27, max: 35, isToday: true }, ...]
  }
}
