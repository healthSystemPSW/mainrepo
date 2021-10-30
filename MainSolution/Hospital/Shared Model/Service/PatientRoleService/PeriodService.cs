﻿using Model;
using Repository.PeriodPersistance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class PeriodService
    {
        #region Properties

        public IPeriodRepository PeriodRepository { get; set; }


        public string PatientUsername { get; set; }
        public string ErrorMessage { get; private set; }
        #endregion

        public PeriodService(string patientUsername)
        {
            PeriodRepository = new PeriodRepository();
            PatientUsername = patientUsername;
        }

        #region Methods

        public List<Period> GetAllPeriods()
        {
            return PeriodRepository.GetValues();
        }
        public void RemovePeriodById(int id)
        {
            PeriodRepository.DeleteById(id);
        }

        public Period GetPeriod(int id)
        {
            return PeriodRepository.GetById(id);
        }

        public  void UpdatePeriod(Period period)
        {
            PeriodRepository periodRepository = new PeriodRepository();
            periodRepository.Update(period);
        }

        public  void SerializeNewPeriod(Period period)
        {
            PeriodRepository periodRepository = new PeriodRepository();
            periodRepository.Create(period);
        }
       
        public  bool IsPeriodWithinGivenMinutes(DateTime dateTime, int minutes)
        {
            bool itIs = false || dateTime >= DateTime.Now && dateTime <= DateTime.Now.AddMinutes(minutes);

            return itIs;
        }

        public int GeneratePeriodId()
        {
            if (PeriodRepository.GetValues().Count == 0)
                return 0;

            return PeriodRepository.GetValues().Last().PeriodId + 1;//vrati vrednost za jedan vecu od poslednjeg id-a iz liste
        }

        public bool CheckPeriodAvailability(Period checkedPeriod)
        {
            if (!IsDoctorInShift(checkedPeriod)) { ErrorMessage = "Doctor is not working in that shift!"; return false; }
            List<Period> periods = PeriodRepository.GetValues();
            return periods.All(period => IsPeriodAvailable(period, checkedPeriod));
        }

        private bool IsPeriodAvailable(Period period, Period checkedPeriod)
        {
            bool available = true;
            if (period.StartTime.Date != checkedPeriod.StartTime.Date) return available;
            if (period.PatientUsername.Equals(checkedPeriod.PatientUsername) && !IsPatientAvailable(period, checkedPeriod)) //proveri da li pacijent tad ima zakazano
                available = false;
            else if (period.DoctorUsername.Equals(checkedPeriod.DoctorUsername) && !IsDoctorAvailable(period, checkedPeriod))//proveri da li doktor tad ima zakazano
                available = false;
            return available;
        }

        private bool IsDoctorAvailable(Period period, Period checkedPeriod)
        {
            if (!DoPeriodsOverlap(period, checkedPeriod)) return true;
            ErrorMessage = "Doctor has an existing appointment at selected time!";
            return false;

        }

        private bool IsDoctorInShift(Period checkedPeriod)
        {
            if (checkedPeriod.DoctorUsername == null) return true;//dodato zbog suggestion-a
            DoctorService doctorFunctions = new DoctorService();
            return doctorFunctions.IsTimeInDoctorsShift(checkedPeriod.StartTime, checkedPeriod.DoctorUsername);
        }

        private bool IsPatientAvailable(Period period, Period checkedPeriod)
        {
            if (!DoPeriodsOverlap(period, checkedPeriod)) return true;
            ErrorMessage = "Patient has an existing appointment at selected time!";
            return false;

        }

        public bool DoPeriodsOverlap(Period period, Period checkedPeriod)
        {
            if (period.PeriodId.Equals(checkedPeriod.PeriodId))//u slucaju kad se edituje period
                return false;

            DateTime endingPeriodTime = period.StartTime.AddMinutes(period.Duration);
            DateTime endingCheckedPeriodTime = checkedPeriod.StartTime.AddMinutes(checkedPeriod.Duration);

            return (checkedPeriod.StartTime >= period.StartTime && checkedPeriod.StartTime < endingPeriodTime) || (endingCheckedPeriodTime > period.StartTime && endingCheckedPeriodTime <= endingPeriodTime);
        }

        #endregion

    }
}
