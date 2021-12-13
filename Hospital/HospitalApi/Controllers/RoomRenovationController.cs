﻿using AutoMapper;
using Hospital.GraphicalEditor.Repository;
using Hospital.GraphicalEditor.Service;
using Hospital.RoomsAndEquipment.Model;
using Hospital.RoomsAndEquipment.Repository;
using Hospital.RoomsAndEquipment.Service;
using Hospital.Schedule.Service;
using Hospital.SharedModel.Model.Wrappers;
using Hospital.SharedModel.Repository.Base;
using HospitalApi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoomRenovationController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RoomRenovationController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<RoomRenovationEvent> GetRenovationsByRoom(int roomId)
        {
            var roomRenovationRepo = _uow.GetRepository<IRoomRenovationEventReadRepository>();
            return roomRenovationRepo.GetAll()
                .Where(renovation => renovation.IsCanceled == false &&
                                     (renovation.RoomId == roomId ||
                                     renovation.MergeRoomId == roomId));

        }
        
        [HttpGet]
        public IActionResult GetSurroundingRoomsForRoom([FromQuery(Name = "roomId")] int roomId)
        {
            var surroundingRoomsService = new FindingSurroundingRoomsService(_uow);
           
            if (roomId <= 0)
            {
                return BadRequest();
            }

            var room = _uow.GetRepository<IRoomReadRepository>().GetById(roomId);

            if (room == null) {
                return BadRequest();
            }

            var roomPosition = _uow.GetRepository<IRoomPositionReadRepository>().GetByRoom(roomId);

            return Ok(surroundingRoomsService.GetSurroundingRooms(roomPosition));
        }

        [HttpPost]
        public IEnumerable<TimePeriodDTO> GetAvailableTerms(AvailableTermDTO availableTermsDTO)
        {
            var availableTermsService = new AvailableTermsService(_uow);
            var timePeriod = new TimePeriod()
            {
                StartTime = availableTermsDTO.StartDate,
                EndTime = availableTermsDTO.EndDate
            };

            var terms = availableTermsService.GetAvailableTerms(timePeriod, availableTermsDTO.InitialRoomId, availableTermsDTO.DestinationRoomId, availableTermsDTO.Duration);
            var availableTerms = new List<TimePeriodDTO>();
            foreach (TimePeriod term in terms)
            {
                string start = term.StartTime.ToString("g");
                string end = term.EndTime.ToString("g");
                TimePeriodDTO dto = new TimePeriodDTO()
                {
                    StartDate = start,
                    EndDate = end
                };
                availableTerms.Add(dto);
            }

            return availableTerms;
        }

        [HttpPost]
        public IActionResult AddNewRoomRenovationEvent(RoomRenovationEvent roomRenovationEvent)
        {
            try
            {
                if (roomRenovationEvent == null)
                {
                    return BadRequest("Incorrect format sent! Please try again.");
                }


                var repo = _uow.GetRepository<IRoomRenovationEventWriteRepository>();
                RoomRenovationEvent addedEvent = repo.Add(roomRenovationEvent);

                if (addedEvent == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Could not insert event in the database.");
                }

                return Ok("Your room renovation request has been recorded.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error inserting event event in the database.");
            }
        }

        [HttpGet]
        public IEnumerable<RoomRenovationEvent> GetRenovationsByRoom(int roomId)
        {
            var roomRenovationRepo = _uow.GetRepository<IRoomRenovationEventReadRepository>();
            return roomRenovationRepo.GetAll()
                .Where(renovation => renovation.RoomId == roomId ||
                                     renovation.MergeRoomId == roomId);
        }

        [HttpPost]
        public IActionResult CancelRenovation(RoomRenovationEventDto roomRenovationDTO)
        {
            try
            {
                if (roomRenovationDTO == null)
                {
                    return BadRequest("Incorrect format sent! Please try again.");
                }

                var cancellingEventsService = new CancellingEventsService(_uow);
                cancellingEventsService.CancelRoomRenovationEvent(_mapper.Map<RoomRenovationEvent>(roomRenovationDTO));

                return Ok("Your renovation event has been canceled.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error cancelling renovation event.");
            }
        }

    }
}
