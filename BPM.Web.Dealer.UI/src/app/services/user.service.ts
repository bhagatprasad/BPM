import { Injectable } from '@angular/core';
import { ApiService } from '@app/common/services/api.service';
import { CreateUserDto } from '@app/models/create-user-dto';
import { userInformation, usersRequest, } from '@app/models/user';
import { UserDeactivateDto } from '@app/models/user-deactivate-dto';
import { UserUpdateDto } from '@app/models/user-update-dto';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root', })

export class UserService {

    constructor(private apiService: ApiService) { }

    getAllUsersByDealerId(dealerId: string): Observable<userInformation[]> {
        const endpoint = `${environment.UrlConstants.User.GetAllUsersByDealerIdAsync}/${dealerId}`;
        return this.apiService.send<userInformation[]>('GET', endpoint);
    }
   
    insertUserAsync(createUserDto: CreateUserDto): Observable<any> {
        const endpoint = `${environment.UrlConstants.User.InsertUserAsync}`;
        return this.apiService.send<any>('POST', endpoint, createUserDto);
    }

    updateUserAsync(userId:string, updateUserDto: UserUpdateDto):Observable<userInformation>{
        const endpoint = `${environment.UrlConstants.User.updateUserAsync}/${userId}`;
        return this.apiService.send<userInformation>('PUT', endpoint,updateUserDto);
    }

     deactivateUserAsync(deactivateUserDto:any):Observable<any>{
        const endpoint = `${environment.UrlConstants.User.deactivateUserAsync}`;
        return this.apiService.send<any>('POST',endpoint,deactivateUserDto);
     }
}