import { Injectable } from '@angular/core';
import { DatabaseContextService } from '../database-context/database-context.service';
import { UiEntityContextReference } from '../../clients/godzilla.clients';
import { EntityDatabaseService } from '../entity-database/entity-database.service';
import { ActivatedRoute } from '@angular/router';
import { DatabaseRouterService } from '../database-router/database-router.service';

@Injectable({
  providedIn: 'root'
})
export class DatabaseItemService {

  private context: UiEntityContextReference;

  constructor(
    private route: ActivatedRoute,
    private databaseRouter: DatabaseRouterService,
    private databaseContextService: DatabaseContextService,
    private entityDatabase: EntityDatabaseService) {

    this.databaseContextService.currentContext().subscribe(x => {
      this.context = x;
      this.selectItem();
    });
    this.route.queryParams.subscribe(() => this.selectItem());
  }

  private selectItem() {
    const itemId = this.databaseRouter.getItemIdFromRoute();
    if (!itemId || !this.context) {
      return;
    }
    this.entityDatabase.getItem(this.context, itemId).then(item => {
      console.log(item);
    });
  }
}
