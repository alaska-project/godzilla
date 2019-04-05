import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatabaseTreeComponent } from './database-tree.component';

describe('DatabaseTreeComponent', () => {
  let component: DatabaseTreeComponent;
  let fixture: ComponentFixture<DatabaseTreeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatabaseTreeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabaseTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
