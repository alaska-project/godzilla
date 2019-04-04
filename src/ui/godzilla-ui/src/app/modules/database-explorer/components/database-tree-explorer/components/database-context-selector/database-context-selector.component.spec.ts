import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatabaseContextSelectorComponent } from './database-context-selector.component';

describe('DatabaseContextSelectorComponent', () => {
  let component: DatabaseContextSelectorComponent;
  let fixture: ComponentFixture<DatabaseContextSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatabaseContextSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabaseContextSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
