import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot } from '@angular/router';
import { AuthorizationService } from '../services/authentication.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate, CanActivateChild {
    constructor(
        private router: Router,
        private authService: AuthorizationService
    ) {
    }

    canActivate(router: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

        const result = this.authService.isLoggedIn();

        if (result === false) {
            // not logged in so redirect to login page with the return url
            this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
            return false;
        }

        return result;
    }

    canActivateChild(router: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const result = this.authService.isLoggedIn();

        if (result === false) {
            // not logged in so redirect to login page with the return url
            this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
            return false;
        }

        return result;
    }
}
