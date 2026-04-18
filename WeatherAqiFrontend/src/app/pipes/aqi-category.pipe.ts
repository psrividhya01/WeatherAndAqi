import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'aqiCategory',
  standalone: true
})
export class AqiCategoryPipe implements PipeTransform {
  transform(aqi: number): { category: string; color: string; bgColor: string } {
    if (aqi <= 50) {
      return { category: 'Good', color: '#2ecc71', bgColor: '#d4edda' };
    } else if (aqi <= 100) {
      return { category: 'Moderate', color: '#f39c12', bgColor: '#fff3cd' };
    } else if (aqi <= 150) {
      return { category: 'Unhealthy for Sensitive Groups', color: '#e74c3c', bgColor: '#f8d7da' };
    } else if (aqi <= 200) {
      return { category: 'Unhealthy', color: '#c0392b', bgColor: '#f5c2c7' };
    } else if (aqi <= 300) {
      return { category: 'Very Unhealthy', color: '#8b0000', bgColor: '#e2e3e5' };
    } else {
      return { category: 'Hazardous', color: '#4b0082', bgColor: '#d1d5db' };
    }
  }
}
