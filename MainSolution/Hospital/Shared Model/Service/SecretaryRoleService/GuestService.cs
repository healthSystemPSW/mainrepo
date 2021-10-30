﻿using Model;
using Repository.PatientPersistance;
using System.Collections.Generic;
using ZdravoHospital.GUI.Secretary.DTOs;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class GuestService
    {
        private IPatientRepository _patientRepository;
        public GuestService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public bool isHealthCardUnique(string healthCardNum)
        {
            List<Patient> patients = _patientRepository.GetValues();
            foreach (var patient in patients)
            {
                if (patient.HealthCardNumber.Equals(healthCardNum))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ProcessGuestCreation(GuestDTO guestDTO)
        {
            if (isHealthCardUnique(guestDTO.HealthCardNumber))
            {
                Patient patient = ConvertDtoToPatient(guestDTO);
                _patientRepository.Create(patient);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ProcessGuestUpdate(GuestDTO guestDTO)
        {
            Patient patient = ConvertDtoToPatient(guestDTO);
            _patientRepository.Update(patient);
        }
        
        public Patient ConvertDtoToPatient(GuestDTO guestDTO)
        {
            Patient patient = new Patient(guestDTO.Name, guestDTO.Surname, guestDTO.CitizenId, guestDTO.HealthCardNumber);
            patient.Username = "guest_"+ guestDTO.HealthCardNumber;

            return patient;
        }
    }
}
