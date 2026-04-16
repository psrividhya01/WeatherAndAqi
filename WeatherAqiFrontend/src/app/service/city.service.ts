import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { City } from '../models/city.model';

@Injectable({ providedIn: 'root' })
export class CityService {
  private baseUrl = 'https://your-api-base-url.com/api'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  getCities(): Observable<City[]> {
    return this.http.get<City[]>(`${this.baseUrl}/cities`);
  }
}
