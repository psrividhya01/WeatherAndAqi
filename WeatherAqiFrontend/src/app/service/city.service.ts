import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CityItem {
  name: string;
  temp: number;
  desc?: string;
  range?: string;
}

@Injectable({ providedIn: 'root' })
export class CityService {
  private baseUrl = 'https://your-api-base-url.com/api'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  getCities(): Observable<CityItem[]> {
    return this.http.get<CityItem[]>(`${this.baseUrl}/cities`);
    // Expected response:
    // [{ name: 'Chennai', temp: 34, desc: 'Mostly clear', range: '27 ~ 35°C' }, ...]
  }
}
