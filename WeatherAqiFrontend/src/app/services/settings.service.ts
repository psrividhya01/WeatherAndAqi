import { Injectable } from '@angular/core';
import { UserPreferences } from '../models/preferences.model';

@Injectable({
  providedIn: 'root',
})
export class SettingsService {
  private readonly PREFERENCES_KEY = 'user_preferences';
  private readonly DIGEST_KEY = 'digest_shown_today';

  getPreferences(): UserPreferences {
    const stored = localStorage.getItem(this.PREFERENCES_KEY);
    return stored ? JSON.parse(stored) : this.getDefaults();
  }

  savePreferences(prefs: UserPreferences): void {
    localStorage.setItem(this.PREFERENCES_KEY, JSON.stringify(prefs));
  }

  hasShownDigestToday(): boolean {
    const stored = sessionStorage.getItem(this.DIGEST_KEY);
    if (!stored) return false;
    const storedDate = new Date(stored);
    const today = new Date().toDateString();
    return storedDate.toDateString() === today;
  }

  markDigestShownToday(): void {
    sessionStorage.setItem(this.DIGEST_KEY, new Date().toISOString());
  }

  private getDefaults(): UserPreferences {
    return {
      userId: 0,
      preferredUnit: 'metric',
      showForecast: true,
      showAQI: true,
      showMap: true,
      showHourlyChart: true,
      showHealthAdvisory: true,
    };
  }
}
