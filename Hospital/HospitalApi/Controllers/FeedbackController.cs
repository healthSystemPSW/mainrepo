using AutoMapper;
using Hospital.Schedule.Model;
using Hospital.Schedule.Repository;
using Hospital.SharedModel.Model.Enumerations;
using Hospital.SharedModel.Repository.Base;
using HospitalApi.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyCorsImplementationPolicy")]
    public class FeedbackController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public FeedbackController(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPublishableFeedbacks()
        {
            try
            {
                var feedbackReadRepo = _uow.GetRepository<IFeedbackReadRepository>();
                var feedbacks = feedbackReadRepo.GetAll().Include(x => x.Patient);
                return Ok(feedbacks);

            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error!Failed loading comments!");
            }
            
        }

        [HttpGet("approved")]
        public IEnumerable<Feedback> GetApprovedFeedbacks()
        {
            var feedbackReadRepo = _uow.GetRepository<IFeedbackReadRepository>();
            return feedbackReadRepo.GetAll().Include(x => x.Patient).Where(x => x.IsApproved());
        }

        [HttpPost]
        public IActionResult InsertFeedback(NewFeedbackDTO feedbackDTO)
        {
            try
            {
                if (feedbackDTO == null)
                    return BadRequest("Incorrect feedback format sent! Please try again.");

                var feedbackWriteRepo = _uow.GetRepository<IFeedbackWriteRepository>();
                var addedFeedback = feedbackWriteRepo.Add(_mapper.Map<Feedback>(feedbackDTO));

                return addedFeedback == null ? StatusCode(StatusCodes.Status500InternalServerError, "Could not insert feedback in the database.")
                    : Ok("Your feedback has been submitted.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error inserting feedback in the database.");
            }
        }

        [HttpPut("publish")]
        public IActionResult ApproveFeedback(Feedback feedback)
        {
            try
            {
                if(feedback == null)
                    return BadRequest("Feedback format is wrong!");
                
                var feedbackWriteRepo = _uow.GetRepository<IFeedbackWriteRepository>();
                feedback.Publish();
                var approvedFeedback = feedbackWriteRepo.Update(feedback);

                return approvedFeedback == null ? StatusCode(StatusCodes.Status500InternalServerError, "Couldn't update feedback!")
                    : Ok(approvedFeedback);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data in database!");
            }
        }
        [HttpGet("{Id}")]
        public Feedback GetFeedback(int Id )
        {
            var feedbackReadRepo = _uow.GetRepository<IFeedbackReadRepository>();
            return feedbackReadRepo.GetById(Id);

        }

        [HttpPut("unpublish")]
        public IActionResult UnapproveFeedback(Feedback feedback)
        {
            try
            {
                if (feedback == null)
                    return BadRequest("Feedback format is wrong!");
                

                var feedbackWriteRepo = _uow.GetRepository<IFeedbackWriteRepository>();
                feedback.Unpublish();
                var approvedFeedback = feedbackWriteRepo.Update(feedback);

                return approvedFeedback == null ? StatusCode(StatusCodes.Status500InternalServerError, "Couldn't update feedback!")
                    : Ok(approvedFeedback);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data in database!");
            }
        }

    }
}
