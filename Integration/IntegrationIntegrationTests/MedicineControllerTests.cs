﻿using Castle.Core.Internal;
using Integration.Model;
using IntegrationAPI.DTO;
<<<<<<< HEAD
using IntegrationIntegrationTests.Base;
=======
using IntegrationClassLibTests.Base;
>>>>>>> feature/integration-sftp-medicine-specification
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationClassLibTests
{
    public class MedicineControllerTests : BaseTest
    {
        public MedicineControllerTests(BaseFixture fixture) : base(fixture)
        {
<<<<<<< HEAD
            Context.Pharmacies.RemoveRange(Context.Pharmacies);
            Context.Countries.RemoveRange(Context.Countries);
            Context.Cities.RemoveRange(Context.Cities);
            Context.SaveChanges();
            MakePharmacies();
=======
            if (Context.Pharmacies.IsNullOrEmpty())
                MakePharmacies();
>>>>>>> feature/integration-sftp-medicine-specification
        }

        [Fact]
        public async Task Check_medicine_availability_incorrect_pharmacyid_should_return_bad_request()
        {
            CreateMedicineRequestForPharmacyDTO newRequest = GetRequestWithIncorrectPharmacyId();

            var content = GetContent(newRequest);

            var response = await Client.PostAsync(BaseUrl + "api/Medicine/RequestMedicineInformation", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Check_medicine_availability_incorrect_quantity_should_return_bad_request()
        {
            CreateMedicineRequestForPharmacyDTO newRequest = GetRequestWithIncorrectQuantity();

            var content = GetContent(newRequest);

            var response = await Client.PostAsync(BaseUrl + "api/Medicine/RequestMedicineInformation", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Check_medicine_availability_no_answer_should_return_bad_request()
        {
            CreateMedicineRequestForPharmacyDTO newRequest = GetRequestWithCorrectData();

            var content = GetContent(newRequest);

            var response = await Client.PostAsync(BaseUrl + "api/Medicine/RequestMedicineInformation", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Urgent_rocurement_of_medicine_incorrect_pharmacyid_should_return_bad_request()
        {
            var newRequest = GetRequestWithIncorrectPharmacyId();

            var content = GetContent(newRequest);

            var response = await Client.PostAsync(BaseUrl + "api/Medicine/UrgentProcurementOfMedicine", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Urgent_rocurement_of_medicine_incorrect_quantity_should_return_bad_request()
        {
            CreateMedicineRequestForPharmacyDTO newRequest = GetRequestWithIncorrectQuantity();

            var content = GetContent(newRequest);

            var response = await Client.PostAsync(BaseUrl + "api/Medicine/UrgentProcurementOfMedicine", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Urgent_rocurement_of_medicine_no_answer_should_return_bad_request()
        {
            CreateMedicineRequestForPharmacyDTO newRequest = GetRequestWithCorrectData();

            var content = GetContent(newRequest);

            var response = await Client.PostAsync(BaseUrl + "api/Medicine/UrgentProcurementOfMedicine", content);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
        private CreateMedicineRequestForPharmacyDTO GetRequestWithIncorrectPharmacyId()
        {
            return new CreateMedicineRequestForPharmacyDTO()
            {
                PharmacyId = -1,
                ManufacturerName = "Hemofarm",
                MedicineName = "Brufen",
                Quantity = 10
            };
        }
        private CreateMedicineRequestForPharmacyDTO GetRequestWithIncorrectQuantity()
        {
            return new CreateMedicineRequestForPharmacyDTO()
            {
                PharmacyId = 1,
                ManufacturerName = "Hemofarm",
                MedicineName = "Brufen",
                Quantity = -1
            };
        }
        private CreateMedicineRequestForPharmacyDTO GetRequestWithCorrectData()
        {
            return new CreateMedicineRequestForPharmacyDTO()
            {
                PharmacyId = 1,
                ManufacturerName = "Hemofarm",
                MedicineName = "Brufen",
                Quantity = 10
            };
        }

        private void MakePharmacies()
        {
            Context.Pharmacies.Add(new Pharmacy()
            {
                City = new City()
                {
                    Id = 1,
                    Country = new Country()
                    {
                        Name = "Test country",
                        Id = 1
                    },
                    Name = "Test city"
                },
                Id = 1,
                ApiKey = new Guid(),
                BaseUrl = "baseUrl",
                Name = "Test pharmacy",
                StreetName = "Test Street Name",
                StreetNumber = "1"
            });

            Context.SaveChanges();
        }
    }
}
