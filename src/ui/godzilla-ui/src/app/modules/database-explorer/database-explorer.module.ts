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
import { DatabaseContextSettingsDialogComponent } from './components/database-tree-explorer/components/database-context-settings-dialog/database-context-settings-dialog.component';
import { DatabaseTreeComponent } from './components/database-tree-explorer/components/database-tree/database-tree.component';
import { DatabaseTreeNodeComponent } from './components/database-tree-explorer/components/database-tree-node/database-tree-node.component';
import { DatabaseItemInfoComponent } from './components/database-item-editor/components/database-item-info/database-item-info.component';
import { DatabaseItemJsonEditorComponent } from './components/database-item-editor/components/database-item-json-editor/database-item-json-editor.component';

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
    DatabaseContextSettingsComponent,
    DatabaseContextSettingsDialogComponent,
    DatabaseTreeComponent,
    DatabaseTreeNodeComponent,
    DatabaseItemInfoComponent,
    DatabaseItemJsonEditorComponent
  ],
  exports: [
    DatabaseExplorerComponent
  ],
  entryComponents: [
    DatabaseContextSettingsDialogComponent
  ],
  providers: [
    UiManagementClient,
    DatabaseContextService
  ]
})
export class DatabaseExplorerModule { }
