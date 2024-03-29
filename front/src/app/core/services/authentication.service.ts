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
        this.generalService.showActionConfirmationFail('Password reset required.');
        return err.code;
      } else if (err.code === 'NotAuthorizedException') {
        this.generalService.showActionConfirmationFail('Invalid email or password');
      } else if (err.code === 'UserNotFoundException') {
        this.generalService.showActionConfirmationFail('Invalid email or password');
      } else {
        this.generalService.showActionConfirmationFail('Invalid email or password');
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
      .catch(err => this.generalService.showActionConfirmationFail(err.message));
  }

  async confirmSignUp(username: string, code: string): Promise<any> {
    Auth.confirmSignUp(username, code)
      .then(data => this.router.navigateByUrl('/login'))
      .catch(err => this.generalService.showActionConfirmationFail(err.message));
  }

  resendSignUp(username: string) {
    Auth.resendSignUp(username)
      .then(data => this.router.navigateByUrl(`/user-confirmation/${AuthFlow.Register}`))
      .catch(err => this.generalService.showActionConfirmationFail(err.message));
  }

  forgotPassword(username: string) {
    Auth.forgotPassword(username)
      .then(data => this.router.navigateByUrl(`/user-confirmation/${AuthFlow.ForgetPassword}`))
      .catch(err => this.generalService.showActionConfirmationFail(err.message));
  }

  forgotPasswordSubmit(username: string, code, newPassword) {
    Auth.forgotPasswordSubmit(username, code, newPassword)
      .then(data => this.router.navigateByUrl('/login'))
      .catch(err => console.log(err));
  }

  changePassword(oldPassword: string, newPassword: string): Promise<any> {
    return Auth.currentAuthenticatedUser().then(user => {
        return Auth.changePassword(user, oldPassword, newPassword);
    })
    .catch(err => {
      this.generalService.showActionConfirmationFail(err.message);
      return null;
    });
  }

  getUserName(): string {
    return JSON.parse(localStorage.getItem('user')).username;
  }

  getUserEmail(): string {
    return JSON.parse(localStorage.getItem('user')).attributes.email;
  }

  isLoggedIn() {
    return localStorage.getItem('user') !== null;
  }

  getUserToken(): string {
    return JSON.parse(localStorage.getItem('user')).signInUserSession.idToken.jwtToken;
  }

  logOut() {
    Auth.signOut({ global: true }).finally(() => {
      localStorage.removeItem('user');
      this.user = null;
      window.location.reload();
    });
  }
}
