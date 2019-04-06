import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatabaseItemJsonEditorComponent } from './database-item-json-editor.component';

describe('DatabaseItemJsonEditorComponent', () => {
  let component: DatabaseItemJsonEditorComponent;
  let fixture: ComponentFixture<DatabaseItemJsonEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatabaseItemJsonEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabaseItemJsonEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
