import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DatabaseExplorerComponent } from './modules/database-explorer/database-explorer.component';

const routes: Routes = [
  {
    path: '**',
    component: DatabaseExplorerComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
