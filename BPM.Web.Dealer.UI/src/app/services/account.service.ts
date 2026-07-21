import { Injectable } from '@angular/core';
import { ApiService } from '../common/services/api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private apiService: ApiService) {}

  authenticateAsync(loginObj: any): Observable<any> {
    return this.apiService.send<any>(
      'POST',
      'Account/authenticate',
      loginObj
    );
  }

  async isAuthenticated(): Promise<boolean> {
    const loggedData = localStorage.getItem('AuthenticatedUserResponse');

    if (!loggedData) {
      return false;
    }

    const authResponse = JSON.parse(loggedData);
    return !!authResponse?.jwtToken;
  }
}