import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { DatabaseContextSettingsDialogComponent } from '../database-context-settings-dialog/database-context-settings-dialog.component';

@Component({
  selector: 'god-database-context-settings',
  templateUrl: './database-context-settings.component.html',
  styleUrls: ['./database-context-settings.component.scss']
})
export class DatabaseContextSettingsComponent implements OnInit {

  constructor(private dialog: MatDialog) { }

  ngOnInit() {
  }

  openSettingsDialog() {
    this.dialog.open(DatabaseContextSettingsDialogComponent, {
      // width: '250px',
      // height: '150px',
    });
  }
}
