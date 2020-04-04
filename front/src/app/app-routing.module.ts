import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LandingPageComponent } from './landing/landing.component';
import { LoginComponent } from './user/user-login/user-login.component';
import { AuthGuard } from './core/guards/auth-guard';
import { RegisterComponent } from './user/user-register/user-register.component';
import { UserConfirmationComponent } from './user/user-confirmation/user-confirmation.component';


const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'user-confirmation', component: UserConfirmationComponent },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
