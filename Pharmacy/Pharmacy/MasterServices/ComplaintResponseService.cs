﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Model;
using Pharmacy.Repositories;
using Pharmacy.Repositories.Base;

namespace Pharmacy.MasterServices
{
    public class ComplaintResponseService
    {
        private readonly IUnitOfWork unitOfWork;
        public ComplaintResponseService(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

        public void SaveComplaintResponse(ComplaintResponse complaintResponse)
        {
            var complaintResponseRepo = unitOfWork.GetRepository<IComplaintResponseWriteRepository>();
            complaintResponseRepo.Add(complaintResponse);
        }
    }
}
