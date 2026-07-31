import { Injectable, } from '@angular/core';
import { ApiService } from '../common/services/api.service';
import { UpdatedDealerResponse, UpdatedDealerRequest } from '../models/dealer-profile';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DealerService {

  constructor(private apiService: ApiService) { }

  updateDealerAsync(dealerId: string, Data: UpdatedDealerRequest): Observable<UpdatedDealerResponse> {
    var url = `${environment.UrlConstants.Dealer.updateDealerAsync}/${dealerId}`;
    return this.apiService.send<UpdatedDealerResponse>('PUT', url, Data);
  }
}
