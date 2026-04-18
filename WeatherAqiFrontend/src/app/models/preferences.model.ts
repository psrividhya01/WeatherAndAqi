export interface Favorite {
  favoriteId: number;
  cityName: string;
  savedAt: Date;
}

export interface UserPreferences {
  preferenceId?: number;
  userId: number;
  preferredUnit: 'metric' | 'imperial';
  showForecast: boolean;
  showAQI: boolean;
  showMap: boolean;
  showHourlyChart: boolean;
  showHealthAdvisory: boolean;
}

export interface DailyDigest {
  title: string;
  summary: string;
  highTemp: number;
  lowTemp: number;
  condition: string;
  aqiScore: number;
  aqiCategory: string;
  healthTip: string;
  generatedAt: Date;
}
