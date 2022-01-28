﻿using Integration.Pharmacies.Model;
using Integration.Pharmacies.Service;
using Integration.Shared.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Renci.SshNet;
using RestSharp;
using System.IO;
using System.Linq;
using System.Net;
using IntegrationAPI.Adapters.PDF;
using IntegrationAPI.Adapters.PDF.Implementation;
using IntegrationAPI.Controllers.Base;
using IntegrationAPI.DTO.MedicineConsumption;
using IntegrationAPI.DTO.Shared;
using IntegrationAPI.HttpRequestSenders;
using IntegrationApi.Messages;

namespace IntegrationAPI.Controllers.Medicine
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : BaseIntegrationController
    {
        private readonly PharmacyMasterService _pharmacyMasterService;
        public ReportController(IUnitOfWork unitOfWork, IHttpRequestSender httpRequestSender) : base(unitOfWork, httpRequestSender)
        {
            _pharmacyMasterService = new PharmacyMasterService(unitOfWork);
        }

        [HttpPost]
        public MedicineConsumptionReportDTO CreateConsumptionReport(TimePeriodDTO timeRange)
        {
            string targetUrl = _hospitalBaseUrl + "/api/MedicationExpenditureReport/GetMedicationExpenditureReport";
            var response = _httpRequestSender.Post(targetUrl, timeRange);

            if (response.StatusCode != HttpStatusCode.OK) return null;
            var report = JsonConvert.DeserializeObject<MedicineConsumptionReportDTO>(response.Content);
            report.MedicationExpenditureDTO = report.MedicationExpenditureDTO.OrderByDescending(medicine => medicine.Amount).ToList();
            return report;
        }
        [HttpPost]
        [Produces("application/json")]
        public IActionResult SendConsumptionReport(MedicineConsumptionReportDTO report)
        {
            report.MedicationExpenditureDTO = report.MedicationExpenditureDTO.OrderByDescending(medicine => medicine.Amount).ToList();
            if (report.MedicationExpenditureDTO.Count < 1) return BadRequest(ReportMessages.Empty);
            string fileName;
            try
            {
                fileName = SaveMedicineReportToSftpServer(report, _sftpCredentials);
            }
            catch
            {
                return Problem(ReportMessages.SftpError);
            }
            SendToPharmacies(report, fileName);
            return Ok(ReportMessages.ReportSent);
        }

        private void SendToPharmacies(MedicineConsumptionReportDTO report, string fileName)
        {
            var pharmacies = _pharmacyMasterService.GetPharmacies();
            foreach (Pharmacy pharmacy in pharmacies)
            {
                RestClient client = new RestClient();
                string targetUrl = pharmacy.BaseUrl + "/api/Report/ReceiveMedicineConsumptionReport";
                RestRequest request = new RestRequest(targetUrl);
                request.AddJsonBody(new ReportDTO
                {
                    ApiKey = pharmacy.ApiKey,
                    FileName = fileName,
                    Host = _sftpCredentials.Host
                });
                client.PostAsync<IActionResult>(request);
            }
        }
        private string SaveMedicineReportToSftpServer(MedicineConsumptionReportDTO report, SftpCredentialsDTO credentials)
        {
            IPDFAdapter adapter = new DynamicPDFAdapter();
            var medicineConsumptionReportToPdfDto = CreateMedicineConsumptionReportToPdfDto(report);
            string fileName = adapter.MakeMedicineConsumptionReportPdf(medicineConsumptionReportToPdfDto);
            string dest = "MedicineReports" + Path.DirectorySeparatorChar + fileName;
            SaveToSftp(dest, credentials);
            return fileName;
        }

        private MedicineConsumptionReportToPdfDTO CreateMedicineConsumptionReportToPdfDto(MedicineConsumptionReportDTO report)
        {
            MedicineConsumptionReportToPdfDTO medicineConsumptionReportToPdfDto = new MedicineConsumptionReportToPdfDTO
            {
                StartDate = report.startDate,
                EndDate = report.endDate,
                CreatedDate = report.createdDate,
                MedicineConsumptions = report.MedicationExpenditureDTO,
                HospitalName = _hospitalInfo.Name
            };
            return medicineConsumptionReportToPdfDto;
        }

        private void SaveToSftp(string path, SftpCredentialsDTO credentials)
        {
            SftpClient sftpClient = new SftpClient(new PasswordConnectionInfo(credentials.Host, credentials.Username, credentials.Password));
            sftpClient.Connect();
            Stream fileStream = System.IO.File.OpenRead(path);
            string filePath = Path.GetFileName(path);
            sftpClient.UploadFile(fileStream, filePath);
            sftpClient.Disconnect();
            fileStream.Close();
        }

    }
}