import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthenticationService } from '../services/authentication.service';


@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private authService: AuthenticationService) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const authorizedUser = this.authService.isUserLoggedIn();

        if (authorizedUser) {
            const helper = new JwtHelperService();
            const isExpired = helper.isTokenExpired(this.authService.currentUserValue.token);

            if (isExpired) {
                this.authService.logout();

            } else {
                // console.log(this.autService.getUserToken());
                request = request.clone({
                    setHeaders: {
                        Authorization: `Bearer ${this.authService.currentUserValue.token}`
                    }
                });
            }
        }

        return next.handle(request);
    }
}
