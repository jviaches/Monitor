import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthorizationService } from 'src/app/core/services/authentication.service';
import { CustomValidators } from 'src/app/core/validators/validators';

@Component({
  selector: 'app-user-new-password',
  templateUrl: './user-new-password.component.html',
  styleUrls: ['./user-new-password.component.scss']
})
export class UserNewPasswordComponent {

  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthorizationService) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      code: ['', [Validators.required, Validators.minLength(1)]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      passwordConfirm: ['', Validators.compose([Validators.required, Validators.minLength(8)])]
    },
    { validator: CustomValidators.passwordMatchValidator });
  }

  doSubmit() {
    this.authService.forgotPasswordSubmit(this.loginForm.value.email, this.loginForm.value.code, this.loginForm.value.password);
  }

  get getEmail() {
    return this.loginForm.get('email');
  }

  get getCode() {
    return this.loginForm.get('code');
  }

  get getPassword() {
    return this.loginForm.get('password');
  }

  get getPasswordConfirm() {
    return this.loginForm.get('passwordConfirm');
  }

  isInValid() {
    return this.loginForm.invalid;
  }
}
