import { Injectable } from '@angular/core';
import { UiManagementClient, UiEntityContextReference } from '../../clients/godzilla.clients';
import { OperationsService } from 'src/app/modules/common/services/operations/operations.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DatabaseContextService {

  private currentContextSubject = new BehaviorSubject<UiEntityContextReference>(undefined);
  private availableContextsSubject = new BehaviorSubject<UiEntityContextReference[]>([]);

  constructor(
    private operation: OperationsService,
    private databaseClient: UiManagementClient) { }

  loadContexts() {
    this.operation.run({
      operation: this.databaseClient.getContexts(),
      callback: x => this.setContexts(x),
      errorMessage: 'Error loading entity contexts'
    });
  }

  availableContexts() {
    return this.availableContextsSubject.asObservable();
  }

  currentContext() {
    return this.currentContextSubject.asObservable();
  }

  selectContext(context: UiEntityContextReference) {
    this.currentContextSubject.next(context);
  }

  private setContexts(contexts: UiEntityContextReference[]) {
    this.availableContextsSubject.next(contexts);
    if (contexts.length > 0) {
      this.currentContextSubject.next(contexts[0]);
    }
  }
}
