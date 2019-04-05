import { Component, OnInit } from '@angular/core';
import { DatabaseContextService } from 'src/app/modules/database-explorer/services/database-context/database-context.service';
import { Observable } from 'rxjs';
import { UiEntityContextReference } from 'src/app/modules/database-explorer/clients/godzilla.clients';

@Component({
  selector: 'god-database-context-selector',
  templateUrl: './database-context-selector.component.html',
  styleUrls: ['./database-context-selector.component.scss']
})
export class DatabaseContextSelectorComponent implements OnInit {

  dartabaseContexts: Observable<UiEntityContextReference[]>;

  constructor(private databaseContext: DatabaseContextService) { }

  ngOnInit() {
    this.dartabaseContexts = this.databaseContext.availableContexts();
  }

}
