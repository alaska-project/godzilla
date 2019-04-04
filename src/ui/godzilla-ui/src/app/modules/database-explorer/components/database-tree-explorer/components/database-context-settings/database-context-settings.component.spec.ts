import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatabaseContextSettingsComponent } from './database-context-settings.component';

describe('DatabaseContextSettingsComponent', () => {
  let component: DatabaseContextSettingsComponent;
  let fixture: ComponentFixture<DatabaseContextSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatabaseContextSettingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabaseContextSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
