import { Injectable, Service } from '@angular/core';
import { ApiService } from '../common/services/api.service';
import { Observable } from 'rxjs';
import { ChangePasswordRequest, ChangePasswordResponse, UpdateUserRequest, UpdateUserResponse } from '../models/user-profile';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
 
  constructor(private apiService: ApiService) { }


  updateUserProfile(userId: string, data: UpdateUserRequest): Observable<UpdateUserResponse> {
    // Replace {userId} in the endpoint with actual userId
    const endpoint = `${environment.UrlConstants.User.UpdateUserProfileAsync}/${userId}`;
    return this.apiService.send<UpdateUserResponse>('PUT', endpoint, data);
  }

  updatedChangePassword(userId: string, data: ChangePasswordRequest): Observable<ChangePasswordResponse> {
    const endpoint = `${environment.UrlConstants.User.ChangePasswordAsync}/${userId}`;
    return this.apiService.send<ChangePasswordResponse>('PUT', endpoint, data)
  }


}

