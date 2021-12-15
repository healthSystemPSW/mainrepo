﻿using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hospital.MedicalRecords.Model;
using Hospital.MedicalRecords.Repository;
using Hospital.RoomsAndEquipment.Model;
using Hospital.RoomsAndEquipment.Repository;
using Hospital.SharedModel.Model;
using Hospital.SharedModel.Model.Enumerations;
using Hospital.SharedModel.Repository;
using HospitalApi.DTOs;
using HospitalIntegrationTests.Base;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace HospitalIntegrationTests
{
    public class RegistrationTests : BaseTest
    {
        private ITestOutputHelper _itoh;

        public RegistrationTests(BaseFixture fixture, ITestOutputHelper itoh) : base(fixture)
        {
            _itoh = itoh;
        }


        [Fact]
        public async Task Register_should_return_200()
        {
            ClearUser();

            var newPatient = InsertPatient();

            var content = GetContent(newPatient);

            var response = await Client.PostAsync(BaseUrl + "api/Registration/Register", content);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var foundRegisteredUser = UoW.GetRepository<IPatientReadRepository>()
                .GetAll().FirstOrDefault(x => x.UserName.ToUpper().Equals(newPatient.UserName.ToUpper()));

            ClearUser();

            foundRegisteredUser.ShouldNotBeNull();
            foundRegisteredUser.UserName.ShouldBe("testUserName");
        }

        private void ClearUser()
        {
            var user = UoW.GetRepository<IPatientReadRepository>()
                .GetAll().ToList()
                .FirstOrDefault(x => x.UserName == "testUserName");


            if (user != null)
            {
                var medicalRecord = UoW.GetRepository<IMedicalRecordReadRepository>()
                    .GetAll()
                    .FirstOrDefault(x => x.Patient.UserName == user.UserName);

                UoW.GetRepository<IPatientWriteRepository>().Delete(user);
            }
        }

        private NewPatientDTO InsertPatient()
        {
            //var user = UoW.GetRepository<IPatientReadRepository>()
            //    .GetAll().Include(p => p.MedicalRecord).ThenInclude(mr => mr.Doctor)
            //    .FirstOrDefault(x => x.UserName == "testUserName");

            {
                var city = UoW.GetRepository<ICityReadRepository>()
                    .GetAll()
                    .FirstOrDefault();

                if (city == null)
                {
                    var country = UoW.GetRepository<ICountryReadRepository>()
                        .GetAll()
                        .FirstOrDefault();

                    if (country == null)
                    {
                        country = new Country()
                        {
                            Name = "Test country"
                        };
                        UoW.GetRepository<ICountryWriteRepository>().Add(country);
                    }

                    city = new City()
                    {
                        CountryId = country.Id,
                        Name = "Test city",
                        PostalCode = 10
                    };
                    UoW.GetRepository<ICityWriteRepository>().Add(city);
                }

                var doctor = UoW
                    .GetRepository<IDoctorReadRepository>().GetAll().Include(d => d.Specialization).Include(d => d.Room)
                    .FirstOrDefault(d => d.Specialization.Name.ToLower().Equals("general practice"));

                if (doctor == null)
                {
                    var specialization = UoW.GetRepository<ISpecializationReadRepository>()
                        .GetAll()
                        .FirstOrDefault(x => x.Name.ToLower().Equals("general practice"));
                    if (specialization == null)
                    {
                        specialization = new Specialization()
                        {
                            Name = "General Practice"
                        };
                        UoW.GetRepository<ISpecializationWriteRepository>().Add(specialization);
                    }

                    var room = UoW.GetRepository<IRoomReadRepository>()
                        .GetAll()
                        .FirstOrDefault(x => x.RoomType == RoomType.AppointmentRoom);
                    if (room == null)
                    {
                        room = new Room()
                        {
                            Name = "test room",
                            RoomType = RoomType.AppointmentRoom
                        };
                        UoW.GetRepository<IRoomWriteRepository>().Add(room);
                    }


                    doctor = new Doctor()
                    {
                        UserName = "testDoctor",
                        CityId = city.Id,
                        RoomId = room.Id,
                        SpecializationId = specialization.Id
                    };
                    UoW.GetRepository<IDoctorWriteRepository>().Add(doctor);
                }

                return new NewPatientDTO()
                {
                    UserName = "testUserName",
                    Email = "testttemail@gmail.com",
                    Password = "Test Passw0rd",
                    CityId = city.Id,
                    MedicalRecord = new NewMedicalRecordDTO()
                    {
                        DoctorId = doctor.Id
                    }
                };
            }
        }
    }
}