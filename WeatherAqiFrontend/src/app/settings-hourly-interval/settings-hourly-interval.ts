import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { SettingsService } from '../service/settings.service';

@Component({
  selector: 'app-hourly-interval',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './settings-hourly-interval.html',
  styleUrls: ['./settings-hourly-interval.css'],
})
export class HourlyIntervalComponent {
  twoHourSlots = [
    { time: 'Now', icon: '☀️' },
    { time: '12:00', icon: '⛅' },
    { time: '14:00', icon: '🌤️' },
    { time: '16:00', icon: '⛅' },
    { time: '18:00', icon: '⛅' },
  ];

  oneHourSlots = [
    { time: 'Now', icon: '☀️' },
    { time: '12:00', icon: '⛅' },
    { time: '13:00', icon: '🌤️' },
    { time: '14:00', icon: '⛅' },
    { time: '15:00', icon: '⛅' },
  ];

  constructor(
    private router: Router,
    private settingsService: SettingsService,
  ) {}

  get selected() {
    return this.settingsService.hourlyInterval();
  }

  goBack() {
    this.router.navigate(['/settings']);
  }

  select(val: '2h' | '1h') {
    const interval = val === '2h' ? '2-hour interval' : '1-hour interval';
    this.settingsService.setHourlyInterval(interval);
  }
}
