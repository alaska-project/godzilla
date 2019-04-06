import { Component, OnInit, Input } from '@angular/core';
import { UiNodeValue } from 'src/app/modules/database-explorer/clients/godzilla.clients';

@Component({
  selector: 'god-database-item-json-editor',
  templateUrl: './database-item-json-editor.component.html',
  styleUrls: ['./database-item-json-editor.component.scss']
})
export class DatabaseItemJsonEditorComponent implements OnInit {

  @Input()
  item: UiNodeValue;

  constructor() { }

  ngOnInit() {
  }

}
