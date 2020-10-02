import { NgModule } from '@angular/core';

import { IntegrationSettingsService } from '../services/integration.service';
import { MainPageComponent } from 'src/app/pages/main/main.component';
import { LoginComponent } from 'src/app/user/user-login/user-login.component';
import { RegisterComponent } from 'src/app/user/user-register/user-register.component';
import { UserConfirmationComponent } from 'src/app/user/user-confirmation/user-confirmation.component';
import { UserResendConfirmationComponent } from 'src/app/user/user-resend-confirmation/user-resend-confirmation.component';
import { UserForgetPasswordComponent } from 'src/app/user/user-forget-password/user-forget-passwordcomponent';
import { TermsOfServiceComponent } from 'src/app/pages/terms-of-service/terms-of-service.component';
import { PageNotFoundComponent } from 'src/app/pages/page-not-found/page-not-found.component';
import { PrivacyPolicyComponent } from 'src/app/pages/privacy-policy/privacy-policy.component';
import { UserNewPasswordComponent } from 'src/app/user/user-new-password/user-new-password.component';
import { HeaderMainComponent } from 'src/app/pages/header-main/header-main.component';
import { MaterialModule } from 'src/app/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ChartModule } from 'angular-highcharts';
import { HttpClientModule, HttpClientJsonpModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AmplifyAngularModule } from 'aws-amplify-angular';
import { HttpErrorInterceptor } from '../interceptors/error-interceptor';

@NgModule({
  declarations: [
    MainPageComponent,
    TermsOfServiceComponent,
    PageNotFoundComponent,
    PrivacyPolicyComponent,
    LoginComponent,
    RegisterComponent,
    UserConfirmationComponent,
    UserResendConfirmationComponent,
    UserForgetPasswordComponent,
    UserNewPasswordComponent,
    HeaderMainComponent,
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
  providers: [IntegrationSettingsService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true }]
})
export class ExternalModuleModule { }
