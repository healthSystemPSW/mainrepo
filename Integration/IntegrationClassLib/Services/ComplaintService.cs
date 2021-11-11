﻿using Integration;
using Integration.Model;
using Integration.Repositories;
using Integration.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integration.Services
{
    public class ComplaintService
    {
        private readonly IUnitOfWork unitOfWork;
        public ComplaintService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Complaint> GetComplaints()
        {
            var complaintsRepo = unitOfWork.GetRepository<IComplaintReadRepository>();
            IEnumerable<Complaint> complaints = complaintsRepo.GetAll().Include(x => x.ComplaintResponse).Include(x => x.Manager).Include(x => x.Pharmacy);
            return complaints;
        }
        public Complaint GetComplaintById(int id)
        {
            IEnumerable<Complaint> complaints = GetComplaints();
            Complaint complaint = complaints.FirstOrDefault(complaint => complaint.Id == id);
            return complaint;
        }
        public void SaveComplaint(Complaint complaint)
        {
            var complaintRepo = unitOfWork.GetRepository<IComplaintWriteRepository>();
            complaintRepo.Add(complaint);
        }
        public void DeleteComplaint(Complaint complaint)
        {
            var complaintRepo = unitOfWork.GetRepository<IComplaintWriteRepository>();
            complaintRepo.Delete(complaint);
        }
    }
}
