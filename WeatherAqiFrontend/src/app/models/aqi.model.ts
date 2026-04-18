export interface AQITrendEntry {
  recordedDate: Date;
  aqiScore: number;
  category: string;
}

export interface AQITrend {
  city: string;
  trendData: AQITrendEntry[];
}

export interface HealthAdvisory {
  category: string;
  advisory: string;
  sensitiveGroupNote: string;
}
