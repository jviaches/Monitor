import { ResourceService } from './core/services/resource.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { LandingPageComponent } from './landing/landing.component';
import { HttpClientModule } from '@angular/common/http';
import { PeriodicyPipe } from './core/pipes/periodicy.pipe';
import { ChartModule } from 'angular-highcharts';
import { ModalDialogComponent } from './core/components/modal-dialog/modal-dialog.component';
import { GeneralService } from './core/services/general.service';
import { ResourceAddComponent } from './resources/resource-add/resource-add.component';

@NgModule({
  declarations: [
    AppComponent,
    LandingPageComponent,
    DashboardComponent,
    PeriodicyPipe,
    ModalDialogComponent,
    ResourceAddComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    ChartModule,
    HttpClientModule
  ],
  providers: [ResourceService, GeneralService],
  bootstrap: [AppComponent],
  entryComponents: [ModalDialogComponent]
})
export class AppModule { }
