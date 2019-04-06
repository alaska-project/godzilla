import { Component, OnInit, OnDestroy } from '@angular/core';
import { DatabaseItemService } from '../../services/database-item/database-item.service';
import { Subscription } from 'rxjs';
import { UiNodeValue } from '../../clients/godzilla.clients';

@Component({
  selector: 'god-database-item-editor',
  templateUrl: './database-item-editor.component.html',
  styleUrls: ['./database-item-editor.component.scss']
})
export class DatabaseItemEditorComponent implements OnInit, OnDestroy {

  private itemSubscription: Subscription;
  item: UiNodeValue;

  constructor(private databaseItemService: DatabaseItemService) { }

  ngOnInit() {
    this.itemSubscription = this.databaseItemService.getItem().subscribe(x => this.item = x);
  }

  ngOnDestroy(): void {
    this.itemSubscription.unsubscribe();
  }
}
