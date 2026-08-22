import { Injectable } from '@angular/core';
import { ApiService } from '@app/common/services/api.service';
import { DistributorInfo } from '@app/models/distributor';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DistributorService 
{
  constructor(private apiService: ApiService) {} 
  getAllDistributors(): Observable<DistributorInfo[]> {
    const url = environment.UrlConstants.Distributor.GetAllDistributors;
    return this.apiService.send<DistributorInfo[]>('GET', url);
  }
   getDistributorById(): Observable<DistributorInfo> {
      const url = environment.UrlConstants.Distributor.GetAllDistributors;
      return this.apiService.send<DistributorInfo>('GET', url);    
  }
}
