import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { GeneralService } from 'src/app/core/services/general.service';
import { UserResendConfirmationComponent } from '../user-resend-confirmation/user-resend-confirmation.component';
import { AuthFlow } from 'src/app/core/enums/enums';
import { AuthenticationService } from 'src/app/core/services/authentication.service';

@Component({
  selector: 'app-user-confirmation',
  templateUrl: './user-confirmation.component.html',
  styleUrls: ['./user-confirmation.component.scss']
})
export class UserConfirmationComponent implements OnInit {

  codeForm: FormGroup;
  authFlow: AuthFlow;

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private generalService: GeneralService,
              private authService: AuthenticationService, private router: Router) {
    this.codeForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      code: ['', [Validators.required, Validators.maxLength(20)]],
    });

    this.authFlow = (AuthFlow)[this.route.snapshot.paramMap.get('id')];
  }

  ngOnInit() {
  }

  doValidate() {
    // if (this.authFlow.toString() === AuthFlow[AuthFlow.Register]) {
    //   this.authService.confirmSignUp(this.getEmail.value, this.getCode.value);
    // } else if (this.authFlow.toString() === AuthFlow[AuthFlow.ForgetPassword]) {
    //   this.router.navigate(['/new-password'], { state: { email: this.getEmail.value, code: this.getCode.value}});
    // }
  }

  resendCode() {
    // this.generalService.showModalComponent(UserResendConfirmationComponent, 'Send confirmation code');
    this.router.navigateByUrl('/resend-code');
  }

  get getEmail() {
    return this.codeForm.get('email');
  }

  get getCode() {
    return this.codeForm.get('code');
  }

  isInValid() {
    return this.codeForm.invalid;
  }
}
