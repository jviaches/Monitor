import { ResourceService } from './core/services/resource.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { HttpClientModule, HttpClientJsonpModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { PeriodicyPipe } from './core/pipes/periodicy.pipe';
import { ChartModule } from 'angular-highcharts';
import { ModalDialogComponent } from './core/components/modal-dialog/modal-dialog.component';
import { GeneralService } from './core/services/general.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpErrorInterceptor } from './core/interceptors/error-interceptor';
import { ModalYesNoDialogComponent } from './core/components/yesno-modal-dialog/yesno-modal-dialog.component';
import { AuthorizationService } from './core/services/authentication.service';
import { LoginComponent } from './user/user-login/user-login.component';
import { RegisterComponent } from './user/user-register/user-register.component';
import { UserConfirmationComponent } from './user/user-confirmation/user-confirmation.component';
import { AmplifyAngularModule, AmplifyService, AmplifyModules } from 'aws-amplify-angular';
import Auth from '@aws-amplify/auth';
import Interactions from '@aws-amplify/interactions';
import Storage from '@aws-amplify/storage';
import { UserResendConfirmationComponent } from './user/user-resend-confirmation/user-resend-confirmation.component';
import { UserForgetPasswordComponent } from './user/user-forget-password/user-forget-passwordcomponent';
import { UserNewPasswordComponent } from './user/user-new-password/user-new-password.component';
import { JwtInterceptor } from './core/interceptors/jwt-interceptor';
import { UserChangePasswordComponent } from './user/user-change-password/user-change-password.component';
import { FooterComponent } from './pages/footer/footer.component';
import { MainPageComponent } from './pages/main/main.component';
import { PrivacyPolicyComponent } from './pages/privacy-policy/privacy-policy.component';
import { HeaderMainComponent } from './pages/header-main/header-main.component';
import { TermsOfServiceComponent } from './pages/terms-of-service/terms-of-service.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { ModalLoaderDialogComponent } from './core/components/modal-loader-dialog/modal-loader-dialog.component';
// import { UserProfileComponent } from './user/user-profile/user-profile.component';

@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    DashboardComponent,
    PeriodicyPipe,
    ModalDialogComponent,
    ModalLoaderDialogComponent,
    ModalYesNoDialogComponent,
    LoginComponent,
    RegisterComponent,
    UserConfirmationComponent,
    UserResendConfirmationComponent,
    UserForgetPasswordComponent,
    UserNewPasswordComponent,
    UserChangePasswordComponent,
    PageNotFoundComponent,
    PrivacyPolicyComponent,
    FooterComponent,
    HeaderMainComponent,
    TermsOfServiceComponent,
    // UserProfileComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    ChartModule,
    HttpClientModule, HttpClientJsonpModule,
    FormsModule, ReactiveFormsModule,
    AmplifyAngularModule
  ],
  providers: [ ResourceService, GeneralService, AuthorizationService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: AmplifyService, useFactory:  () => {
        return AmplifyModules({
          Auth,
          Storage,
          Interactions
        });
      }
    }],
  bootstrap: [AppComponent],
  entryComponents: [ModalDialogComponent, ModalYesNoDialogComponent, ModalLoaderDialogComponent]
})
export class AppModule { }
