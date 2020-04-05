import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthorizationService } from 'src/app/core/services/authentication.service';
import { Router } from '@angular/router';
import { GeneralService } from 'src/app/core/services/general.service';
import { CustomValidators } from 'src/app/core/validators/validators';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.scss']
})
export class RegisterComponent implements OnInit {

  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthorizationService,
              private router: Router, private generalService: GeneralService) {
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required]],
      email: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      passwordConfirm: ['', Validators.compose([Validators.required])]
    },
    {
       validator: CustomValidators.passwordMatchValidator
    });
  }

  ngOnInit() {
  }

  doRegister() {
    this.authService.register(this.loginForm.value.userName, this.loginForm.value.password, this.loginForm.value.email).subscribe( data => {
      // this.router.navigateByUrl('/dashboard');
      console.log(data);
    }, (err) => {
      this.generalService.showActionConfirmation(err.message);
    });
  }

  get getUserName() {
    return this.loginForm.get('userName');
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
    return this.loginForm.invalid;
  }
}
