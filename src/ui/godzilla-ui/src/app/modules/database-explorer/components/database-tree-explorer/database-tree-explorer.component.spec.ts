import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatabaseTreeExplorerComponent } from './database-tree-explorer.component';

describe('DatabaseTreeExplorerComponent', () => {
  let component: DatabaseTreeExplorerComponent;
  let fixture: ComponentFixture<DatabaseTreeExplorerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatabaseTreeExplorerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabaseTreeExplorerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
