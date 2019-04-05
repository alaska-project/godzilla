import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatabaseContextSettingsDialogComponent } from './database-context-settings-dialog.component';

describe('DatabaseContextSettingsDialogComponent', () => {
  let component: DatabaseContextSettingsDialogComponent;
  let fixture: ComponentFixture<DatabaseContextSettingsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatabaseContextSettingsDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabaseContextSettingsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
