import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ModalDialogComponent } from '../components/modal-dialog/modal-dialog.component';

@Injectable({
    providedIn: 'root'
})

export class GeneralService {

    constructor(private dialog: MatDialog) {
    }

    public showModalMessage(text: string): Observable<any> {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.panelClass = 'logout-dialog';

        dialogConfig.data = { data: text };

        const dialogRef = this.dialog.open(ModalDialogComponent, dialogConfig);

        return dialogRef.afterClosed();
    }
}
