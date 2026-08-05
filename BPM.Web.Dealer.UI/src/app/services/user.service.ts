import { Injectable } from '@angular/core';
import { ApiService } from '@app/common/services/api.service';
import { userInformation, usersRequest,} from '@app/models/user';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root', })

export class UserDetailsService {

    constructor(private apiService: ApiService) { }

    getAllUsersByDealerId(dealerId: string): Observable<userInformation[]> {
        const endpoint = `${environment.UrlConstants.User.GetAllUsersByDealerIdAsync}/${dealerId}`;
        return this.apiService.send<userInformation[]>('GET', endpoint);
    }
    
}