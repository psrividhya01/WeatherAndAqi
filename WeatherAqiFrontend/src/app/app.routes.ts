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
    path: 'compare',
    loadComponent: () => import('./multi-city/multi-city').then((m) => m.MultiCityComponent),
  },
  {
    path: 'settings',
    loadComponent: () => import('./personalization/personalization').then((m) => m.PersonalizationComponent),
  },
  { path: '**', redirectTo: '' },
];
