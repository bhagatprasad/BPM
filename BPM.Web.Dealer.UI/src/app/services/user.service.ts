import { Injectable, Service } from '@angular/core';
import { ApiService } from '@app/common/services/api.service';
import { usersRequest, UsersResponse } from '@app/models/user';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root', })

export class UserService {
    constructor(private apiService: ApiService) { }

getAllUsersByDealerId(dealerId: string, data: usersRequest): Observable<UsersResponse> {
    const endponit=`${environment.UrlConstants.User.GetAllUsersByDealerIdAsync}/${dealerId}`;
    return this.apiService.send<UsersResponse>('GET', endponit);
}
