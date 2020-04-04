import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ModalDialogComponent } from '../components/modal-dialog/modal-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ModalYesNoDialogComponent } from '../components/yesno-modal-dialog/yesno-modal-dialog.component';

@Injectable({
    providedIn: 'root'
})

export class GeneralService {

    URL = 'https://localhost:44356/api/';

    constructor(private dialog: MatDialog, private snackBar: MatSnackBar) {
    }

    public showModalMessage(textCaption: string, textMessage: string): Observable<any> {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        // dialogConfig.panelClass = 'logout-dialog';

        dialogConfig.data = { data: {message: textMessage, caption: textCaption } };

        const dialogRef = this.dialog.open(ModalDialogComponent, dialogConfig);

        return dialogRef.afterClosed();
    }

    public showYesNoModalMessage(): Observable<any> {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        // dialogConfig.panelClass = 'logout-dialog';

        const dialogRef = this.dialog.open(ModalYesNoDialogComponent, dialogConfig);

        return dialogRef.afterClosed();
    }

    public showActionConfirmation(text: string) {
        // const config = new MatSnackBarConfig();
        // config.panelClass = ['background-red'];
        // config.duration = 5000;
        this.snackBar.open(text, null, { duration: 3000});
    }
}
