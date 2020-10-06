import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './user/user-login/user-login.component';
import { AuthGuard } from './core/guards/auth-guard';
import { RegisterComponent } from './user/user-register/user-register.component';
import { UserConfirmationComponent } from './user/user-confirmation/user-confirmation.component';
import { UserForgetPasswordComponent } from './user/user-forget-password/user-forget-passwordcomponent';
import { UserNewPasswordComponent } from './user/user-new-password/user-new-password.component';
import { UserResendConfirmationComponent } from './user/user-resend-confirmation/user-resend-confirmation.component';
import { MainPageComponent } from './pages/main/main.component';
import { PrivacyPolicyComponent } from './pages/privacy-policy/privacy-policy.component';
import { TermsOfServiceComponent } from './pages/terms-of-service/terms-of-service.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { ResourceDetailsComponent } from './resource/resource-details/resource-details.component';
import { ResourceListComponent } from './resource/resource-list/resource-list.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserProfileComponent } from './user/user-profile/user-profile.component';
// import { UserProfileComponent } from './user/user-profile/user-profile.component';


const routes: Routes = [
  { path: '', component: MainPageComponent },
  { path: 'privacy-policy', component: PrivacyPolicyComponent },
  { path: 'terms-of-service', component: TermsOfServiceComponent },
  { path: 'password-recovery', component: UserForgetPasswordComponent },
  { path: 'new-password', component: UserNewPasswordComponent },
  { path: 'resend-code', component: UserResendConfirmationComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'user-confirmation/:id', component: UserConfirmationComponent },
  { path: 'login', component: LoginComponent },
  { path: 'page-not-found', component: PageNotFoundComponent },
  {
    path: 'dashboard',
    canActivate: [AuthGuard],
    component: DashboardComponent,
    children: [
      { path: '', component: ResourceListComponent },
      { path: 'resource-details', component: ResourceDetailsComponent },
      { path: 'profile', component: UserProfileComponent }
    ]
  },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
