import { IPatient } from './patient-interface';

export enum FeedbackStatus {
    Pending,
    Rejected,
    Approved
}
export interface IPatientFeedback {

    id: number;
    patientId: number;
    patient: IPatient;
    feedbackStatus: FeedbackStatus;
    createdDate: Date;
    text: String;
    isPublishable: Boolean;
    isAnonymous: Boolean;



}