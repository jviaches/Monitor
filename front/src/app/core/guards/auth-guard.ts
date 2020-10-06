import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate, CanActivateChild {
    constructor(
        private router: Router,
        private authService: AuthenticationService
    ) {
    }

    canActivate(router: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

        const result = this.authService.isUserLoggedIn();

        if (result === false) {
            this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
            return false;
        }

        return result;
    }

    canActivateChild(router: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const result = this.authService.isUserLoggedIn();

        if (result === false) {
            this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
            return false;
        }

        return result;
    }
}
