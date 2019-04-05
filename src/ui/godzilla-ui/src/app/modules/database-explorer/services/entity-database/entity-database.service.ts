import { Injectable } from '@angular/core';
import { UiManagementClient, UiEntityContextReference, UiNodeReference } from 'src/app/modules/database-explorer/clients/godzilla.clients';

@Injectable({
  providedIn: 'root'
})
export class EntityDatabaseService {

  constructor(private uiManagement: UiManagementClient) { }

  loadRootNodes(context: UiEntityContextReference) {
    return this.uiManagement.getRootNodes(context.id).toPromise();
  }

  getNodeChildren(context: UiEntityContextReference, node: UiNodeReference) {
    return this.uiManagement.getChildNodes(context.id, node.id);
  }
}
