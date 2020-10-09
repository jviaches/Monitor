import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { GeneralService } from 'src/app/core/services/general.service';
import { UserResendConfirmationComponent } from '../user-resend-confirmation/user-resend-confirmation.component';

@Component({
  selector: 'app-user-forget-password',
  templateUrl: './user-forget-password.component.html',
  styleUrls: ['./user-forget-password.component.scss']
})
export class UserForgetPasswordComponent {

  codeForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthenticationService, private generalService: GeneralService) {
    this.codeForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  sendPassword() {
    this.authService.forgotPassword(this.getEmail.value);
  }

  resendCode() {
    this.generalService.showModalComponent(UserResendConfirmationComponent, 'Send confirmation code');
  }

  get getEmail() {
    return this.codeForm.get('email');
  }

  isInValid() {
    return this.codeForm.invalid;
  }
}
