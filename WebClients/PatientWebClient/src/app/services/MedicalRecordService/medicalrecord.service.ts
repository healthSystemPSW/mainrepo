import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IAppointment } from 'src/app/interfaces/appointment';
import { IFinishedAppointment } from 'src/app/interfaces/finished-appoinment';
import { IPatient } from 'src/app/interfaces/patient-interface';

@Injectable({
  providedIn: 'root'
})
export class MedicalRecordService {
  patient!: IPatient;

  constructor(private _http: HttpClient) {}

  get(): any {
    return this._http.get<IPatient>('api/MedicalRecord/GetPatientWithRecord');
  }
  getFutureAppointments(): any {
    return this._http.get<IAppointment[]>('/api/ScheduledEvents/GetUpcomingUserEvents/'+1);
  }

  getfinishedAppointments(): any {
    return this._http.get<IFinishedAppointment[]>('/api/ScheduledEvents/GetEventsForSurvey/'+1);
  }

  getCanceledAppointments(): any {
    return this._http.get<IAppointment[]>('/api/ScheduledEvents/GetCanceledUserEvents/'+1);
  }
  
  cancelAppointments(eventId:any): any {
    return this._http.put<IAppointment[]>('/api/ScheduledEvents/CancelScheduledEvent/'+eventId, []);
  }
}
