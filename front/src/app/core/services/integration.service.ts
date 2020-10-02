import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GeneralService } from './general.service';
import { IIntegrationSettings } from '../models/integration-settings.model';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class IntegrationSettingsService {

    constructor(private httpClient: HttpClient, private generalService: GeneralService) {
    }

    addDefaultIntegrationSettings(email: string): Observable<IIntegrationSettings> {
        const resource = {
            UserEmail: email
        };

        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8');

        // tslint:disable-next-line:max-line-length
        return this.httpClient.post<IIntegrationSettings>(this.generalService.URL + 'IntegrationSettings/AddDefault', JSON.stringify(resource), { headers });
    }
}
