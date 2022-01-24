﻿using Hospital.SharedModel.Repository.Base;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using HospitalApi.DTOs;
using Xunit;
using Hospital.MedicalRecords.Repository;
using Hospital.SharedModel.Repository;
using System;
using Hospital.SharedModel.Model.Enumerations;
using System.Linq;
using Hospital.RoomsAndEquipment.Repository;
using Hospital.SharedModel.Model;
using Hospital.Schedule.Model;
using Hospital.RoomsAndEquipment.Model;
using Microsoft.EntityFrameworkCore;
using Hospital.Schedule.Repository;
using HospitalIntegrationTests.DTOs;

namespace HospitalIntegrationTests.Base
{
    [Collection("IntegrationTests")]
    public abstract class BaseTest : IClassFixture<BaseFixture>
    {
        private readonly BaseFixture _fixture;

        protected BaseTest(BaseFixture fixture)
        {
            _fixture = fixture;
        }

        public IUnitOfWork UoW => _fixture.UoW;
        public HttpClient ManagerClient => _fixture.ManagerClient;
        public HttpClient PatientClient => _fixture.PatientClient;
        public CookieContainer CookieContainer => _fixture.CookieContainer;
        public string BaseUrl => "https://localhost:44303/";
        public string IntegrationBaseUrl => "https://localhost:44302/";
        public string ManagerToken { get; set; }
        public string PatientToken { get; set; }

        public void AddCookie(string name, string value, string domain)
        {
            CookieContainer.Add(new Cookie(name, value) { Domain = domain });
        }

        public StringContent GetContent(object content)
        {
            return new StringContent(JsonConvert.SerializeObject(content, settings: new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }), Encoding.UTF8, "application/json");
        }
        public void RegisterAndLogin(string role)
        {
            object user = null;
            if (role == "Patient")
            {
                user = UoW.GetRepository<IPatientReadRepository>()
                     .GetAll().FirstOrDefault(x => x.UserName.Equals("testPatientUsername"));

                if (user == null)
                {
                    
                    var roomWriteRepo = UoW.GetRepository<IRoomWriteRepository>();
                    var doctorWriteRepo = UoW.GetRepository<IDoctorWriteRepository>();
                    var testRoom = UoW.GetRepository<IRoomReadRepository>().GetAll().FirstOrDefault(x => x.Name == "TestRoom");
                    var testDoctor = UoW.GetRepository<IDoctorReadRepository>().GetAll().FirstOrDefault(x => x.FirstName == "TestDoctor");

                    if (testRoom == null)
                    {
                        testRoom = new Room()
                        {
                            Name = "TestRoom",
                            RoomType = RoomType.AppointmentRoom,
                            FloorNumber = 1,
                            Description = "TestDescription"

                        };
                        roomWriteRepo.Add(testRoom);
                    }
                    if (testDoctor == null)
                    {
                        testDoctor = new Doctor()
                        {
                            FirstName = "TestDoctor",
                            LastName = "TestDoctorLastName",
                            MiddleName = "TestDoctorMiddleName",
                            DateOfBirth = DateTime.Now,
                            Gender = Gender.Female,
                            Street = "TestDoctorStreet",
                            City = new City("Novi Sad", 21000, new Country("Serbia")),
                            Room = testRoom,
                            Specialization = new Specialization("General Practice", ""),
                            UserName = "testDoctorUsername",
                            Email = "testDoctor@gmail.com",
                            EmailConfirmed = true,
                            PhoneNumber = "testDoctorPhoneNumber",
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false,
                            LockoutEnabled = false,
                            AccessFailedCount = 0,
                            Shift = new Shift()
                            {
                                Name = "testShift",
                                From = 8,
                                To = 16
                            }

                        };
                        doctorWriteRepo.Add(testDoctor);
                    }

                    var newPatient = new NewPatientDTO()
                    {
                        FirstName = "TestPatient",
                        MiddleName = "TestPatientMiddleName",
                        LastName = "TestPatientLastName",

                        DateOfBirth = DateTime.Now,
                        Gender = Gender.Female,
                        Street = "TesPatientStreet",
                        StreetNumber = "44",
                        City = new CityDTO(){ Name = "Novi Sad" },
                        Email = "testPatient@gmail.com",
                        UserName = "testPatientUsername",
                        Password = "TestProba123",

                        PhoneNumber = "testPatientPhoneNumber",
                        MedicalRecord = new NewMedicalRecordDTO()
                        {
                            DoctorId = testDoctor.Id,
                            Measurements = new MeasurementsDTO()
                            {
                                Height = 167,
                                Weight = 60
                            },
                            BloodType = BloodType.Undefined,
                            JobStatus = JobStatus.Undefined

                        }
                    };

                    PatientClient.PostAsync(BaseUrl + "api/Registration/Register", GetContent(newPatient))
                     .GetAwaiter()
                     .GetResult();

                    var createdPatient = UoW.GetRepository<IPatientReadRepository>().GetAll().FirstOrDefault(x => x.UserName.Equals("testPatientUsername"));
                    createdPatient.EmailConfirmed = true;
                    UoW.GetRepository<IPatientWriteRepository>().Update(createdPatient);

                }
                var loginDTO = new LoginDTO()
                {
                    Username = "testPatientUsername",
                    Password = "TestProba123",
                };

                var response = PatientClient.PostAsync(BaseUrl + "api/Login", GetContent(loginDTO))
                    .GetAwaiter()
                    .GetResult();

                var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var json = JsonConvert.DeserializeObject<TempUserDTO>(result);
                PatientToken = json.Token;

                PatientClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", PatientToken);
            }
            else if (role == "Manager")
            {
                
                user = UoW.GetRepository<IManagerReadRepository>().GetAll().FirstOrDefault(x => x.UserName.Equals("testLogInManager"));
                if (user == null)
                {
                    user = new NewManagerDTO()
                    {
                        FirstName = "TestManager",
                        LastName = "TestManagerLastName",
                        MiddleName = "TestManagerMiddleName",
                        DateOfBirth = new DateTime(),
                        Gender = Gender.Female,
                        Street = "TestManagerStreet",
                        UserName = "testLogInManager",
                        City = new CityDTO() { Name = "Novi Sad"},
                        Email = "testManager@gmail.com",
                        Password = "111111aA"

                    };
                    var r = ManagerClient.PostAsync(BaseUrl + "api/Registration/RegisterManager", GetContent(user))
                    .GetAwaiter()
                    .GetResult();
                }
                var loginDTO = new LoginDTO()
                {
                    Username = "testLogInManager",
                    Password = "111111aA"
                };

                var response = ManagerClient.PostAsync(BaseUrl + "api/Login", GetContent(loginDTO))
                    .GetAwaiter()
                    .GetResult();

                var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var json = JsonConvert.DeserializeObject<TempUserDTO>(result);
                ManagerToken = json.Token;

                ManagerClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", ManagerToken);
            }
        }
        
        public void DeleteDataFromDataBase()
        {
            var patient = UoW.GetRepository<IPatientReadRepository>().GetAll().FirstOrDefault(x => x.UserName.Equals("testPatientUsername"));
            UoW.GetRepository<IPatientWriteRepository>().Delete(patient);
            if (patient == null) return;
            {
                var medicalRecord = UoW.GetRepository<IMedicalRecordReadRepository>()
                    .GetAll().Include(mr => mr.Doctor)
                    .FirstOrDefault(x => x.Id == patient.MedicalRecordId);

                UoW.GetRepository<IMedicalRecordWriteRepository>().Delete(medicalRecord);
            }

            var survey = UoW.GetRepository<ISurveyReadRepository>().GetAll()
                .FirstOrDefault(x => x.CreatedDate == new DateTime());
            if (survey != null)
            {
                UoW.GetRepository<ISurveyWriteRepository>().Delete(survey);
            }

            var doctor = UoW.GetRepository<IDoctorReadRepository>().GetAll()
                .FirstOrDefault(x => x.UserName == "testDoctorUsername");
            if (doctor != null)
            {
                UoW.GetRepository<IDoctorWriteRepository>().Delete(doctor);
            }

            var room = UoW.GetRepository<IRoomReadRepository>()
                    .GetAll().ToList()
                    .FirstOrDefault(x => x.Name == "TestRoom");

            if (room != null)
            {
                UoW.GetRepository<IRoomWriteRepository>().Delete(room);
            }
        }
    }
}
