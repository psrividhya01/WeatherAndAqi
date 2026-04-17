import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // Required for UC4 Similarity Finder
import * as L from 'leaflet';
import html2canvas from 'html2canvas';
import { WeatherService } from '../service/weather.service';

@Component({
  selector: 'app-multi-city',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './multi-city.html',
  styleUrls: ['./multi-city.css']
})
export class MultiCityComponent implements OnInit, AfterViewInit {
  @ViewChild('exportTarget') exportTarget!: ElementRef;

  // Inject service
  constructor(private weatherService: WeatherService) {}

  // UC1: Multi-City Comparison (Max 4)
  cities: any[] = [];

  // UC3: AQI Ranking Table Data
  sortAscending = false;

  // UC4: Similarity Finder Form Data
  simTemp = 25;
  simCondition = 'Sunny';
  simHumidity = 'Low';
  similarCities: any[] = [];

  private map!: L.Map;

  trackByName(_index: number, city: any) {
    return city.name;}

  ngOnInit(): void {
    // Initial sort for the AQI table
    this.sortCitiesByAqi();

    this.weatherService.getMultiCityWeather(['Chennai', 'Mumbai', 'Delhi', 'Bangalore'])
      .subscribe(data => {
        this.cities = data;
        this.sortCitiesByAqi();
        this.updateMapMarkers(); // Call map update after data arrives
      });
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  private updateMapMarkers(): void {
    if (!this.map) return;
    
    const iconDefault = L.icon({
      iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
      shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41]
    });

    this.cities.forEach(loc => {
      // Ensure backend sends lat/lon
      const marker = L.marker([loc.lat, loc.lon], { icon: iconDefault }).addTo(this.map);
      marker.bindPopup(`<b>${loc.name}</b><br>Current Temp: ${loc.temp}°C`);
    });
  }

  // --- UC1: City Grid Helpers ---
  getAqiColor(aqi: number): string {
    if (aqi <= 50) return '#28a745';
    if (aqi <= 100) return '#ffc107';
    if (aqi <= 150) return '#fd7e14';
    if (aqi <= 200) return '#dc3545';
    if (aqi <= 300) return '#6f42c1';
    return '#85144b';
  }

  // --- UC2: Interactive Leaflet Map ---
  private initMap(): void {
    // Centered roughly on India
    this.map = L.map('weather-map').setView([20.5937, 78.9629], 5);

    L.tileLayer('https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png', {
      maxZoom: 19,
      attribution: '&copy; OpenStreetMap &copy; CARTO'
    }).addTo(this.map);

    // Mock markers for our cities
    const locations = [
      { name: 'Chennai', lat: 13.0827, lon: 80.2707, temp: '32°C' },
      { name: 'Mumbai', lat: 19.0760, lon: 72.8777, temp: '29°C' },
      { name: 'Delhi', lat: 28.7041, lon: 77.1025, temp: '38°C' },
      { name: 'Bangalore', lat: 12.9716, lon: 77.5946, temp: '24°C' }
    ];

    // Fix for default Leaflet icon paths in Angular
    const iconDefault = L.icon({
      iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
      shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41]
    });

    locations.forEach(loc => {
      const marker = L.marker([loc.lat, loc.lon], { icon: iconDefault }).addTo(this.map);
      marker.bindPopup(`<b>${loc.name}</b><br>Current Temp: ${loc.temp}`);
      
      // Simulate click event to switch main dashboard
      marker.on('click', () => {
        alert(`Map Clicked: Switch dashboard context to ${loc.name}`);
      });
    });
  }

  // --- UC3: AQI Sorting Logic ---
  sortCitiesByAqi() {
    this.sortAscending = !this.sortAscending;
    this.cities.sort((a, b) => {
      return this.sortAscending ? a.aqi - b.aqi : b.aqi - a.aqi;
    });
  }

  // --- UC4: Similarity Finder ---
  findSimilarCities() {
    // Mocking an API call to your ASP.NET backend
    // GET /api/weather/similar?temp=...
    this.weatherService.getSimilarCities(this.simTemp, this.simCondition, this.simHumidity)
      .subscribe(data => this.similarCities = data);
  }

  // --- UC5: Export Dashboard as Image ---
  exportDashboard() {
    const element = this.exportTarget.nativeElement;
    
    // html2canvas takes the DOM element and renders it to a Canvas
    html2canvas(element, { useCORS: true, backgroundColor: '#1a1a2e' }).then(canvas => {
      canvas.toBlob((blob) => {
        if (blob) {
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = url;
          link.download = `Weather_Dashboard_${new Date().toISOString().split('T')[0]}.png`;
          link.click();
          window.URL.revokeObjectURL(url);
        }
      });
    });
  }
}