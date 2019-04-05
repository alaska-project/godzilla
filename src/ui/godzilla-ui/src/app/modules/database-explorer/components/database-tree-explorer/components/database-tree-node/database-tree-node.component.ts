import { Component, OnInit, Input } from '@angular/core';
import { EntityNode } from '../../models/database-tree.models';
import { Router } from '@angular/router';

@Component({
  selector: 'god-database-tree-node',
  templateUrl: './database-tree-node.component.html',
  styleUrls: ['./database-tree-node.component.scss']
})
export class DatabaseTreeNodeComponent implements OnInit {

  @Input()
  node: EntityNode;

  constructor(private router: Router) { }

  ngOnInit() {
  }

  selectNode() {
    this.router.navigate(['/item', this.node.item.id]);
  }
}
