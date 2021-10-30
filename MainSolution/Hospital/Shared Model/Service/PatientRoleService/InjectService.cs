﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Model;
using ZdravoHospital.GUI.PatientUI.DTOs;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class InjectService
    {
        public void FillObservableDoctorDTOCollection(ObservableCollection<DoctorDTO> Doctors)
        {
            DoctorService doctorFunctions = new DoctorService();
            foreach (var doctor in doctorFunctions.GetGeneralDoctors())
                Doctors.Add(new DoctorDTO(doctor));
            
        }

        public void FillDoctorDTOCollection(List<DoctorDTO> Doctors)
        {
            DoctorService doctorFunctions = new DoctorService();
            Doctors.AddRange(doctorFunctions.GetGeneralDoctors().Select(doctor => new DoctorDTO(doctor)));
        }

        public  void GenerateTimeSpan(List<TimeSpan> timeList)
        {
            timeList.Add(new TimeSpan(8, 0, 0));
            timeList.Add(new TimeSpan(8, 30, 0));
            timeList.Add(new TimeSpan(9, 0, 0));
            timeList.Add(new TimeSpan(9, 30, 0));
            timeList.Add(new TimeSpan(10, 0, 0));
            timeList.Add(new TimeSpan(10, 30, 0));
            timeList.Add(new TimeSpan(11, 0, 0));
            timeList.Add(new TimeSpan(11, 30, 0));
            timeList.Add(new TimeSpan(12, 0, 0));
            timeList.Add(new TimeSpan(12, 30, 0));
            timeList.Add(new TimeSpan(13, 0, 0));
            timeList.Add(new TimeSpan(13, 30, 0));
            timeList.Add(new TimeSpan(14, 0, 0));
            timeList.Add(new TimeSpan(14, 30, 0));
            timeList.Add(new TimeSpan(15, 0, 0));
            timeList.Add(new TimeSpan(15, 30, 0));
            timeList.Add(new TimeSpan(16, 0, 0));
            timeList.Add(new TimeSpan(16, 30, 0));
            timeList.Add(new TimeSpan(17, 0, 0));
            timeList.Add(new TimeSpan(17, 30, 0));
            timeList.Add(new TimeSpan(18, 0, 0));
            timeList.Add(new TimeSpan(18, 30, 0));
            timeList.Add(new TimeSpan(19, 0, 0));
            timeList.Add(new TimeSpan(19, 30, 0));
        }

        public  void GenerateObesrvableTimes(ObservableCollection<TimeSpan> timeList)
        {
            timeList.Add(new TimeSpan(8, 0, 0));
            timeList.Add(new TimeSpan(8, 30, 0));
            timeList.Add(new TimeSpan(9, 0, 0));
            timeList.Add(new TimeSpan(9, 30, 0));
            timeList.Add(new TimeSpan(10, 0, 0));
            timeList.Add(new TimeSpan(10, 30, 0));
            timeList.Add(new TimeSpan(11, 0, 0));
            timeList.Add(new TimeSpan(11, 30, 0));
            timeList.Add(new TimeSpan(12, 0, 0));
            timeList.Add(new TimeSpan(12, 30, 0));
            timeList.Add(new TimeSpan(13, 0, 0));
            timeList.Add(new TimeSpan(13, 30, 0));
            timeList.Add(new TimeSpan(14, 0, 0));
            timeList.Add(new TimeSpan(14, 30, 0));
            timeList.Add(new TimeSpan(15, 0, 0));
            timeList.Add(new TimeSpan(15, 30, 0));
            timeList.Add(new TimeSpan(16, 0, 0));
            timeList.Add(new TimeSpan(16, 30, 0));
            timeList.Add(new TimeSpan(17, 0, 0));
            timeList.Add(new TimeSpan(17, 30, 0));
            timeList.Add(new TimeSpan(18, 0, 0));
            timeList.Add(new TimeSpan(18, 30, 0));
            timeList.Add(new TimeSpan(19, 0, 0));
            timeList.Add(new TimeSpan(19, 30, 0));
        }
    }
}
