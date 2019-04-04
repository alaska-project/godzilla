import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DeviceStorageService } from './services/storage/device-storage.service';
import { NotificationsSnackbarComponent } from './components/notifications-snackbar/notifications-snackbar.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    NotificationsSnackbarComponent
  ],
  entryComponents: [
    NotificationsSnackbarComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule
  ],
  exports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule
  ],
  providers: [
    DeviceStorageService
  ]
})
export class AppCommonModule { }
