import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiCity } from './multi-city';

describe('MultiCity', () => {
  let component: MultiCity;
  let fixture: ComponentFixture<MultiCity>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MultiCity]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiCity);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
