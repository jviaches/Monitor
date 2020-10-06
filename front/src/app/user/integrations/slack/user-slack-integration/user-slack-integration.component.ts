import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { GeneralService } from 'src/app/core/services/general.service';
import { MatDialog } from '@angular/material/dialog';
import { AuthenticationService } from 'src/app/core/services/authentication.service';

@Component({
  selector: 'app-user-slack-integration',
  templateUrl: './user-slack-integration.component.html',
  styleUrls: ['./user-slack-integration.component.scss']
})
export class UserSlackIntegrationComponent {

  form: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthenticationService,
              private generalService: GeneralService, public dialog: MatDialog) {

    this.form = this.fb.group({ slack_channel: ['', [Validators.required]] });
  }

  async doUpdate() {
    // await this.authService.changePassword(this.form.value.oldPassword, this.form.value.newPassword)
    //  .then( response => {
    //   console.log(response);

    //   if (response === 'SUCCESS') {
    //     this.generalService.showActionConfirmationSuccess('Code changes succesfully!');
    console.log(this.dialog.openDialogs);
    this.dialog.openDialogs[0].close(this.getSlackLink);
    //   }
    // });
  }

  doCancel() {
    this.dialog.closeAll();
  }

  get getSlackLink() {
    return this.form.get('slack_channel');
  }

  isInValid() {
    return this.form.invalid;
  }
}
