using Hospital.MedicalRecords.Model;
using Hospital.MedicalRecords.Repository;
using Hospital.RoomsAndEquipment.Repository;
using Hospital.Schedule.Model;
using Hospital.Schedule.Repository;
using Hospital.SharedModel.Model.Enumerations;
using Hospital.SharedModel.Repository;
using HospitalApi.DTOs;
using HospitalIntegrationTests.Base;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace HospitalIntegrationTests
{
    public class PatientSurveyTests : BaseTest
    {
        public PatientSurveyTests(BaseFixture fixture) : base(fixture)
        {
        }
        [Fact]
        public async Task Create_answered_survey_should_return_200OK()
        {
            //Arrange
            RegisterAndLogin("Patient");
            var patient = UoW.GetRepository<IPatientReadRepository>()
               .GetAll()
               .Where(x => x.UserName == "testPatientUsername")
               .FirstOrDefault();
            AnsweredSurveyDTO answeredSurveyDTO = ArrangeDatabase(patient);
            var eventId = answeredSurveyDTO.ScheduledEventId;
            var content = GetContent(answeredSurveyDTO);

            //Act
            var response = await PatientClient.PostAsync(BaseUrl + "api/AnsweredSurvey/CreateAnsweredSurvey/" + patient.UserName, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var answeredSurveyResult = JsonConvert.DeserializeObject<AnsweredSurvey>(responseContent);

            //Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var answeredSurveyDB = UoW.GetRepository<IAnsweredSurveyReadRepository>().GetAll().FirstOrDefault(x => x.ScheduledEventId == eventId);
            answeredSurveyDB.ScheduledEvent.Id.ShouldBe(eventId);

            ClearDatabase(eventId);
           
        }

        private void ClearDatabase(int eventId)
        {
            var answeredSurvey = UoW.GetRepository<IAnsweredSurveyReadRepository>().GetAll().FirstOrDefault(x => x.ScheduledEventId == eventId);
            UoW.GetRepository<IAnsweredSurveyWriteRepository>().Delete(answeredSurvey, true);

            var events = UoW.GetRepository<IScheduledEventReadRepository>().GetAll().FirstOrDefault(x => x.Id == eventId);
            UoW.GetRepository<IScheduledEventWriteRepository>().Delete(events, true);
            DeleteDataFromDataBase();
        }

        private AnsweredSurveyDTO ArrangeDatabase(Patient testPatient)
        {
            var testDoctor = UoW.GetRepository<IDoctorReadRepository>().GetAll().FirstOrDefault(x => x.FirstName == "TestDoctor");
            var testRoom = UoW.GetRepository<IRoomReadRepository>().GetAll().FirstOrDefault(x => x.Name == "TestRoom");
            var survey = UoW.GetRepository<ISurveyReadRepository>().GetAll().Where(x => x.isActive ).FirstOrDefault();
            if (survey == null)
            {
                survey = new Survey(true);
               
                UoW.GetRepository<ISurveyWriteRepository>().Add(survey);
            }
            var question = UoW.GetRepository<IQuestionReadRepository>()
                .GetAll()
                .Where(x => x.SurveyId == survey.Id)
                .FirstOrDefault();
            if (question == null)
            {
                question = new Question()
                {
                    Text = "How did you like our services?",
                    Category = SurveyCategory.HospitalSurvey,
                    SurveyId = survey.Id
                };
                UoW.GetRepository<IQuestionWriteRepository>().Add(question);
            }

            var answeredQuestionHospital = new AnsweredQuestionDTO
            {
                QuestionId = question.Id,
                Rating = 5,
                Type = SurveyCategory.HospitalSurvey
            };

            var answeredQuestionDTOs = new List<AnsweredQuestionDTO>
           {
               answeredQuestionHospital
           };

            ScheduledEvent scheduledEvent = new ScheduledEvent(0, false, true, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-3).Day), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-3).Day),
                 new DateTime(), testPatient.Id, testDoctor.Id, testDoctor);
            UoW.GetRepository<IScheduledEventWriteRepository>().Add(scheduledEvent);

            var answeredSurveyDTO = new AnsweredSurveyDTO()
            {
                questions = answeredQuestionDTOs,
                AnsweredDate = DateTime.Now,
                SurveyId = survey.Id,
                ScheduledEventId = scheduledEvent.Id

            };

            return answeredSurveyDTO;
        }
    }
}
