import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SettingsHourlyInterval } from './settings-hourly-interval';

describe('SettingsHourlyInterval', () => {
  let component: SettingsHourlyInterval;
  let fixture: ComponentFixture<SettingsHourlyInterval>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SettingsHourlyInterval]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SettingsHourlyInterval);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
