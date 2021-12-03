﻿using System;
using System.Collections.Generic;
using System.IO;
using Integration.Partnership.Model;
using Integration.Partnership.Repository;
using Integration.Pharmacies.Model;
using Integration.Pharmacies.Repository;
using Integration.Shared.Repository.Base;
using IntegrationAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using ceTe.DynamicPDF.PageElements;
using IntegrationAPI.Controllers.Base;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;
using Path = System.IO.Path;

namespace IntegrationAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MedicineSpecificationController : BaseSftpController
    {
        public MedicineSpecificationController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        [HttpGet]
        public IEnumerable<MedicineSpecificationFrontDTO> GetAllMedicineSpecificationFiles()
        {
            var allSpecFiles = 
                _unitOfWork.GetRepository<IMedicineSpecificationFileReadRepository>()
                .GetAll()
                .Include(x => x.Pharmacy);
            List<MedicineSpecificationFrontDTO> retVal = new List<MedicineSpecificationFrontDTO>();
            foreach (MedicineSpecificationFile medicineSpecificationFile in allSpecFiles)
            {
                retVal.Add(new MedicineSpecificationFrontDTO
                {
                    MedicineName = medicineSpecificationFile.MedicineName,
                    PharmacyName = medicineSpecificationFile.Pharmacy.Name,
                    FileName = medicineSpecificationFile.FileName,
                    ReceivedDate = medicineSpecificationFile.ReceivedDate
                });
            }

            return retVal;
        }

        [HttpGet]
        public IActionResult GetSpecificationPdf([FromQuery(Name = "fileName")] string fileName)
        {
            try
            {
                var stream = new FileStream("MedicineSpecifications" + Path.DirectorySeparatorChar + fileName, FileMode.Open);
                return File(stream, "application/pdf", fileName);
            }
            catch
            {
                return NotFound("File not found");
            }
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult SendMedicineSpecificationRequest(MedicineSpecificationRequestDTO dto)
        {
            Pharmacy pharmacy = _unitOfWork.GetRepository<IPharmacyReadRepository>().GetById(dto.PharmacyId);
            if (pharmacy == null)
            {
                ModelState.AddModelError("Pharmacy Id", "Pharmacy with that id does not exist");
                return BadRequest(ModelState);
            }
            RestClient client = new RestSharp.RestClient();
            RestRequest request = new RestRequest(pharmacy.BaseUrl + "/api/MedicineSpecification/MedicineSpecificationRequest");
            request.AddJsonBody(new MedicineSpecificationToPharmacyDTO
                { ApiKey = pharmacy.ApiKey, MedicineName = dto.MedicineName });
            var response = client.Post(request);
            if (response.StatusCode != HttpStatusCode.OK) return NotFound("Failed to reach pharmacy or pharmacy does not have medicine with given name!");
            MedicineSpecificationFileDTO medicineSpecificationFile =
                JsonConvert.DeserializeObject<MedicineSpecificationFileDTO>(response.Content);
            try
            {
                SftpClient sftpClient = new SftpClient(new PasswordConnectionInfo(sftpCredentials.Host, sftpCredentials.Username, sftpCredentials.Password));
                sftpClient.Connect();
                Stream fileStream = System.IO.File.OpenWrite("MedicineSpecifications" + Path.DirectorySeparatorChar + medicineSpecificationFile.FileName);
                sftpClient.DownloadFile(medicineSpecificationFile.FileName, fileStream);
                sftpClient.Disconnect();
                fileStream.Close();
            }
            catch
            {
                return Problem("Failed to save file, error while trying to download from sftp");
            }
            _unitOfWork.GetRepository<IMedicineSpecificationFileWriteRepository>().Add(new MedicineSpecificationFile
            {
                FileName = medicineSpecificationFile.FileName,
                Host = medicineSpecificationFile.Host,
                PharmacyId = pharmacy.Id,
                MedicineName = medicineSpecificationFile.MedicineName,
                ReceivedDate = medicineSpecificationFile.Date
            });
            return Ok("Pharmacy has sent the specification file to sftp server");
        }

    }
}
