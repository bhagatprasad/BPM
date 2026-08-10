import { Injectable, Service } from '@angular/core';
import { ApiService } from '@app/common/services/api.service';
import { roleInfo } from '@app/models/user';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
constructor(private apiService: ApiService) { }

getAllRolesAsync(): Observable<roleInfo[]> {
    const endpoint = `${environment.UrlConstants.Role.GetAllRolesAsync}`;
    return this.apiService.send<roleInfo[]>('GET', endpoint);
  }

}
