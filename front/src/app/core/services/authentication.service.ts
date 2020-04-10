import { Injectable } from '@angular/core';
import { Auth } from 'aws-amplify';
import { GeneralService } from './general.service';
import { Router } from '@angular/router';
import { AuthFlow } from '../enums/enums';

// source: https://aws-amplify.github.io/docs/js/authentication

@Injectable()
export class AuthorizationService {

  private user: any;

  constructor(private router: Router, private generalService: GeneralService) { }

  async SignIn(email: string, password: string) {
    try {
      this.user = await Auth.signIn(email, password);

      if (this.user.challengeName === 'NEW_PASSWORD_REQUIRED') {
        console.log('NEW_PASSWORD_REQUIRED');
      } else {
        localStorage.setItem('user', JSON.stringify(this.user));
        this.router.navigate(['/dashboard']);
      }
    } catch (err) {
      if (err.code === 'UserNotConfirmedException') {
        // The error happens if the user didn't finish the confirmation step when signing up
        // In this case you need to resend the code and confirm the user
        // About how to resend the code and confirm the user, please check the signUp part
        this.generalService.showActionRedirectConfirmation(
          'This account is not confirmet yet.\r\nPlease click on close button for redirect.', `/user-confirmation/${AuthFlow.Register}`);
      } else if (err.code === 'PasswordResetRequiredException') {
        // The error happens when the password is reset in the Cognito console
        // In this case you need to call forgotPassword to reset the password
        // Please check the Forgot Password part.
        this.generalService.showActionConfirmation('Password reset required.');
        return err.code;
      } else if (err.code === 'NotAuthorizedException') {
        this.generalService.showActionConfirmation('This user is not authorized.');
      } else if (err.code === 'UserNotFoundException') {
        this.generalService.showActionConfirmation('Invalid email or password');
      } else {
        this.generalService.showActionConfirmation('Invalid email or password');
      }
    }
  }

  async signUp(username: string, password: string): Promise<any> {
    Auth.signUp({
      username,
      password,
      attributes: {
        // email,          // optional
        // phone_number,   // optional - E.164 number convention
        // other custom attributes
      },
      validationData: []  // optional
    })
      .then(data => this.router.navigateByUrl(`/user-confirmation/${AuthFlow.Register}`))
      .catch(err => this.generalService.showActionConfirmation(err.message));
  }

  async confirmSignUp(username: string, code: string): Promise<any> {
    Auth.confirmSignUp(username, code)
      .then(data => this.router.navigateByUrl('/login'))
      .catch(err => this.generalService.showActionConfirmation(err.message));
  }

  resendSignUp(username: string) {
    Auth.resendSignUp(username)
      .then(data => this.router.navigateByUrl(`/user-confirmation/${AuthFlow.Register}`))
      .catch(err => this.generalService.showActionConfirmation(err.message));
  }

  forgotPassword(username: string) {
    Auth.forgotPassword(username)
      .then(data => this.router.navigateByUrl(`/user-confirmation/${AuthFlow.ForgetPassword}`))
      .catch(err => this.generalService.showActionConfirmation(err.message));
  }

  forgotPasswordSubmit(username: string, code, newPassword) {
    Auth.forgotPasswordSubmit(username, code, newPassword)
      .then(data => this.router.navigateByUrl('/login'))
      .catch(err => console.log(err));
  }

  getUserName(): string {
    return JSON.parse(localStorage.getItem('user')).username;
  }

  getUserEmail(): string {
    return this.user.attributes.email;
  }

  isLoggedIn() {
    return localStorage.getItem('user') !== null;
  }

  logOut() {
    Auth.signOut({ global: true }).finally(() => {
      localStorage.removeItem('user');
      this.user = null;
    });
  }
}
