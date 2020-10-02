import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { AuthorizationService } from 'src/app/core/services/authentication.service';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { UserChangePasswordComponent } from '../user-change-password/user-change-password.component';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GeneralService } from 'src/app/core/services/general.service';
import { IIntegrationSettings } from 'src/app/core/models/integration-settings.model';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { UserSlackIntegrationComponent } from '../integrations/slack/user-slack-integration/user-slack-integration.component';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss', ]
})
export class UserProfileComponent implements OnInit {

  public userIntegrationSettings: IIntegrationSettings;

  constructor(public authorizationService: AuthorizationService, public dialog: MatDialog,
              private httpClient: HttpClient, private generalService: GeneralService) {
  }

  ngOnInit() {
    this.getUserIntegrations();
  }

  getUserIntegrations() {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');

    // tslint:disable-next-line:max-line-length
    console.log(this.generalService.URL + `IntegrationSettings/GetByEmail/${this.authorizationService.getUserEmail()}`);

    // tslint:disable-next-line:max-line-length
    this.httpClient.get<IIntegrationSettings>(this.generalService.URL + `IntegrationSettings/GetByEmail/${this.authorizationService.getUserEmail()}`, {headers}).subscribe( data => {
      console.log(data);
      this.userIntegrationSettings = data;
    });
  }

  onEmailNotificationChange(event: MatSlideToggleChange) {
    console.log(event);

    this.userIntegrationSettings.notificationEmail = event.checked;

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');

    // tslint:disable-next-line:max-line-length
    this.httpClient.post<IIntegrationSettings>(this.generalService.URL + 'IntegrationSettings/Update', JSON.stringify(this.userIntegrationSettings), { headers }).subscribe( () => {
    });
  }

  onSlackNotificationChange(event: MatSlideToggleChange) {
    console.log(event);

    this.userIntegrationSettings.notificationSlack = event.checked;

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');

    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.panelClass = 'custom-modal-dialog-transparent-background';
    const dialogRef = this.dialog.open(UserSlackIntegrationComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      console.log(result.value);
    });


    // tslint:disable-next-line:max-line-length
    // this.httpClient.post<IIntegrationSettings>(this.generalService.URL + 'IntegrationSettings/Update', JSON.stringify(this.userIntegrationSettings), { headers }).subscribe( () => {
    // });
  }

  changePassword() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.disableClose = true;
    dialogConfig.panelClass = 'custom-modal-dialog-transparent-background';
    this.dialog.open(UserChangePasswordComponent, dialogConfig);
  }
}
