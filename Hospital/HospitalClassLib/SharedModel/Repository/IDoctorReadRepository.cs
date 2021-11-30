﻿using Hospital.SharedModel.Model;
using Hospital.SharedModel.Repository.Base;

namespace Hospital.SharedModel.Repository
{
    public interface IDoctorReadRepository : IReadBaseRepository<int, Doctor>
    {
    }
}
