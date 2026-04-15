import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { SettingsService } from '../service/settings.service';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './settings.html',
  styleUrls: ['./settings.css'],
})
export class SettingsComponent {
  constructor(
    private router: Router,
    private settingsService: SettingsService,
  ) {}

  get version() {
    return this.settingsService.version();
  }

  get tempUnit() {
    return this.settingsService.tempUnit();
  }

  get hourlyInterval() {
    return this.settingsService.hourlyInterval();
  }

  get forecastFormat() {
    return this.settingsService.forecastFormat();
  }

  goBack() {
    this.router.navigate(['/']);
  }

  goToHourlyInterval() {
    this.router.navigate(['/settings/hourly-interval']);
  }
}
