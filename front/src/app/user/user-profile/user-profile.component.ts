import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { AuthorizationService } from 'src/app/core/services/authentication.service';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { UserChangePasswordComponent } from '../user-change-password/user-change-password.component';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss', ]
})
export class UserProfileComponent implements OnInit {

  constructor(public authorizationService: AuthorizationService, public dialog: MatDialog) {
  }

  ngOnInit() {
  }


  changePassword() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.panelClass = 'custom-modal-dialog-transparent-background';
    this.dialog.open(UserChangePasswordComponent, dialogConfig);
  }
}
