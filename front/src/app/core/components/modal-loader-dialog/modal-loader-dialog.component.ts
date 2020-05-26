import { Component, Inject } from '@angular/core';
import { DialogData } from './dialog-data';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-modal-loader-dialog',
  templateUrl: './modal-loader-dialog.component.html',
  styleUrls: ['./modal-loader-dialog.component.scss']
})

export class ModalLoaderDialogComponent {

  public caption: 'Loading in progress..';
  public message = '';

  constructor(private dialogRef: MatDialogRef<ModalLoaderDialogComponent>, @Inject(MAT_DIALOG_DATA) { data }: DialogData) {
    this.caption = data.caption;
    this.message = data.message;
  }
}
