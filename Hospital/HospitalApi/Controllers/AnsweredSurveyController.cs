﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Hospital.Schedule.Service.ServiceInterface;
using Hospital.Schedule.Model;
using HospitalApi.DTOs;
using Microsoft.AspNetCore.Cors;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("MyCorsImplementationPolicy")]
    public class AnsweredSurveyController : ControllerBase
    {
        private readonly IPatientSurveyService surveyService;
        private readonly IMapper mapper;

        public AnsweredSurveyController(IPatientSurveyService surveyService, IMapper mapper)
        {
            this.surveyService = surveyService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<AnsweredSurvey> GetAllAnsweredSurvey()
        {
            return surveyService.getAllAnsweredSurvey();
        }

        [HttpPost]
        public IActionResult CreateAnsweredSurvey(AnsweredSurveyDTO answeredSurveyDTO)
        {
            var temp = mapper.Map<AnsweredSurvey>(answeredSurveyDTO);
            return Ok(surveyService.createAnsweredSurvey(temp));
        }
    }
}
