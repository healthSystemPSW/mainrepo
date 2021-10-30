﻿using System;
using Model;

namespace ZdravoHospital.GUI.PatientUI.DTOs
{
    public class PeriodDTO 
    { 
        public string DoctorName { get; set; }
        public string DoctorSurname { get; set; }
        private DateTime dateTime;
        public DateTime Date
        {
            get => dateTime;
            set { dateTime = value; }
        }
        public int RoomNumber { get; set; }
        public PeriodType PeriodType { get; set; }

        public int PeriodId { get; set; }
        public PeriodDTO(string doctorName, string doctorSurname, DateTime date, int roomNumber, PeriodType periodType, int periodId)
        {
            DoctorName = doctorName;
            DoctorSurname = doctorSurname;
            Date = date;
            RoomNumber = roomNumber;
            PeriodType = periodType;
            PeriodId = periodId;
        }

        public PeriodDTO()
        {
            
        }
    }
}
