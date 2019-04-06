import { Component, OnInit, Input, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { UiNodeValue } from 'src/app/modules/database-explorer/clients/godzilla.clients';
import { JsonEditorOptions, JsonEditorComponent } from 'ang-jsoneditor';

@Component({
  selector: 'god-database-item-json-editor',
  templateUrl: './database-item-json-editor.component.html',
  styleUrls: ['./database-item-json-editor.component.scss']
})
export class DatabaseItemJsonEditorComponent implements OnInit, OnChanges {

  @Input()
  item: UiNodeValue;

  @ViewChild(JsonEditorComponent) 
  editor: JsonEditorComponent;
  
  editorOptions: JsonEditorOptions;
  data: any;

  constructor() { }

  ngOnInit() {
    this.editorOptions = new JsonEditorOptions();
    this.editorOptions.modes = ['code', 'text', 'tree', 'view'];
  }

  ngOnChanges(changes: SimpleChanges) {
    this.data = { aa: 'bb'};
    // this.data = this.item.serializedValue ? 
    //   JSON.parse(this.item.serializedValue) :
    //   undefined;
  }
}
