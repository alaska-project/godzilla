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
    value = this.cleanFunctionFromJsonJson(value, 'UUID', 36);
    value = this.cleanFunctionFromJsonJson(value, 'ISODate', 24);
    return value;
  }

  private cleanFunctionFromJsonJson(value: string, functionName: string, contentLength: number) {
    const searchString = `${functionName}("`;
    let cleanedValue = value;

    if (cleanedValue.indexOf(searchString) >= 0) {
      const functionStartIndex = value.indexOf(searchString);
      const contentStartIndex = functionStartIndex + searchString.length - 1;
      const contentEndIndex = functionStartIndex + searchString.length + contentLength + 1;
      cleanedValue = 
        cleanedValue.substring(0, functionStartIndex) +
        cleanedValue.substring(contentStartIndex, contentEndIndex) +
        cleanedValue.substring(contentEndIndex + 1);
    }

    if (cleanedValue.indexOf(searchString) >= 0) { 
      cleanedValue = this.cleanFunctionFromJsonJson(cleanedValue, functionName, contentLength);
    }

    return cleanedValue;
  }
}
