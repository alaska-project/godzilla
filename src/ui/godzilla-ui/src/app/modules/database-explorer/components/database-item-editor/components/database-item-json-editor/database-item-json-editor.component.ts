import { Component, OnInit, Input, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { UiNodeValue } from 'src/app/modules/database-explorer/clients/godzilla.clients';
import { JsonEditorOptions, JsonEditorComponent } from 'ang-jsoneditor';
import { preserveWhitespacesDefault } from '@angular/compiler';

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
    this.data = this.item.serializedValue ? 
      JSON.parse(this.cleanJson(this.item.serializedValue)) :
      undefined;
  }

  private cleanJson(value: string) {
    const searchString = 'UUID("';
    const guidLength = 36;
    let cleanedValue = value;

    if (cleanedValue.indexOf(searchString) >= 0) {
      const uuidIndex = value.indexOf(searchString);
      const guidStartIndex = uuidIndex + searchString.length - 1;
      const guidEndIndex = uuidIndex + searchString.length + guidLength + 1;
      cleanedValue = 
        cleanedValue.substring(0, uuidIndex) +
        cleanedValue.substring(guidStartIndex, guidEndIndex) +
        cleanedValue.substring(guidEndIndex + 1);
    }

    if (cleanedValue.indexOf(searchString) >= 0) { 
      cleanedValue = this.cleanJson(cleanedValue);
    }

    return cleanedValue;
  }
}
