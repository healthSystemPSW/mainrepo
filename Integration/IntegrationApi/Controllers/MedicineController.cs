﻿using Integration.MasterServices;
using Integration.Model;
using Integration.Repositories.Base;
using IntegrationAPI.Adapters;
using IntegrationAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private PharmacyMasterService _pharmacyMasterService;

        public MedicineController(IUnitOfWork unitOfWork)
        {
            _pharmacyMasterService = new PharmacyMasterService(unitOfWork);
        }

        [HttpPost]
        public IActionResult RequestMedicineInformation(CreateMedicineRequestForPharmacyDTO createMedicineRequestDTO)
        {
            if (createMedicineRequestDTO.Quantity <= 0)
            {
                return BadRequest("Invalid quantity.");
            }
            Pharmacy pharmacy = _pharmacyMasterService.GetPharmacyById(createMedicineRequestDTO.PharmacyId);
            if (pharmacy==null)
            {
                return BadRequest("Pharmacy id doesn't exist.");
            }
            MedicineInformationRequestForPharmacyDTO medicineRequestDTO = MedicineInventoryAdapter.CreateMedicineRequestToMedicineInformationRequest(createMedicineRequestDTO, pharmacy);
            IRestResponse response = SendMedicineRequestToPharmacy(medicineRequestDTO, pharmacy);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return BadRequest("Pharmacy failed to receive request! Try again");
            }
            MedicineInformationResponseFromPharmacyDTO responseDTO = JsonConvert.DeserializeObject <MedicineInformationResponseFromPharmacyDTO>(response.Content);
            return Ok(responseDTO);
        }

        private IRestResponse SendMedicineRequestToPharmacy(MedicineInformationRequestForPharmacyDTO medicineRequestDTO, Pharmacy pharmacy)
        {
            RestClient client = new RestClient();
            string targetUrl = pharmacy.BaseUrl + "/api/HospitalCommunication/AcceptHospitalRegistration";
            RestRequest request = new RestRequest(targetUrl);
            request.AddJsonBody(medicineRequestDTO);
            return client.Post(request);
        }

        [HttpPost]
        public IActionResult UrgentProcurementOfMedicine(CreateMedicineRequestForPharmacyDTO createMedicineRequestDTO)
        {
            if (createMedicineRequestDTO.Quantity <= 0)
            {
                return BadRequest("Invalid quantity.");
            }
            Pharmacy pharmacy = _pharmacyMasterService.GetPharmacyById(createMedicineRequestDTO.PharmacyId);
            if (pharmacy == null)
            {
                return BadRequest("Pharmacy id doesn't exist.");
            }
            EmergencyProcurementRequestForPharmacyDTO medicineRequestDTO = MedicineInventoryAdapter.CreateMedicineRequestToEmergencyProcurementRequest(createMedicineRequestDTO, pharmacy);
            IRestResponse response = SendUrgentProcurementRequestToPharmacy(medicineRequestDTO, pharmacy);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return BadRequest("Pharmacy failed to receive request! Try again");
            }
            EmergencyProcurementResponseFromPharmacyDTO responseDTO = JsonConvert.DeserializeObject<EmergencyProcurementResponseFromPharmacyDTO>(response.Content);
            if(responseDTO.answer == true)
            {

                return Ok(responseDTO);
            }
            else
            {
                return Ok(responseDTO);
            }
        }

        private IRestResponse SendUrgentProcurementRequestToPharmacy(EmergencyProcurementRequestForPharmacyDTO medicineRequestDTO, Pharmacy pharmacy)
        {
            RestClient client = new RestClient();
            string targetUrl = pharmacy.BaseUrl + "/api/HospitalCommunication/AcceptHospitalRegistration";
            RestRequest request = new RestRequest(targetUrl);
            request.AddJsonBody(medicineRequestDTO);
            return client.Post(request);
        }
    }
}
