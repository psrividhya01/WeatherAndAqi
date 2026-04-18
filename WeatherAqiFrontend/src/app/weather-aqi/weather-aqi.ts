import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import html2canvas from 'html2canvas';
import { WeatherService } from '../service/weather.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Chart, registerables } from 'chart.js';
Chart.register(...registerables);
import { WeatherStateService, WidgetConfig } from '../service/weather-state.service';

@Component({
  selector: 'app-weather',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './weather-aqi.html',
  styleUrls: ['./weather-aqi.css'],
})
export class WeatherAqi implements OnInit, AfterViewInit {
  @ViewChild('aqiChart') aqiChartRef!: ElementRef;
  @ViewChild('dashboardExport') dashboardExport!: ElementRef;

  exportTimestamp = '';

  // --- Weather Variables ---
  city = '';
  currentTemp = 10;
  condition = 'sunny';
  tempMin = 0;
  tempMax = 0;
  feelsLike = 0;
  hourlyForecast: any[] = [];
  dailyForecast: any[] = [];
  activeTab: 'hourly' | 'daily' = 'hourly';
  menuOpen = false;
  aqi = 0; 
  aqiCategory = '';
  healthAdvisory = '';
  
  dominantPollutant: any = { };

  pollutants: any[] = [];

  // 1. Declare the config object locally so HTML can read it
  widgetConfig!: WidgetConfig;

  constructor(
    private weatherService: WeatherService,
    private router: Router,
    private route: ActivatedRoute,
    private weatherStateService: WeatherStateService
  ) {}

  fetchWeatherData(cityName: string) {
    // Call the updated service using the dynamic city name
    this.weatherService.getCurrentWeather(cityName).subscribe({
      next: (data: any) => {
        this.currentTemp = data.currentTemp;
        this.condition = data.condition;
        this.tempMin = data.tempMin;
        this.tempMax = data.tempMax;
        this.feelsLike = data.feelsLike;
        this.aqi = data.aqi; 
        this.aqiCategory = this.getAqiStatus(this.aqi);
        
        this.healthAdvisory = data.healthAdvisory;
        this.dominantPollutant = data.dominantPollutant;
        this.pollutants = data.pollutants;
      },
      error: (err) => {
        console.error(`Failed to load weather for ${cityName}:`, err);
        // You could trigger your 'Offline' banner or show an error state here!
      }
    });

    this.weatherService.getHourlyForecast(cityName).subscribe((data: any[]) => {
      this.hourlyForecast = data;
    });

    this.weatherService.getDailyForecast(cityName).subscribe((data: any[]) => {
      this.dailyForecast = data;
    });
  }

  ngOnInit(): void {
    // Whenever personalization changes a toggle, this updates instantly.
    this.weatherStateService.widgetConfig$.subscribe(config => {
      this.widgetConfig = config;
    });

    // Subscribe to the URL parameters
    this.route.paramMap.subscribe(params => {
      const urlCity = params.get('cityName');
      
      if (urlCity) {
        this.city = urlCity; // Set the city name from the URL!
        this.fetchWeatherData(this.city); // Fetch weather data for this city
        // FUTURE: Pass this city name to your API call
        // this.weatherService.getCurrentWeather(this.city).subscribe(...)
      }
      else {
        // Fallback just in case there is no city in the URL
        this.city = 'Chennai';
        this.fetchWeatherData(this.city);
      }
    });
  }

  // Runs after the HTML renders so the Chart can attach to the <canvas>
  ngAfterViewInit(): void {
    this.initAqiChart();
  }

  // --- AQI Helper Methods ---

  getAqiStatus(value: number): string {
    if (value <= 50) return 'Good';
    if (value <= 100) return 'Moderate';
    if (value <= 150) return 'Sensitive';
    if (value <= 200) return 'Unhealthy';
    if (value <= 300) return 'Very Unhealthy';
    return 'Hazardous';
  }

  getAqiColor(value: number): string {
    if (value <= 50) return '#28a745';   // Green
    if (value <= 100) return '#ffc107';  // Yellow
    if (value <= 150) return '#fd7e14';  // Orange
    if (value <= 200) return '#dc3545';  // Red
    if (value <= 300) return '#6f42c1';  // Purple
    return '#85144b';                    // Maroon
  }

  // Math for the SVG Circular Gauge
  getGaugeOffset(aqi: number): number {
    const circumference = 2 * Math.PI * 40; // radius is 40 in the SVG
    const maxAqi = 500;
    const percentage = Math.min(aqi / maxAqi, 1);
    return circumference - (percentage * circumference);
  }

  // Math for Pollutant Progress Bars
  getPollutantWidth(value: number, limit: number): number {
    const percentage = (value / (limit * 2)) * 100; 
    return Math.min(percentage, 100); // Cap at 100% width
  }

  getPollutantColor(value: number, limit: number): string {
    if (value <= limit) return '#28a745'; // Safe (Green)
    if (value <= limit * 2) return '#ffc107'; // Warning (Yellow)
    return '#dc3545'; // Danger (Red)
  }

  // Initializes the 7-Day Trend Chart
  initAqiChart() {
    const ctx = this.aqiChartRef.nativeElement.getContext('2d');
    new Chart(ctx, {
      type: 'line',
      data: {
        labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
        datasets: [{
          label: 'AQI Trend',
          data: [85, 92, 120, 155, 140, 110, 95], // Mock 7-day data
          borderColor: 'rgba(255, 255, 255, 0.8)',
          backgroundColor: 'rgba(255, 255, 255, 0.2)',
          borderWidth: 2,
          tension: 0.4, // Makes the line curve smoothly
          fill: true
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: { legend: { display: false } },
        scales: {
          y: { 
            beginAtZero: true, 
            max: 300,
            ticks: { color: 'rgba(255,255,255,0.7)' },
            grid: { color: 'rgba(255,255,255,0.1)' }
          },
          x: { 
            ticks: { color: 'rgba(255,255,255,0.7)' },
            grid: { display: false }
          }
        }
      }
    });
  }

  // --- Weather Helper Methods ---

  // Dynamic Photographic Backgrounds based on Weather AND AQI
  getWeatherBackgroundImage(condition: string, aqi: number): string {
    const cond = condition.toLowerCase();
    const overlay = 'linear-gradient(rgba(10, 15, 30, 0.5), rgba(10, 15, 30, 0.8))';
    const assetsPath = 'assets/'; // Points to your src/assets folder
    
    if (cond.includes('rain') || cond.includes('drizzle') || cond.includes('shower')) {
      // Rainy window
      return `${overlay}, url('${assetsPath}rainy(drops).png')`;
    }
    if (cond.includes('thunder') || cond.includes('storm')) {
      // Lightning storm
      return `${overlay}, url('${assetsPath}thunder.png')`;
    }
    if (cond.includes('snow') || cond.includes('ice')) {
      // Snowy landscape
      return `${overlay}, url('${assetsPath}snowy.png')`;
    }
    if (cond.includes('cloud') || cond.includes('overcast')) {
      // Cloudy sky
      return `${overlay}, url('${assetsPath}cloudy.png')`;
    }
    
    // Default / Clear / Sunny
    return `${overlay}, url('${assetsPath}sunny.png')`;
  }

  getWeatherIcon(condition: string): string {
    const cond = condition.toLowerCase();
    if (cond.includes('clear') || cond.includes('sunny')) return '☀️';
    if (cond.includes('partly cloud')) return '⛅';
    if (cond.includes('cloud') || cond.includes('overcast')) return '☁️';
    if (cond.includes('rain') || cond.includes('drizzle') || cond.includes('shower')) return '🌧️';
    if (cond.includes('thunder') || cond.includes('storm')) return '⛈️';
    if (cond.includes('snow') || cond.includes('ice') || cond.includes('flurries')) return '❄️';
    if (cond.includes('fog') || cond.includes('mist') || cond.includes('haze')) return '🌫️';
    if (cond.includes('wind') || cond.includes('breezy')) return '💨';
    return '🌡️'; 
  }

  getTempBarWidth(min: number, max: number): number {
    const range = 45 - 20;
    return ((max - min) / range) * 100;
  }

  getTempBarLeft(min: number): number {
    const range = 45 - 20;
    return ((min - 20) / range) * 100;
  }

  // --- Navigation & UI ---

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  goToSettings() {
    this.menuOpen = false;
    this.router.navigate(['/settings']);
  }

  exportDashboard() {
    const element = this.dashboardExport?.nativeElement;
    if (!element) {
      return;
    }

    this.exportTimestamp = new Date().toLocaleString();

    html2canvas(element, {
      useCORS: true,
      backgroundColor: '#ffffff',
      scale: window.devicePixelRatio || 1
    }).then((canvas: HTMLCanvasElement) => {
      canvas.toBlob((blob: Blob | null) => {
        if (!blob) {
          return;
        }

        const fileName = `${this.city.replace(/\s+/g, '_')}_Dashboard_${new Date().toISOString().replace(/[:.]/g, '-')}.png`;
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName;
        link.click();
        window.URL.revokeObjectURL(url);
      });
    });
  }

  shareWeather() {
    this.menuOpen = false;
    if (navigator.share) {
      navigator.share({
        title: 'Weather Dashboard',
        text: `${this.city}: ${this.currentTemp}°C, ${this.condition}. AQI: ${this.aqi} (${this.aqiCategory})`,
      });
    }
  }

  toggleWidget(widgetName: keyof WidgetConfig) {
    // This tells the brain (service) to update the setting globally.
    // Because you are subscribed in ngOnInit, it will instantly update this component too!
    this.weatherStateService.updateWidgetConfig({
      [widgetName]: !this.widgetConfig[widgetName]
    });
  }
}