import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthorizationService } from 'src/app/core/services/authentication.service';

@Component({
  selector: 'app-user-resend-confirmation',
  templateUrl: './user-resend-confirmation.component.html',
  styleUrls: ['./user-resend-confirmation.component.scss']
})
export class UserResendConfirmationComponent {

  codeForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthorizationService) {
    this.codeForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  resendCode() {
    this.authService.resendSignUp(this.getEmail.value);
  }

  get getEmail() {
    return this.codeForm.get('email');
  }

  isInValid() {
    return this.codeForm.invalid;
  }
}
