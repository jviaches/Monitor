import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthorizationService } from 'src/app/core/services/authentication.service';
import { Router } from '@angular/router';
import { GeneralService } from 'src/app/core/services/general.service';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthorizationService,
              private router: Router, private generalService: GeneralService) {
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  ngOnInit() {
  }

  doLogin() {
    this.authService.signIn(this.loginForm.value.userName, this.loginForm.value.password).subscribe( () => {
      this.router.navigateByUrl('/dashboard');
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

  isInValid() {
    return this.loginForm.invalid;
  }
}
