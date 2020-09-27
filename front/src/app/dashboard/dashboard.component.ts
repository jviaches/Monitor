import { Component, OnInit } from '@angular/core';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { GeneralService } from '../core/services/general.service';
import { AuthorizationService } from '../core/services/authentication.service';
import { UserChangePasswordComponent } from '../user/user-change-password/user-change-password.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  constructor(public dialog: MatDialog, private generalService: GeneralService,
              public authService: AuthorizationService, private router: Router) {
  }

  ngOnInit(): void {
  }

  logOff() {
    this.generalService.showYesNoModalMessage().subscribe(data => {
      if (data === 'yes') {
        this.authService.logOut();
      }
    });
  }

  redirectToDashboard() {
    this.router.navigate(['/dashboard']);
  }

  redirectToUserProfile() {
    this.router.navigate(['/dashboard/profile']);
  }
}
