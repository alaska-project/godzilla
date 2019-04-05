import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatabaseTreeNodeComponent } from './database-tree-node.component';

describe('DatabaseTreeNodeComponent', () => {
  let component: DatabaseTreeNodeComponent;
  let fixture: ComponentFixture<DatabaseTreeNodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatabaseTreeNodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabaseTreeNodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
