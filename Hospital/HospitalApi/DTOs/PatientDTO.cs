﻿using Hospital.SharedModel.Model.Enumerations;
using System;

namespace HospitalApi.DTOs
{
    public class PatientDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public CityDTO City { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public MedicalRecordDTO MedicalRecord { get; set; }
        public string PhotoEncoded { get; set; }
    }
}
