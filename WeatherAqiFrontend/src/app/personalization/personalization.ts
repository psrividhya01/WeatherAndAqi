import { Component, OnInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DragDropModule, CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { WeatherStateService } from '../service/weather-state.service';
import { WeatherService } from '../service/weather.service';

@Component({
  selector: 'app-personalization',
  standalone: true,
  imports: [CommonModule, FormsModule, DragDropModule],
  templateUrl: './personalization.html',
  styleUrls: ['./personalization.css']
})
export class PersonalizationComponent implements OnInit {
  
  // UC5: Offline Mode Monitoring
  isOffline = !navigator.onLine;
  lastFetched = new Date().toLocaleTimeString();

  @HostListener('window:offline')
  onOffline() { this.isOffline = true; }

  @HostListener('window:online')
  onOnline() { 
    this.isOffline = false; 
    this.lastFetched = new Date().toLocaleTimeString();
    // In a real app, trigger this.weatherService.refreshData() here
  }

  // UC1: Severe Weather Alerts
  alerts: any[] = [];

  // UC4: Daily Digest
  showDigest = false;
  digestData: any =  null;

  // UC2: Favorite Cities
  searchQuery = '';
  savedCities: any[] = [];

  // UC3: Customizable Widgets
  // Map the IDs to exactly match the interface in our Service
  widgets = [
    { id: 'showForecastCard', name: '7-Day Forecast', visible: true },
    { id: 'showAqiGauge', name: 'AQI Gauge', visible: true },
    { id: 'showPollutantBreakdown', name: 'AQI Breakdown', visible: true },
    { id: 'showAqiTrendChart', name: 'Trend Chart', visible: true },
    { id: 'showHealthAdvisory', name: 'Health Recommendations', visible: true }
  ];

  // INJECT THE SERVICE HERE
  constructor(
    private weatherStateService: WeatherStateService, 
    private weatherService: WeatherService
  ) {}

  ngOnInit(): void {
    if (!sessionStorage.getItem('digestShown')) {
      this.showDigest = true;
    }
    
    // Fetch Favorites
    this.weatherService.getFavorites().subscribe(data => this.savedCities = data);
    
    // Fetch Alerts
    this.weatherService.getAlerts().subscribe(data => this.alerts = data);

    if (!sessionStorage.getItem('digestShown')) {
      this.weatherService.getDigest().subscribe(data => {
        this.digestData = data;
        this.showDigest = true;
      });
    }
    
    // Listen to the service to ensure toggles match the true state on load
    this.weatherStateService.widgetConfig$.subscribe(config => {
      this.widgets.forEach(w => {
        w.visible = (config as any)[w.id];
      });
    });
  }

  // FIRES WHEN A USER CLICKS A TOGGLE
  onToggleChange(widgetId: string, isVisible: boolean) {
    this.weatherStateService.updateWidgetConfig({ [widgetId]: isVisible });
  }

  // UC1 Logic
  dismissAlert(id: number) {
    this.alerts = this.alerts.filter(a => a.id !== id);
  }

  // UC4 Logic
  closeDigest() {
    this.showDigest = false;
    sessionStorage.setItem('digestShown', 'true');
  }

  // UC2 Logic
  toggleFavorite(city: any) {
    city.isFavorite = !city.isFavorite;
    // API Call goes here: POST/DELETE /api/favorites
  }

  // UC3 Logic
  dropWidget(event: CdkDragDrop<any[]>) {
    moveItemInArray(this.widgets, event.previousIndex, event.currentIndex);
    // API Call goes here: PUT /api/users/dashboard-config
  }
}