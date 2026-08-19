import { Injectable } from '@angular/core';
import { ApiService } from '@app/common/services/api.service';
import { Warehouse, WarehouseCreateDto, WarehouseUpdateDto } from '@app/models/warehouse';
import { environment } from '@env/environment';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root',
})
export class WarehouseService {
  constructor(private apiService: ApiService) {}

  createWarehouse(dto: WarehouseCreateDto): Observable<Warehouse> {
    return this.apiService.send<Warehouse>(
      'POST',
      environment.UrlConstants.Warehouse.CreateWarehouse,
      dto,
    );
  }

  getAllWarehouses(): Observable<Warehouse[]> {
    return this.apiService.send<Warehouse[]>(
      'GET',
      environment.UrlConstants.Warehouse.GetAllWarehouses,
    );
  }

  getWarehouseById(id: string): Observable<Warehouse> {
    return this.apiService.send<Warehouse>(
      'GET',
      `${environment.UrlConstants.Warehouse.GetWarehouseById}/${id}`,
    );
  }

  getWarehousesByDistributor(distributorId: string): Observable<Warehouse[]> {
    return this.apiService.send<Warehouse[]>(
      'GET',
      `${environment.UrlConstants.Warehouse.GetWarehouseByDistributor}/${distributorId}`,
    );
  }

  updateWarehouse(id: string, dto: WarehouseUpdateDto): Observable<Warehouse> {
    return this.apiService.send<Warehouse>(
      'PUT',
      `${environment.UrlConstants.Warehouse.UpdateWarehouse}/${id}`,
      dto,
    );
  }

  deleteWarehouse(id: string): Observable<any> {
    return this.apiService.send<any>(
      'DELETE',
      `${environment.UrlConstants.Warehouse.DeleteWarehouse}/${id}`,
    );
  }
}
