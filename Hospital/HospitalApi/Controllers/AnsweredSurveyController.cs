﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Hospital.Schedule.Service.ServiceInterface;
using Hospital.Schedule.Model;
using HospitalApi.DTOs;
using Microsoft.AspNetCore.Cors;
using Hospital.SharedModel.Repository.Base;
using Hospital.MedicalRecords.Repository;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("MyCorsImplementationPolicy")]
    public class AnsweredSurveyController : ControllerBase
    {
        private readonly IPatientSurveyService surveyService;
        private readonly ISurveyService _surveyService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper mapper;

        public AnsweredSurveyController(ISurveyService _surveyService,IPatientSurveyService surveyService, IUnitOfWork uow, IMapper mapper)
        {
            this.surveyService = surveyService;
            this._surveyService = _surveyService;
            this.mapper = mapper;
            this._uow = uow;
        }

        //[Authorize(Roles = "Patient")]
        [HttpGet]
        public IEnumerable<AnsweredSurvey> GetAllAnsweredSurvey()
        {
            return surveyService.getAllAnsweredSurvey();
        }

        //[Authorize(Roles = "Patient")]
        [HttpPost]
        public IActionResult CreateAnsweredSurvey(AnsweredSurveyDTO answeredSurveyDTO)
        {
            Survey activeSurvey = _surveyService.GetActiveSurvey();
            var patient = _uow.GetRepository<IPatientReadRepository>().GetPatient(answeredSurveyDTO.UserName);
            answeredSurveyDTO.PatientId = patient.Id;
            activeSurvey.CreateAnsweredSurvey(mapper.Map<AnsweredSurvey>(answeredSurveyDTO));
            _surveyService.Save(activeSurvey);

            //var temp = mapper.Map<AnsweredSurvey>(answeredSurveyDTO);
            //return Ok(surveyService.createAnsweredSurvey(temp));
            return Ok();
        }
    }
}
