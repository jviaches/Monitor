import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { CustomValidators } from 'src/app/core/validators/validators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-new-password',
  templateUrl: './user-new-password.component.html',
  styleUrls: ['./user-new-password.component.scss']
})
export class UserNewPasswordComponent {

  loginForm: FormGroup;
  email = '';
  verifCode = '';

  constructor(private fb: FormBuilder, private authService: AuthenticationService, private router: Router) {
    this.email = this.router.getCurrentNavigation().extras.state.email;
    this.verifCode = this.router.getCurrentNavigation().extras.state.code;
    console.log(this.email);
    console.log(this.verifCode);

    this.loginForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8)]],
      passwordConfirm: ['', Validators.compose([Validators.required, Validators.minLength(8)])]
    },
    { validator: CustomValidators.passwordMatchValidator });
  }

  doSubmit() {
    // this.authService.forgotPasswordSubmit(this.email, this.verifCode, this.loginForm.value.password);
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
