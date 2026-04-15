import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./city-list/city-list').then((m) => m.CityList),
  },
  {
    path: 'weather/:cityName',
    loadComponent: () => import('./weather-aqi/weather-aqi').then((m) => m.WeatherAqi),
  },
  {
    path: 'settings',
    loadComponent: () => import('./settings/settings').then((m) => m.SettingsComponent),
  },
  {
    path: 'settings/hourly-interval',
    loadComponent: () =>
      import('./settings-hourly-interval/settings-hourly-interval').then(
        (m) => m.HourlyIntervalComponent,
      ),
  },
  { path: '**', redirectTo: '' },
];
