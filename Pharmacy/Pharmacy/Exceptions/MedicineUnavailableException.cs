﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Exceptions
{
    public class MedicineUnavailableException : Exception
    {
        public MedicineUnavailableException() : base("Medicine unavailable.")
        {

        }
    }
}
