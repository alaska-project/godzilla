import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatabaseItemInfoComponent } from './database-item-info.component';

describe('DatabaseItemInfoComponent', () => {
  let component: DatabaseItemInfoComponent;
  let fixture: ComponentFixture<DatabaseItemInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatabaseItemInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabaseItemInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
