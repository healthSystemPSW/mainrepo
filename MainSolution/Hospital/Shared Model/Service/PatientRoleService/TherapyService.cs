﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repository.PeriodPersistance;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class TherapyService
    {
        public PeriodService PeriodFunctions { get; set; }

        public TherapyService(IPeriodRepository periodRepository, string patientUsername)
        {
            PeriodFunctions = new PeriodService(periodRepository, patientUsername);
        }

        public List<Therapy> GetPatientTherapies(string username)
        {
            return PeriodFunctions.GetAllPeriods().Where(period => period.PatientUsername.Equals(username) && period.Prescription != null).SelectMany(period => period.Prescription.TherapyList).ToList();
        }

        public List<DateTime> GenerateDates(Therapy therapy)
        {
            List<DateTime> notifications = new List<DateTime>();
            DateTime dateIterator = therapy.StartHours;
            while (dateIterator.Date < therapy.EndDate.Date)
            {
                for (int i = 0; i < therapy.TimesPerDay; ++i)
                    notifications.Add(dateIterator.AddHours(i * 24 / therapy.TimesPerDay));

                dateIterator = dateIterator.AddDays(therapy.PauseInDays + 1);
            }
            return notifications;
        }
    }
}
