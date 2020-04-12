import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { AuthorizationService } from '../core/services/authentication.service';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss', 'global.css', ]
})
export class LandingPageComponent implements OnInit {

  @ViewChild('submitBtn', { read: ElementRef, static: false }) submitButton: ElementRef;
  @ViewChild('mainHeader', { read: ElementRef, static: false }) mainHeader: ElementRef;

  constructor(public authService: AuthorizationService) {
  }

  ngOnInit() {
  }

  logOut() {
    this.authService.logOut();
  }

  @HostListener('window:scroll', ['$event']) // for window scroll events
  onScroll(event) {
    const windowScroll = window.pageYOffset;
    if (windowScroll >= 10) {
      this.mainHeader.nativeElement.classList.add('header-style-four', 'fixed-header');
    } else {
      this.mainHeader.nativeElement.classList.remove('header-style-four', 'fixed-header');
    }
  }
}
