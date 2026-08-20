import { Injectable } from '@angular/core';
import { ApiService } from '@app/common/services/api.service';
import { userInformation, CreateUserDto, UserDistributorUpdateDto } from '@app/models/user';
import { UserUpdateDto } from '@app/models/user-update-dto';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private apiService: ApiService) {}

  getAllUsersByDealerId(dealerId: string): Observable<userInformation[]> {
    const endpoint = `${environment.UrlConstants.User.GetAllUsersByDealerIdAsync}/${dealerId}`;

    return this.apiService.send<userInformation[]>('GET', endpoint);
  }

  getAllUsersByDistributorId(distributorId: string): Observable<userInformation[]> {
    const endpoint = `${environment.UrlConstants.User.GetAllUsersByDistributorIdAsync}/${distributorId}`;

    return this.apiService.send<userInformation[]>('GET', endpoint);
  }

  updateUserDistributor(updateDto: UserDistributorUpdateDto): Observable<any> {
    const endpoint = `${environment.UrlConstants.User.UpdateUserDistributorAsync}`;

    return this.apiService.send<any>('PUT', endpoint, updateDto);
  }

  insertUserAsync(createUserDto: CreateUserDto): Observable<any> {
    const endpoint = `${environment.UrlConstants.User.InsertUserAsync}`;

    return this.apiService.send<any>('POST', endpoint, createUserDto);
  }

  updateUserAsync(userId: string, updateUserDto: UserUpdateDto): Observable<userInformation> {
    const endpoint = `${environment.UrlConstants.User.updateUserAsync}/${userId}`;

    return this.apiService.send<userInformation>('PUT', endpoint, updateUserDto);
  }

  deactivateUserAsync(deactivateUserDto: any): Observable<any> {
    const endpoint = `${environment.UrlConstants.User.deactivateUserAsync}`;

    return this.apiService.send<any>('POST', endpoint, deactivateUserDto);
  }
}
