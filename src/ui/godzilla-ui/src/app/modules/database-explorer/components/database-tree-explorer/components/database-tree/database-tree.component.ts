import { Component, OnInit, Input, AfterContentInit, OnChanges, SimpleChanges } from '@angular/core';
import { FlatTreeControl } from '@angular/cdk/tree';
import { EntityNode } from '../../models/database-tree.models';
import { EntityDatabaseDatasource } from '../../services/entity-datatabase-datasource/entity-database-datasource.service';
import { EntityDatabaseService } from 'src/app/modules/database-explorer/services/entity-database/entity-database.service';
import { UiEntityContextReference } from 'src/app/modules/database-explorer/clients/godzilla.clients';

@Component({
  selector: 'god-database-tree',
  templateUrl: './database-tree.component.html',
  styleUrls: ['./database-tree.component.scss']
})
export class DatabaseTreeComponent implements OnInit, OnChanges {

  @Input()
  context: UiEntityContextReference;

  constructor(private databaseService: EntityDatabaseService) {
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.context) {
      this.treeControl = new FlatTreeControl<EntityNode>(this.getLevel, this.isExpandable);
      this.dataSource = new EntityDatabaseDatasource(this.context, this.databaseService, this.treeControl);
      this.databaseService.loadRootNodes(this.context).then(nodes => {
        this.dataSource.data = nodes.map(node => new EntityNode(node, 0, !node.isLeaf, false));
      });
    } else {
      this.treeControl = undefined;
      this.dataSource = undefined;
    }
  }

  treeControl: FlatTreeControl<EntityNode>;

  dataSource: EntityDatabaseDatasource;

  getLevel = (node: EntityNode) => node.level;

  isExpandable = (node: EntityNode) => node.expandable;

  hasChild = (_: number, _nodeData: EntityNode) => _nodeData.expandable;
}
