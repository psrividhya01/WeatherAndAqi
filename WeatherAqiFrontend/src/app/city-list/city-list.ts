import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-city-list',
  imports: [],
  templateUrl: './city-list.html',
  styleUrl: './city-list.css',
})
export class CityList {
  cities = [
    { name: 'Chennai', temp: 34, desc: 'Mostly clear', range: '27 ~ 35°C' }
  ];

  constructor(private router: Router) {}

  goToDetails(cityName: string) {
    this.router.navigate(['/weather', cityName]);
  }
}
