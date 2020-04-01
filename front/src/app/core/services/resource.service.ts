import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IResource } from '../models/resource.model';

@Injectable({
    providedIn: 'root'
})
export class ResourceService {

    constructor(private httpClient: HttpClient) { }

    getResources(userId: number): Observable<IResource[]> {
        return this.httpClient.get<IResource[]>(`https://localhost:44356/api/Resources/GetByUserId/${userId}`);
    }

    addResource(resource: any): Observable<any> {
        return this.httpClient.post<any>('https://localhost:44356/api/Resources', resource);
    }

    updateResource(resource: any): Observable<any> {
        return this.httpClient.post<any>('https://localhost:44356/api/Resources/Update', resource);
    }

    deleteResource(resource: any): Observable<any> {
        return this.httpClient.request('delete', 'https://localhost:44356/api/Resources', { body: resource });
    }
}
