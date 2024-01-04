import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CommonComponent } from './common.component';

describe('CommonComponent', () => {
  let component: CommonComponent;
  let fixture: ComponentFixture<CommonComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CommonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
