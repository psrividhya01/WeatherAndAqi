import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CityService } from '../service/city.service';
import { City } from '../models/city.model';

@Component({
  selector: 'app-city-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './city-list.html',
  styleUrls: ['./city-list.css'],
})
export class CityList implements OnInit {
  cities: City[] = [];

  constructor(
    private router: Router,
    private cityService: CityService,
  ) {}

  ngOnInit() {
    this.cityService.getCities().subscribe((data) => {
      this.cities = data;
    });
  }

  goToDetails(cityName: string) {
    this.router.navigate(['/weather', cityName]);
  }
}
