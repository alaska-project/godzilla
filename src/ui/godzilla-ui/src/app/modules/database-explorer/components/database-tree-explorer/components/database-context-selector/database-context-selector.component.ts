import { Component, OnInit, OnDestroy } from '@angular/core';
import { DatabaseContextService } from 'src/app/modules/database-explorer/services/database-context/database-context.service';
import { Observable, Subscription } from 'rxjs';
import { UiEntityContextReference } from 'src/app/modules/database-explorer/clients/godzilla.clients';

@Component({
  selector: 'god-database-context-selector',
  templateUrl: './database-context-selector.component.html',
  styleUrls: ['./database-context-selector.component.scss']
})
export class DatabaseContextSelectorComponent implements OnInit, OnDestroy {

  dartabaseContexts: Observable<UiEntityContextReference[]>;

  private currentContextSubscription: Subscription;
  currentContext: UiEntityContextReference;

  constructor(private databaseContext: DatabaseContextService) {
  }

  ngOnInit() {
    this.dartabaseContexts = this.databaseContext.availableContexts();
    this.currentContextSubscription = this.databaseContext.currentContext().subscribe(x => this.currentContext = x);
  }

  ngOnDestroy(): void {
    this.currentContextSubscription.unsubscribe();
  }

  selectContext(context: UiEntityContextReference) {
    this.databaseContext.selectContext(context);
  }
}
