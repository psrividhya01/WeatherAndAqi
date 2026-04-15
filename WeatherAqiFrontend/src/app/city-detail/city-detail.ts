import { Component } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';

@Component({
  selector: 'app-city-detail',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './city-detail.html',
  styleUrls: ['./city-detail.css'],
})
export class CityDetail {
  cityName: string | null = '';

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    // Get the city name from the URL
    this.cityName = this.route.snapshot.paramMap.get('cityName');
  }
}
