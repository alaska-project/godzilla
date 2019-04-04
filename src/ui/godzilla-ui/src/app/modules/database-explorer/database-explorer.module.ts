import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppCommonModule } from '../common/common.module';
import { MaterialModule } from '../material/material.module';
import { DatabaseExplorerComponent } from './database-explorer.component';
import { DatabaseTreeExplorerComponent } from './components/database-tree-explorer/database-tree-explorer.component';
import { DatabaseItemEditorComponent } from './components/database-item-editor/database-item-editor.component';
import { DatabaseContextService } from './services/database-context/database-context.service';
import { UiManagementClient } from './clients/godzilla.clients';
import { DatabaseContextSelectorComponent } from './components/database-tree-explorer/components/database-context-selector/database-context-selector.component';
import { DatabaseContextSettingsComponent } from './components/database-tree-explorer/components/database-context-settings/database-context-settings.component';

@NgModule({
  imports: [
    CommonModule,
    AppCommonModule,
    MaterialModule
  ],
  declarations: [
    DatabaseExplorerComponent,
    DatabaseTreeExplorerComponent,
    DatabaseItemEditorComponent,
    DatabaseContextSelectorComponent,
    DatabaseContextSettingsComponent
  ],
  exports: [
    DatabaseExplorerComponent
  ], 
  providers: [
    UiManagementClient,
    DatabaseContextService
  ]
})
export class DatabaseExplorerModule { }
