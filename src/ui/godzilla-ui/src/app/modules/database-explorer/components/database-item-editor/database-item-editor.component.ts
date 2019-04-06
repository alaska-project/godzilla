import { Component, OnInit } from '@angular/core';
import { DatabaseItemService } from '../../services/database-item/database-item.service';

@Component({
  selector: 'god-database-item-editor',
  templateUrl: './database-item-editor.component.html',
  styleUrls: ['./database-item-editor.component.scss']
})
export class DatabaseItemEditorComponent implements OnInit {

  constructor(private databaseItemService: DatabaseItemService) { }

  ngOnInit() {
  }

}
