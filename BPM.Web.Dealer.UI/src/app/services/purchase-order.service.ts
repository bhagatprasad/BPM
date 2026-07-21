import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PurchaseOrderService {
  private apiUrl = 'http://localhost:5067/api/PurchaseOrder';
  constructor(private http: HttpClient) {}
  createPurchaseOrder(request: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/CreatePurchaseOrder`, request);
  }
}
