import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WeatherService } from '../service/weather.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-weather',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './weather-aqi.html',
  styleUrls: ['./weather-aqi.css'],
})
export class WeatherAqi implements OnInit {
  city = '';
  currentTemp = 0;
  condition = '';
  tempMin = 0;
  tempMax = 0;
  feelsLike = 0;
  hourlyForecast: any[] = [];
  dailyForecast: any[] = [];
  activeTab: 'hourly' | 'daily' = 'hourly';
  menuOpen = false;

  constructor(
    private weatherService: WeatherService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.weatherService.getCurrentWeather().subscribe((data: any) => {
      this.city = data.city;
      this.currentTemp = data.currentTemp;
      this.condition = data.condition;
      this.tempMin = data.tempMin;
      this.tempMax = data.tempMax;
      this.feelsLike = data.feelsLike;
    });

    this.weatherService.getHourlyForecast().subscribe((data: any[]) => {
      this.hourlyForecast = data;
    });

    this.weatherService.getDailyForecast().subscribe((data: any[]) => {
      this.dailyForecast = data;
    });
  }

  getTempBarWidth(min: number, max: number): number {
    const range = 45 - 20;
    return ((max - min) / range) * 100;
  }

  getTempBarLeft(min: number): number {
    const range = 45 - 20;
    return ((min - 20) / range) * 100;
  }
  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }
  goToSettings() {
    this.menuOpen = false;
    this.router.navigate(['/settings']);
  }
  shareWeather() {
    this.menuOpen = false;
    if (navigator.share) {
      navigator.share({
        title: 'Weather',
        text: `${this.city}: ${this.currentTemp}°C, ${this.condition}`,
      });
    }
  }
}
