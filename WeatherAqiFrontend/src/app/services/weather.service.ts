import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class WeatherService {
  private apiUrl = 'https://localhost:5001/api';

  constructor(private http: HttpClient) {}

  getCurrentWeather(city: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/weather/current?city=${city}`);
  }

  getHourlyForecast(city: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/weather/hourly?city=${city}`);
  }

  getDailyForecast(city: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/weather/forecast?city=${city}`);
  }

  getMultiCityWeather(cities: string[]): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/weather/multi?cities=${cities.join(',')}`);
  }

  getNearbyCities(lat: number, lon: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/weather/nearby?lat=${lat}&lon=${lon}`);
  }

  getMultiAQI(cities: string[]): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/aqi/multi?cities=${cities.join(',')}`);
  }

  getSimilarCities(temp: number, condition: string, humidity: string): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.apiUrl}/weather/similar?temp=${temp}&condition=${condition}&humidity=${humidity}`,
    );
  }

  getAQITrend(city: string, days: number = 7): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/aqi/trend?city=${city}&days=${days}`);
  }

  getHealthAdvisory(aqi: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/aqi/advisory?aqi=${aqi}`);
  }

  getAlerts(city: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/alerts/active?city=${city}`);
  }

  getFavorites(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/favorites?userId=${userId}`);
  }

  addFavorite(userId: number, cityName: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/favorites`, { userId, cityName });
  }

  removeFavorite(userId: number, cityName: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/favorites/${cityName}?userId=${userId}`);
  }

  getPreferences(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/preferences?userId=${userId}`);
  }

  savePreferences(preferences: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/preferences`, preferences);
  }
}
