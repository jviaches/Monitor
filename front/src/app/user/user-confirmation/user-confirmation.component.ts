import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthorizationService } from 'src/app/core/services/authentication.service';
import { Router } from '@angular/router';
import { GeneralService } from 'src/app/core/services/general.service';

@Component({
  selector: 'app-user-confirmation',
  templateUrl: './user-confirmation.component.html',
  styleUrls: ['./user-confirmation.component.scss']
})
export class UserConfirmationComponent implements OnInit {

  codeForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthorizationService,
              private router: Router, private generalService: GeneralService) {
    this.codeForm = this.fb.group({
      userName: ['', [Validators.required, Validators.maxLength(20)]],
      code: ['', [Validators.required, Validators.maxLength(20)]],
    });
  }

  ngOnInit() {
  }

  doValidate() {
    this.authService.confirmAuthCode(this.getCode.value, this.getUserName.value).subscribe(data => {
      if (data === 'SUCCESS') {
        this.router.navigateByUrl('/login');
      }
    },
      err => this.generalService.showActionConfirmation('Combination of user name and validation code is invalid.')
    );
  }

  get getUserName() {
    return this.codeForm.get('userName');
  }

  get getCode() {
    return this.codeForm.get('code');
  }

  isInValid() {
    return this.codeForm.invalid;
  }
}
