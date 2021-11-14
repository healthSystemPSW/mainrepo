﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Model
{
    public class Complaint
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int HospitalComplaintId { get; set; }
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
