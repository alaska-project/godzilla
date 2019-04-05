import { Injectable } from '@angular/core';
import { UiManagementClient, UiEntityContextReference } from '../../clients/godzilla.clients';
import { OperationsService } from 'src/app/modules/common/services/operations/operations.service';
import { BehaviorSubject } from 'rxjs';
import { EndpointService } from '../endpoint/endpoint.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class DatabaseContextService {

  private currentContextSubject = new BehaviorSubject<UiEntityContextReference>(undefined);
  private availableContextsSubject = new BehaviorSubject<UiEntityContextReference[]>([]);

  constructor(
    private router: Router,
    private endpointService: EndpointService,
    private operation: OperationsService,
    private databaseClient: UiManagementClient) {

    this.loadContexts();
    this.endpointService.endpointChanged().subscribe(() => {
      this.loadContexts();
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
    this.router.navigate([context.name]);
  }

  private loadContexts() {
    this.operation.run({
      operation: this.databaseClient.getContexts(),
      callback: x => this.setContexts(x),
      errorMessage: 'Error loading entity contexts'
    });
  }

  private setContexts(contexts: UiEntityContextReference[]) {
    this.availableContextsSubject.next(contexts);
    if (contexts.length > 0) {
      this.selectContext(contexts[0]);
    }
  }
}
