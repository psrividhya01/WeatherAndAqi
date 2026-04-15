import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { WeatherAqi } from './weather-aqi/weather-aqi';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, WeatherAqi],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
})
export class App {
  protected readonly title = signal('WeatherAqiFrontend');
}
