import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IMedicineResponse } from "../interfaces/medicineResponse";
import { environment } from "src/environments/environment";


@Injectable()
export class PharmacyService{

    private _APIUrl = `${environment.baseIntegrationUrl}`;  //uneti url za http get zahtev!
    
    constructor(private _httpClient: HttpClient) {}

    registerPharmacy(val:any){
        return this._httpClient.post(this._APIUrl +'api/PharmacyCommunication/RegisterPharmacy',val);
    }

    getPharmacies(): Observable<any[]>{
        return this._httpClient.get<any[]>(this._APIUrl +'api/Pharmacy/GetPharmacies');
    }

    checkMedicine(val: any): Observable<IMedicineResponse>{
        return this._httpClient.post<IMedicineResponse>(this._APIUrl + 'api/Medicine/RequestMedicineInformation', val);
    }

    orderMedicine(val: any): Observable<IMedicineResponse>{
        return this._httpClient.post<IMedicineResponse>(this._APIUrl + 'api/Medicine/UrgentProcurementOfMedicine', val);
    }
}