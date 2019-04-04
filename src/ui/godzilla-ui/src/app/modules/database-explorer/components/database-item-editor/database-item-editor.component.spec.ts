import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatabaseItemEditorComponent } from './database-item-editor.component';

describe('DatabaseItemEditorComponent', () => {
  let component: DatabaseItemEditorComponent;
  let fixture: ComponentFixture<DatabaseItemEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatabaseItemEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabaseItemEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
