import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthorizationService } from 'src/app/core/services/authentication.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-user-change-password',
  templateUrl: './user-change-password.component.html',
  styleUrls: ['./user-change-password.component.scss']
})
export class UserChangePasswordComponent {

  form: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthorizationService,
              private generalService: GeneralService, public dialog: MatDialog) {
    this.form = this.fb.group({
      oldPassword: ['', [Validators.required, Validators.minLength(8)]],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
    });
  }

  async doSubmit() {
    await this.authService.changePassword(this.form.value.oldPassword, this.form.value.newPassword)
     .then( response => {
      console.log(response);

      if (response === 'SUCCESS') {
        this.generalService.showActionConfirmationSuccess('Code changes succesfully!');
        this.dialog.closeAll();
      }
    });
  }
  doCancel() {
    this.dialog.closeAll();
  }

  get getOldPassword() {
    return this.form.get('oldPassword');
  }

  get getNewPassword() {
    return this.form.get('newPassword');
  }

  isInValid() {
    return this.form.invalid;
  }
}
