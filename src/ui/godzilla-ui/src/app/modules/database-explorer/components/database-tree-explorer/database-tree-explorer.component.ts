import { Component, OnInit } from '@angular/core';
import { DatabaseContextService } from '../../services/database-context/database-context.service';
import { Observable } from 'rxjs';
import { UiEntityContextReference } from '../../clients/godzilla.clients';

@Component({
  selector: 'god-database-tree-explorer',
  templateUrl: './database-tree-explorer.component.html',
  styleUrls: ['./database-tree-explorer.component.scss']
})
export class DatabaseTreeExplorerComponent implements OnInit {

  contexts: Observable<UiEntityContextReference[]>;

  constructor(private databaseContextService: DatabaseContextService) { }

  ngOnInit() {
    this.contexts = this.databaseContextService.availableContexts();
    this.databaseContextService.loadContexts();
  }

}
