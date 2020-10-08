import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { CustomValidators } from 'src/app/core/validators/validators';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.scss']
})
export class RegisterComponent {

  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthenticationService) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      passwordConfirm: ['', Validators.compose([Validators.required, Validators.minLength(8)])]
    },
    { validator: CustomValidators.passwordMatchValidator });
  }

  doRegister() {
    this.authService.signUp(this.loginForm.value.email, this.loginForm.value.password);
  }

  get getPassword() {
    return this.loginForm.get('password');
  }

  get getPasswordConfirm() {
    return this.loginForm.get('passwordConfirm');
  }

  get getEmail() {
    return this.loginForm.get('email');
  }

  isInValid() {
    return this.loginForm.invalid && this.getPassword !== this.getPasswordConfirm;
  }
}
