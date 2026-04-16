export interface CurrentWeather {
  city: string;
  currentTemp: number;
  condition: string;
  tempMin: number;
  tempMax: number;
  feelsLike: number;
}

export interface HourlyForecastItem {
  time: string;
  icon: string;
  temp?: number;
  active?: boolean;
}

export interface DailyForecastItem {
  label: string;
  icon: string;
  min: number;
  max: number;
  isToday?: boolean;
}
