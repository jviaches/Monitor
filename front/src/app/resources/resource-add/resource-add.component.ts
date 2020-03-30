import { Component, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogData } from 'src/app/core/components/modal-dialog/dialog-data';
import { ResourceService } from 'src/app/core/services/resource.service';

@Component({
  selector: 'app-root',
  templateUrl: './resource-add.component.html',
  styleUrls: ['./resource-add.component.scss']
})
export class ResourceAddComponent {

    constructor(public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) {data}: DialogData, resourceService: ResourceService) {
    }

    closeDialog() {
        this.dialog.closeAll();
      }
}
