import { Component, OnInit } from '@angular/core';
import { EndpointService } from 'src/app/modules/database-explorer/services/endpoint/endpoint.service';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'god-database-context-settings-dialog',
  templateUrl: './database-context-settings-dialog.component.html',
  styleUrls: ['./database-context-settings-dialog.component.scss']
})
export class DatabaseContextSettingsDialogComponent implements OnInit {

  endpoint: string;

  constructor(
    private dialogRef: MatDialogRef<DatabaseContextSettingsDialogComponent>,
    private endpointService: EndpointService) { }

  ngOnInit() {
    this.endpoint = this.endpointService.getEndpoint();
  }

  saveEndpoint() {
    this.endpointService.setEndpoint(this.endpoint);
  }

  close() {
    this.dialogRef.close();
  }
}
