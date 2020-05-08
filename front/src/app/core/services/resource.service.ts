import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IResource } from '../models/resource.model';
import { GeneralService } from './general.service';
import { AuthorizationService } from './authentication.service';

@Injectable({
    providedIn: 'root'
})
export class ResourceService {

    constructor(private httpClient: HttpClient, private generalService: GeneralService, private authService: AuthorizationService) { }

    getResources(): Observable<IResource[]> {
        return this.httpClient.get<IResource[]>(this.generalService.URL + `Resources/GetByUserId/${this.authService.getUserName()}`);
    }

    addResource(resource: any): Observable<any> {
        return this.httpClient.post<any>(this.generalService.URL + 'Resources', resource);
    }

    updateResource(resource: any): Observable<any> {
        return this.httpClient.post<any>(this.generalService.URL + 'Resources/Update', resource);
    }

    deleteResource(id: number): Observable<any> {
        return this.httpClient.delete(this.generalService.URL + `Resources/Delete/${id}`);
    }
}
