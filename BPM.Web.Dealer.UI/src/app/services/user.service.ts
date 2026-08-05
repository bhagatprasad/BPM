import { Injectable } from '@angular/core';
import { ApiService } from '@app/common/services/api.service';
import { CreateUserDto } from '@app/models/create-user-dto';
import { userInformation, usersRequest, } from '@app/models/user';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root', })

export class UserService {

    constructor(private apiService: ApiService) { }

    getAllUsersByDealerId(dealerId: string): Observable<userInformation[]> {
        const endpoint = `${environment.UrlConstants.User.GetAllUsersByDealerIdAsync}/${dealerId}`;
        return this.apiService.send<userInformation[]>('GET', endpoint);
    }

    InsertUser(userData: userInformation): Observable<any> {
        const endpoint = `${environment.UrlConstants.User.InsertUserAsync}`;
        return this.apiService.send<any>('POST', endpoint, userData);
    }
    insertUserAsync(createUserDto: CreateUserDto): Observable<any> {
        const endpoint = `${environment.UrlConstants.User.InsertUserAsync}`;
        return this.apiService.send<any>('POST', endpoint, createUserDto);
    }
}