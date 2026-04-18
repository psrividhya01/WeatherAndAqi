import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NetworkStatusService {
  private onlineSubject = new BehaviorSubject<boolean>(navigator.onLine);
  public online$ = this.onlineSubject.asObservable();

  constructor() {
    window.addEventListener('online', () => {
      console.log('Network connection restored');
      this.onlineSubject.next(true);
    });

    window.addEventListener('offline', () => {
      console.log('Network connection lost');
      this.onlineSubject.next(false);
    });
  }

  isOnline(): boolean {
    return this.onlineSubject.value;
  }

  getLastFetchedAt(key: string): Date | null {
    const stored = localStorage.getItem(`${key}_fetchedAt`);
    return stored ? new Date(stored) : null;
  }

  setLastFetchedAt(key: string): void {
    localStorage.setItem(`${key}_fetchedAt`, new Date().toISOString());
  }
}
