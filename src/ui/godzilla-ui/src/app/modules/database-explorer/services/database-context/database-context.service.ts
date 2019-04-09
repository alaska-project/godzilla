import { Injectable } from '@angular/core';
import { UiManagementClient, UiEntityContextReference } from '../../clients/godzilla.clients';
import { OperationsService } from 'src/app/modules/common/services/operations/operations.service';
import { BehaviorSubject } from 'rxjs';
import { EndpointService } from '../endpoint/endpoint.service';
import { DatabaseRouterService } from '../database-router/database-router.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class DatabaseContextService {

  private currentContextSubject = new BehaviorSubject<UiEntityContextReference>(undefined);
  private availableContextsSubject = new BehaviorSubject<UiEntityContextReference[]>([]);

  constructor(
    private router: Router,
    private databaseRouterService: DatabaseRouterService,
    private endpointService: EndpointService,
    private operation: OperationsService,
    private databaseClient: UiManagementClient) {

    this.loadContexts();
    this.endpointService.endpointChanged().subscribe(() => {
      this.router.navigate(['']);
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
    this.databaseRouterService.navigateToContext(context);
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
    this.selectDefaultContext(contexts);
  }

  private selectDefaultContext(contexts: UiEntityContextReference[]) {
    if (contexts.length === 0) {
      return;
    };

    const routeContextName = this.databaseRouterService.getContextNameFromRoute();
    const routeContext = routeContextName ?
      contexts.find(x => x.name === routeContextName) :
      undefined;

    const defaultContext = routeContext ?
      routeContext :
      contexts[0];

    this.selectContext(defaultContext);
  }
}
