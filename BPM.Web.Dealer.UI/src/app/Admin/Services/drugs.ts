import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Drugs } from '../Models/drugs';
import { environment } from '../../../environments/environment';
import { ApiService } from '../../common/services/api.service';

@Injectable({
  providedIn: 'root'
})
export class DrugService {

  constructor(private apiService: ApiService) { }

  getAll(): Observable<Drugs[]> {
    return this.apiService.send('GET', environment.UrlConstants.Drug.GetAllDrugs);
  }

}