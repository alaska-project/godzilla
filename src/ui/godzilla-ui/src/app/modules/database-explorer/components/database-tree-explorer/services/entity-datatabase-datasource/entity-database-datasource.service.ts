import { Injectable } from '@angular/core';
import { BehaviorSubject, merge, Observable } from 'rxjs';
import { UiEntityContextReference } from 'src/app/modules/database-explorer/clients/godzilla.clients';
import { FlatTreeControl } from '@angular/cdk/tree';
import { EntityNode } from '../../models/database-tree.models';
import { CollectionViewer, SelectionChange } from '@angular/cdk/collections';
import { map } from 'rxjs/operators';
import { EntityDatabaseService } from 'src/app/modules/database-explorer/services/entity-database/entity-database.service';

@Injectable()
export class EntityDatabaseDatasource {

  dataChange = new BehaviorSubject<EntityNode[]>([]);

  get data(): EntityNode[] { return this.dataChange.value; }
  set data(value: EntityNode[]) {
    this.treeControl.dataNodes = value;
    this.dataChange.next(value);
  }

  constructor(
    private databaseContext: UiEntityContextReference,
    private databaseService: EntityDatabaseService,
    private treeControl: FlatTreeControl<EntityNode>) { }

  connect(collectionViewer: CollectionViewer): Observable<EntityNode[]> {
    this.treeControl.expansionModel.onChange.subscribe(change => {
      if ((change as SelectionChange<EntityNode>).added ||
        (change as SelectionChange<EntityNode>).removed) {
        this.handleTreeControl(change as SelectionChange<EntityNode>);
      }
    });

    return merge(collectionViewer.viewChange, this.dataChange).pipe(map(() => this.data));
  }

  /** Handle expand/collapse behaviors */
  handleTreeControl(change: SelectionChange<EntityNode>) {
    if (change.added) {
      change.added.forEach(node => this.toggleNode(node, true));
    }
    if (change.removed) {
      change.removed.slice().reverse().forEach(node => this.toggleNode(node, false));
    }
  }

  /**
   * Toggle the node, remove from display list
   */
  toggleNode(node: EntityNode, expand: boolean) {
    // const children = this.database.getChildren(node.item);
    // const index = this.data.indexOf(node);
    // if (!children || index < 0) { // If no children, or cannot find the node, no op
    //   return;
    // }
    if (node.item.isLeaf === true) {
      return;
    }

    const index = this.data.indexOf(node);
    node.isLoading = true;
    this.databaseService.getNodeChildren(this.databaseContext, node.item).subscribe(children => {
      if (expand) {
        const nodes = children.map(child =>
          new EntityNode(child, node.level + 1, !child.isLeaf));
        this.data.splice(index + 1, 0, ...nodes);
      } else {
        let count = 0;
        for (let i = index + 1; i < this.data.length
          && this.data[i].level > node.level; i++ , count++) { }
        this.data.splice(index + 1, count);
      }

      // notify the change
      this.dataChange.next(this.data);
      node.isLoading = false;
    })

    // setTimeout(() => {
    //   if (expand) {
    //     const nodes = children.map(name =>
    //       new EntityNode(name, node.level + 1, this.database.isExpandable(name)));
    //     this.data.splice(index + 1, 0, ...nodes);
    //   } else {
    //     let count = 0;
    //     for (let i = index + 1; i < this.data.length
    //       && this.data[i].level > node.level; i++ , count++) { }
    //     this.data.splice(index + 1, count);
    //   }

    //   // notify the change
    //   this.dataChange.next(this.data);
    //   node.isLoading = false;
    // }, 1000);
  }
}
