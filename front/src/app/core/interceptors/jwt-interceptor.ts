import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { GeneralService } from '../services/general.service';
import { AuthorizationService } from '../services/authentication.service';


@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private autService: AuthorizationService, private generalService: GeneralService) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const authorizedUser = this.autService.isLoggedIn();
        if (authorizedUser) {
            const helper = new JwtHelperService();
            const isExpired = helper.isTokenExpired(this.autService.getUserToken());

            if (isExpired) {
                this.autService.sessionExpired();

            } else {
                // console.log(this.autService.getUserToken());
                request = request.clone({
                    setHeaders: {
                        Authorization: `Bearer ${this.autService.getUserToken()}`
                    }
                });
            }
            return next.handle(request);
        }
    }
}
