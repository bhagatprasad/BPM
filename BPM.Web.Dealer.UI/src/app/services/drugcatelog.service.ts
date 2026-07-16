import { Injectable } from "@angular/core";
import { ApiService } from "../common/services/api.service";
import { drugCatelog } from "../models/drug-catelog";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";



@Injectable({
    providedIn: 'root'
})
export class DrugCatalogService {
    constructor(private apiService: ApiService) { }

    getDrugsCatalogAsync(): Observable<drugCatelog[]> {
        return this.apiService.send<drugCatelog[]>('GET', environment.UrlConstants.Drug.GetAllDrugs);
    }
}