import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { UiEntityContextReference, UiNodeReference } from '../../clients/godzilla.clients';

@Injectable({
  providedIn: 'root'
})
export class DatabaseRouterService {

  private readonly contextRootName = 'context';
  private readonly contextRoot = `/${this.contextRootName}/`;

  constructor(private router: Router) { }

  getContextNameFromRoute() {
    const i = this.router.url.lastIndexOf(this.contextRoot);
    const contextName = i >= 0 ?
      this.router.url.substr(i + this.contextRoot.length).split('?')[0] : '';
    return contextName;
  }

  getItemIdFromRoute() {
    const itemId = this.router.parseUrl(this.router.url).queryParams['itemId'];
    return itemId;
  }

  navigateToContext(context: UiEntityContextReference) {
    const currentContextRoute = this.getContextNameFromRoute();
    if (context.name === currentContextRoute) {
      return;
    }
    this.router.navigate([this.contextRootName, context.name]);
  }

  navigateToItem(item: UiNodeReference) {
    const contextName = this.getContextNameFromRoute();
    this.router.navigate([this.contextRootName, contextName], { queryParams: { itemId: item.id } });
  }
}
