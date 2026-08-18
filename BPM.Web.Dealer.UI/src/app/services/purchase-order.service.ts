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

  // Creates a new purchase order
  createPurchaseOrder(request: any): Observable<any> {
    return this.apiService.send<any>(
      'POST',
      environment.UrlConstants.PurchaseOrder.CreatePurchaseOrder,
      request,
    );
  }

  // Submits a draft purchase order for approval.
  submitPurchaseOrder(request: any): Observable<any> {
    return this.apiService.send<any>(
      'POST',
      environment.UrlConstants.PurchaseOrder.SubmitPurchaseOrder,
      request,
    );
  }

  // Fetches all purchase orders for a specific dealer
  getOrdersByDealer(dealerId: string): Observable<any> {
    return this.apiService.send<any>(
      'GET',
      `${environment.UrlConstants.PurchaseOrder.FetchPurchaseOrderByDealer}/${dealerId}`,
    );
  }

  // Fetches a purchase order by its ID
  getPurchaseOrderById(id: string): Observable<any> {
    return this.apiService.send<any>(
      'GET',
      `${environment.UrlConstants.PurchaseOrder.FetchPurchaseOrderById}/${id}`,
    );
  }

  // Saves a purchase order as a draft
  savePurchaseOrderDraft(request: any): Observable<any> {
    return this.apiService.send<any>(
      'POST',
      environment.UrlConstants.PurchaseOrder.SavePurchaseOrderDraft,
      request,
    );
  }

  // Fetches draft purchase orders for a specific dealer
  getDraftPurchaseOrders(dealerId: string): Observable<any> {
    return this.apiService.send<any>(
      'GET',
      `${environment.UrlConstants.PurchaseOrder.GetDraftPurchaseOrders}/${dealerId}`,
    );
  }

  // Process an existing purchase order
  processPurchaseOrder(request: any): Observable<any> {
    return this.apiService.send<any>(
      'POST',
      environment.UrlConstants.PurchaseOrder.ProcessPurchaseOrder,
      request,
    );
  }

  // Deletes a draft purchase order by its ID
  deletePurchaseOrderDraft(id: string): Observable<any> {
    return this.apiService.send<any>(
      'DELETE',
      `${environment.UrlConstants.PurchaseOrder.DeletePurchaseOrderDraft}/${id}`,
    );
  }
}
