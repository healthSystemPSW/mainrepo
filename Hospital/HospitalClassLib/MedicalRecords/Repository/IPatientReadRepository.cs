﻿using System.Collections.Generic;
using Hospital.MedicalRecords.Model;
using Hospital.Schedule.Model;
using Hospital.SharedModel.Repository.Base;

namespace Hospital.MedicalRecords.Repository
{
    public interface IPatientReadRepository : IReadBaseRepository<int, Patient>
    {
        public Patient GetPatient(string username);
        public Patient GetByUsername(string username);
        public Prescription GetPrescriptionForScheduledEvent(int scheduledEventId, string loggedPatientUsername);
    }
}
