import { Component, OnInit, Input } from '@angular/core';
import { EntityNode } from '../../models/database-tree.models';
import { DatabaseRouterService } from 'src/app/modules/database-explorer/services/database-router/database-router.service';

@Component({
  selector: 'god-database-tree-node',
  templateUrl: './database-tree-node.component.html',
  styleUrls: ['./database-tree-node.component.scss']
})
export class DatabaseTreeNodeComponent implements OnInit {

  @Input()
  node: EntityNode;

  constructor(private databaseRouter: DatabaseRouterService) { }

  ngOnInit() {
  }

  selectNode() {
    this.databaseRouter.navigateToItem(this.node.item);
  }
}
