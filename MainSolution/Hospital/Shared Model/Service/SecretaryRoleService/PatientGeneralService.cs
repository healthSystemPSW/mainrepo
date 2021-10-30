﻿using Model;
using Repository.CredentialsPersistance;
using Repository.PatientPersistance;
using System.Collections.Generic;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class PatientGeneralService
    {
        private IPatientRepository _patientRepository;
        private ICredentialsRepository _credentialsRepository;
        public PatientGeneralService(IPatientRepository patientRepository, ICredentialsRepository credentialsRepository)
        {
            _patientRepository = patientRepository;
            _credentialsRepository = credentialsRepository;
        }
        public List<Patient> GetAll()
        {
            return _patientRepository.GetValues();
        }
        public void ProcessPatientDeletion(Patient SelectedPatient)
        {
            if (SelectedPatient == null)
            {
                
            }
            else
            {
                if (SelectedPatient.IsGuest)
                {
                    _patientRepository.DeleteById("guest_" + SelectedPatient.HealthCardNumber);
                }
                else
                {
                    _patientRepository.DeleteById(SelectedPatient.Username);
                    _credentialsRepository.DeleteById(SelectedPatient.Username);
                }
            }
        }
        public void ProcessPatientUnblock(Patient patientToUnblock)
        {
            patientToUnblock.RecentActions = 0;
            _patientRepository.Update(patientToUnblock);
        }
        public bool IsPatientRegistered(string username)
        {
            List<Credentials> accounts = _credentialsRepository.GetValues();
            foreach (var account in accounts)
            {
                if (account.Username.Equals(username) && account.Role == Model.RoleType.PATIENT)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
