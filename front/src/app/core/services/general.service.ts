import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ModalDialogComponent } from '../components/modal-dialog/modal-dialog.component';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { ModalYesNoDialogComponent } from '../components/yesno-modal-dialog/yesno-modal-dialog.component';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})

export class GeneralService {

    URL = environment.api;

    constructor(private dialog: MatDialog, private snackBar: MatSnackBar, private router: Router) {
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

    public showModalComponent(component: any, textCaption: string): Observable<any> {
        const dialogConfig = new MatDialogConfig();

        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        // dialogConfig.panelClass = 'logout-dialog';

        dialogConfig.data = { data: {caption: textCaption } };

        const dialogRef = this.dialog.open(component, dialogConfig);

        return dialogRef.afterClosed();
    }

    public showActionConfirmationSuccess(text: string) {
        const config = new MatSnackBarConfig();
        config.panelClass = ['snackBar-success'];
        config.duration = 3000;
        this.snackBar.open(text, null, config);
    }

    public showActionConfirmationFail(text: string) {
        const config = new MatSnackBarConfig();
        config.panelClass = ['snackBar-fail'];
        config.duration = 3000;
        this.snackBar.open(text, null, config);
    }

    public showActionRedirectConfirmation(text: string, url: string) {
        this.snackBar.open(text, 'Close').onAction().subscribe( () => this.router.navigateByUrl(url));
    }
}
