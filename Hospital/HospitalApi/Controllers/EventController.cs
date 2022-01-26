﻿using System;
using Hospital.EventStoring.Model;
using Hospital.EventStoring.Repository;
using Hospital.EventStoring.Service.Interfaces;
using Hospital.SharedModel.Repository.Base;
using HospitalApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EventController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IEventStoringService _eventStoringService;

        public EventController(IUnitOfWork uow,IEventStoringService eventStoringService)
        {
            this.uow = uow;
            this._eventStoringService = eventStoringService;
        }

        [HttpGet]
        public IActionResult GetAllEvents()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult AddEvent(EventDTO newEvent)
        {
            var incomingEvent = new StoredEvent()
            {
                Step = newEvent.Step,
                Time = DateTime.Now,
                Username = newEvent.Username
            };

            uow.GetRepository<IStoredEventWriteRepository>().Add(incomingEvent);

            return Ok();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult GetStatisticsPerPartOfDay()
        {
            var result = _eventStoringService.GetStatisticsPerPartOfDay();
            return Ok(result);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult GetStatisticsPerDayOfWeek()
        {
            var result = _eventStoringService.GetStatisticsPerDayOfWeek();
            return Ok(result);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult GetStatisticsPerMonths()
        {
            var result = _eventStoringService.GetStatisticsPerMonths();
            return Ok(result);
        }
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult GetNumberOfStepsSuccessScheduling()
        {
            var result = _eventStoringService.GetNumberOfSteps();
            return Ok(result);
        }
    }
}
