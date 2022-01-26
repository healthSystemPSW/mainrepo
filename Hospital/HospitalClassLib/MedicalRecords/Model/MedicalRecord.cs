﻿using System;
using Hospital.SharedModel.Model;
using Hospital.SharedModel.Model.Enumerations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Hospital.MedicalRecords.Model
{
    public class MedicalRecord
    {
        public int Id { get; private set; }
        public Measurements Measurements { get; private set; }
        public BloodType BloodType { get; private set; }
        public JobStatus JobStatus { get; private set; }
        [Required]
        public int DoctorId { get; private set; }
        public Doctor Doctor { get; private set; }
        public IEnumerable<Allergy> Allergies { get; private set; }
        public List<Prescription> Prescriptions { get; private set; } = new List<Prescription>();

        public MedicalRecord(int id, Measurements measurements, BloodType bloodType, JobStatus jobStatus, int doctorId, IEnumerable<Allergy> allergies)
        {
            Id = id;
            Measurements = measurements;
            BloodType = bloodType;
            JobStatus = jobStatus;
            DoctorId = doctorId;
            Allergies = allergies;
            Prescriptions = new List<Prescription>();
            Validate();

        }
        public MedicalRecord( Measurements measurements, BloodType bloodType, JobStatus jobStatus, int doctorId, IEnumerable<Allergy> allergies)
        {
            Measurements = measurements;
            BloodType = bloodType;
            JobStatus = jobStatus;
            DoctorId = doctorId;
            Allergies = allergies;
            Prescriptions = new List<Prescription>();
            Validate();
        }
        public MedicalRecord()
        {
        }

        public void AddPrescription(Prescription prescription)
        {
            this.Prescriptions.Add(prescription);
        }

        private void Validate()
        {
            if (DoctorId <= 0) throw new ArgumentException("Not Valid");
        }

        public IEnumerable<Allergy> GetAllergies()
        {
            return new List<Allergy>(Allergies);
        }

        public void AddNewAllergy(Allergy newAllergy)
        {
            Allergies.ToList().Add(newAllergy);
            Validate();
        }
    }
}
