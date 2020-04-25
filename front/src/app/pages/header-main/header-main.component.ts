import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from 'src/app/core/services/authentication.service';
import { GeneralService } from 'src/app/core/services/general.service';

@Component({
  selector: 'app-header-main',
  templateUrl: './header-main.component.html',
  styleUrls: ['./header-main.component.scss']
})
export class HeaderMainComponent implements OnInit {

  isAuthorized = false;

  constructor(private router: Router, private authorizationService: AuthorizationService,
              private generalService: GeneralService) { }

  ngOnInit() {
    if (this.authorizationService.isLoggedIn()) {
      this.isAuthorized = true;
    }
  }

  logOff() {
    this.generalService.showYesNoModalMessage().subscribe(data => {
      if (data === 'yes') {
        this.authorizationService.logOut();
      }
    });
  }
}
