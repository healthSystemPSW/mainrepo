import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IPatientFeedback } from 'src/app/interfaces/patient-feedback-interface';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {


  constructor(private _http: HttpClient) { }

  getAll(): Observable<IPatientFeedback[]> {
    return this._http.get<IPatientFeedback[]>(`${environment.baseUrl}` + 'api/Feedback');
  }
}
