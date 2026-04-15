import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WeatherAqi } from './weather-aqi';

describe('WeatherAqi', () => {
  let component: WeatherAqi;
  let fixture: ComponentFixture<WeatherAqi>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WeatherAqi]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WeatherAqi);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
