import { Injectable, Service } from '@angular/core';
import { ApiService } from '../common/services/api.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class AccountService {

    constructor(private apiService: ApiService) { }

    authenticateAsync(loginObj: any): Observable<any> {
        return this.apiService.send<any>('POST', 'Account/authenticate', loginObj);
    }
}