﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationAPI.DTO
{
    public class MedicineProcurementGrpcResponseDTO
    {
        public bool ConnectionSuccesfull { get; set; }

        public MedicineProcurementResponseDTO Response { get; set; }
    }
}
