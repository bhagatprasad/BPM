import { Injectable, Service } from '@angular/core';
import { ApiService } from '../common/services/api.service';
import { Observable } from 'rxjs';
import { UpdateUserRequest, UpdateUserResponse } from '../models/user-profile';

@Injectable({
  providedIn: 'root'
})
export class UserService {
constructor(private apiService: ApiService) {}

  
  updateUserProfile(userId: string, data: UpdateUserRequest): Observable<UpdateUserResponse> {
    // Replace {userId} in the endpoint with actual userId
    const endpoint = `User/updateuser/${userId}`;
    
    return this.apiService.send<UpdateUserResponse>(
      'POST',
      endpoint,
      data
    );
  }

   updateUserProfilePut(userId: string, data: UpdateUserRequest): Observable<UpdateUserResponse> {
    const endpoint = `User/updateuser/${userId}`;
    
    return this.apiService.send<UpdateUserResponse>(
      'PUT',
      endpoint,
      data
    );
  }

  
}

