import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    MatSnackBarModule,
    MatSelectModule,
    MatButtonModule,
    MatInputModule,
    MatDialogModule,
    MatIconModule
  ],
  exports: [
    BrowserAnimationsModule,
    MatSnackBarModule,
    MatSelectModule,
    MatButtonModule,
    MatInputModule,
    MatDialogModule,
    MatIconModule
  ]
})
export class MaterialModule { }
