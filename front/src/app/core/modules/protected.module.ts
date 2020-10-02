import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HttpClientJsonpModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AmplifyAngularModule, AmplifyService, AmplifyModules } from 'aws-amplify-angular';
import Auth from '@aws-amplify/auth';
import Interactions from '@aws-amplify/interactions';
import Storage from '@aws-amplify/storage';
import { AppComponent } from 'src/app/app.component';
import { DashboardComponent } from 'src/app/dashboard/dashboard.component';
import { PeriodicyPipe } from '../pipes/periodicy.pipe';
import { ModalDialogComponent } from '../components/modal-dialog/modal-dialog.component';
import { ModalLoaderDialogComponent } from '../components/modal-loader-dialog/modal-loader-dialog.component';
import { ModalYesNoDialogComponent } from '../components/yesno-modal-dialog/yesno-modal-dialog.component';
import { UserChangePasswordComponent } from 'src/app/user/user-change-password/user-change-password.component';
import { ResourceDetailsComponent } from 'src/app/resource/resource-details/resource-details.component';
import { ResourceListComponent } from 'src/app/resource/resource-list/resource-list.component';
import { UserProfileComponent } from 'src/app/user/user-profile/user-profile.component';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from 'src/app/material.module';
import { ChartModule } from 'angular-highcharts';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ExternalModuleModule } from './external-module';
import { ResourceService } from '../services/resource.service';
import { GeneralService } from '../services/general.service';
import { AuthorizationService } from '../services/authentication.service';
import { JwtInterceptor, JwtModule } from '@auth0/angular-jwt';
import { JwtHelperService, JWT_OPTIONS  } from '@auth0/angular-jwt';
import { HttpErrorInterceptor } from '../interceptors/error-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    PeriodicyPipe,
    ModalDialogComponent,
    ModalLoaderDialogComponent,
    ModalYesNoDialogComponent,
    UserChangePasswordComponent,
    ResourceDetailsComponent,
    ResourceListComponent,
    UserProfileComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    ChartModule,
    HttpClientModule, HttpClientJsonpModule,
    FormsModule, ReactiveFormsModule,
    AmplifyAngularModule,
    ExternalModuleModule,
    JwtModule
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
    },
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService]
})
export class ProtectedModule { }
