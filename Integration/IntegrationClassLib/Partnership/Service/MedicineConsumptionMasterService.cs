﻿using Integration.Partnership.Model;
using Integration.Partnership.Repository;
using Integration.Shared.Model;
using Integration.Shared.Repository.Base;
using System;
using System.Collections.Generic;

namespace Integration.Partnership.Service
{
    public class MedicineConsumptionMasterService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicineConsumptionMasterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public MedicineConsumptionReport CreateConsumptionReportInTimeRange(TimeRange timeRange)
        {
            MedicineConsumptionCalculationMicroService medicineConsumptionCalculationMicroService = new MedicineConsumptionCalculationMicroService();
            var repo = _unitOfWork.GetRepository<IReceiptReadRepository>();
            IEnumerable<Receipt> receiptsInTimeRange = repo.GetReceiptLogsInTimeRange(timeRange);
            IEnumerable<MedicineConsumption> medicineConsumptions =
                medicineConsumptionCalculationMicroService.CalculateMedicineConsumptions(receiptsInTimeRange);
            MedicineConsumptionReport retVal = new MedicineConsumptionReport
            {
                startDate = timeRange.startDate,
                endDate = timeRange.endDate,
                createdDate = DateTime.Now,
                MedicineConsumptions = medicineConsumptions
            };
            return retVal;
        }
    }
}
