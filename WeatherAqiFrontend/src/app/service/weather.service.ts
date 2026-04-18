import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  
  private apiUrl = 'https://localhost:5001/api'; 

  constructor(private http: HttpClient) {}

  // ADD 'city: string' HERE
  getCurrentWeather(city: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/weather/current?city=${city}`);
  }

  // ADD 'city: string' HERE
  getHourlyForecast(city: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/weather/hourly?city=${city}`);
  }

  // ADD 'city: string' HERE
  getDailyForecast(city: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/weather/daily?city=${city}`);
  }

  getMultiCityWeather(cities: string[]): Observable<any[]> {
    const cityQuery = cities.join(',');
    return this.http.get<any[]>(`${this.apiUrl}/weather/multi?cities=${cityQuery}`);
  }

  getNearbyCities(lat: number, lon: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/weather/nearby?lat=${lat}&lon=${lon}`);
  }

  getMultiAQI(cities: string[]): Observable<any[]> {
    const cityQuery = cities.join(',');
    return this.http.get<any[]>(`${this.apiUrl}/weather/aqi/multi?cities=${cityQuery}`);
  }

  getSimilarCities(temp: number, condition: string, humidity: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/weather/similar?temp=${temp}&condition=${condition}&humidity=${humidity}`);
  }

  getAlerts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/alerts`);
  }

  getFavorites(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/favorites`);
  }

  getDigest(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/weather/digest`);
  }
}