import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'unitConvert',
  standalone: true,
})
export class UnitConvertPipe implements PipeTransform {
  transform(value: number, unit: string, targetUnit: 'metric' | 'imperial'): number {
    if (!value) return 0;

    if (unit === 'celsius' && targetUnit === 'imperial') {
      return Math.round(((value * 9) / 5 + 32) * 10) / 10;
    } else if (unit === 'fahrenheit' && targetUnit === 'metric') {
      return Math.round((((value - 32) * 5) / 9) * 10) / 10;
    } else if (unit === 'kmh' && targetUnit === 'imperial') {
      return Math.round(value * 0.621371 * 10) / 10;
    } else if (unit === 'mph' && targetUnit === 'metric') {
      return Math.round((value / 0.621371) * 10) / 10;
    } else if (unit === 'km' && targetUnit === 'imperial') {
      return Math.round(value * 0.621371 * 10) / 10;
    } else if (unit === 'miles' && targetUnit === 'metric') {
      return Math.round((value / 0.621371) * 10) / 10;
    }
    return value;
  }
}
