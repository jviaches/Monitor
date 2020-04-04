import { ResourceService } from './core/services/resource.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { LandingPageComponent } from './landing/landing.component';
import { HttpClientModule, HttpClientJsonpModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { PeriodicyPipe } from './core/pipes/periodicy.pipe';
import { ChartModule } from 'angular-highcharts';
import { ModalDialogComponent } from './core/components/modal-dialog/modal-dialog.component';
import { GeneralService } from './core/services/general.service';
import { ResourceAddComponent } from './resources/resource-add/resource-add.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpErrorInterceptor } from './core/interceptors/error-interceptor';
import { ResourceEditComponent } from './resources/resource-edit/resource-edit.component';
import { ModalYesNoDialogComponent } from './core/components/yesno-modal-dialog/yesno-modal-dialog.component';
import { AuthorizationService } from './core/services/authentication.service';
import { LoginComponent } from './user/user-login/user-login.component';
import { JwtInterceptor } from './core/interceptors/jwt-interceptor.service';
import { RegisterComponent } from './user/user-register/user-register.component';
import { UserConfirmationComponent } from './user/user-confirmation/user-confirmation.component';

@NgModule({
  declarations: [
    AppComponent,
    LandingPageComponent,
    DashboardComponent,
    PeriodicyPipe,
    ModalDialogComponent,
    ModalYesNoDialogComponent,
    ResourceAddComponent,
    ResourceEditComponent,
    LoginComponent,
    RegisterComponent,
    UserConfirmationComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    ChartModule,
    HttpClientModule, HttpClientJsonpModule,
    FormsModule, ReactiveFormsModule
  ],
  providers: [ ResourceService, GeneralService, AuthorizationService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }],
  bootstrap: [AppComponent],
  entryComponents: [ModalDialogComponent, ModalYesNoDialogComponent]
})
export class AppModule { }
