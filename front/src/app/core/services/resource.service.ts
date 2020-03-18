import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IResource } from '../models/resource.model';

@Injectable({
    providedIn: 'root'
})
export class ResourceService {

    constructor(private httpClient: HttpClient) { }

    getResources(userId: number): Observable<IResource[]> {
        return this.httpClient.get<IResource[]>(`https://localhost:44356/api/Resources/GetByUserId/${userId}`);
    }
}
