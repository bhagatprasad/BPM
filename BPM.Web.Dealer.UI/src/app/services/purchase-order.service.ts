//import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../common/services/api.service';
import { environment } from '../../environments/environment';

// @Injectable({
//   providedIn: 'root',
// })
// export class PurchaseOrderService {
//   private apiUrl = 'http://localhost:5067/api/PurchaseOrder';
//   constructor(
//     private http: HttpClient,
//     private apiService: ApiService,
//   ) {}

//   //create purchaseorder
//   createPurchaseOrder(request: any): Observable<any> {
//     return this.http.post(`${this.apiUrl}/CreatePurchaseOrder`, request);
//   }

//   //
//   getOrdersByDealer(dealerId: string): Observable<any> {
//     return this.apiService.send<any>('GET', `PurchaseOrder/FetchPurchaseOrderByDealer/${dealerId}`);
//   }
// }
@Injectable({
  providedIn: 'root',
})
export class PurchaseOrderService {
  constructor(private apiService: ApiService) {}

  //create purchase order
  createPurchaseOrder(request: any): Observable<any> {
    return this.apiService.send<any>(
      'POST',
      environment.UrlConstants.PurchaseOrder.CreatePurchaseOrder,
      request,
    );
  }

  //fetch orders by dealerId
  getOrdersByDealer(dealerId: string): Observable<any> {
    return this.apiService.send<any>(
      'GET',
      `${environment.UrlConstants.PurchaseOrder.FetchPurchaseOrderByDealer}/${dealerId}`,
    );
  }

  //fetch order by purchaseorderbyid
  getPurchaseOrderById(id: string): Observable<any> {
    return this.apiService.send<any>(
      'GET',
      `${environment.UrlConstants.PurchaseOrder.FetchPurchaseOrderById}/${id}`,
    );
  }
}
