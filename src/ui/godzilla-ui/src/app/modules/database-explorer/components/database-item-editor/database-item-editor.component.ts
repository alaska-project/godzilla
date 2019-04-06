import { Component, OnInit } from '@angular/core';
import { DatabaseItemService } from '../../services/database-item/database-item.service';
import { Observable } from 'rxjs';
import { UiNodeValue } from '../../clients/godzilla.clients';

@Component({
  selector: 'god-database-item-editor',
  templateUrl: './database-item-editor.component.html',
  styleUrls: ['./database-item-editor.component.scss']
})
export class DatabaseItemEditorComponent implements OnInit {

  item: Observable<UiNodeValue>;

  constructor(private databaseItemService: DatabaseItemService) { }

  ngOnInit() {
    this.item = this.databaseItemService.getItem();
  }

}
