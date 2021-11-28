﻿using Hospital.Schedule.Model;
using Hospital.Shared_model.Repository.Base;

namespace Hospital.Schedule.Repository
{
    public interface IAnsweredSurveyReadRepository : IReadBaseRepository<int, AnsweredSurvey>
    {
    }
}
