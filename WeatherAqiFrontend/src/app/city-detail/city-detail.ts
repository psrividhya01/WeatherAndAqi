import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-city-detail',
  imports: [ActivatedRoute],
  templateUrl: './city-detail.html',
  styleUrl: './city-detail.css',
})
export class CityDetail {
  cityName: string | null = '';

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    // Get the city name from the URL
    this.cityName = this.route.snapshot.paramMap.get('cityName');
  }
}
