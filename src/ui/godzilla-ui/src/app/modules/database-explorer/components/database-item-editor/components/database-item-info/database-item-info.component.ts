import { Component, OnInit, Input } from '@angular/core';
import { UiNodeValue } from 'src/app/modules/database-explorer/clients/godzilla.clients';

@Component({
  selector: 'god-database-item-info',
  templateUrl: './database-item-info.component.html',
  styleUrls: ['./database-item-info.component.scss']
})
export class DatabaseItemInfoComponent implements OnInit {

  @Input()
  item: UiNodeValue;

  constructor() { }

  ngOnInit() {
  }

}
