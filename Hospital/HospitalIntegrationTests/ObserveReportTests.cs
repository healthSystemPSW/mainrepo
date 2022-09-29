using Hospital.Schedule.Model;
using Hospital.Schedule.Repository;
using HospitalIntegrationTests.Base;
using System;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.Net;
using Hospital.SharedModel.Repository;
using Hospital.MedicalRecords.Repository;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HospitalIntegrationTests
{
    public class ObserveReportTests : BaseTest
    {
        public ObserveReportTests(BaseFixture fixture) : base(fixture)
        {
        }
        [Fact]
        public async Task Report_count_should_not_be_zeroAsync()
        {
            //Arrange
            RegisterAndLogin("Patient");
            ScheduledEvent events = ArrangeDatabase();
            events.SetToDone();
            UoW.GetRepository<IScheduledEventWriteRepository>().Update(events, true);

            //Act
            var response = await PatientClient.GetAsync(BaseUrl + "api/Report/GetReport?eventId=" + events.Id);
           
            //Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            UoW.GetRepository<IReportReadRepository>().GetReport(events.Id).ShouldNotBe(null);
            UoW.GetRepository<IReportReadRepository>().GetAll().ToList().Count.ShouldNotBe(0);

            ClearDatabase(events);
        }

        private void ClearDatabase(ScheduledEvent events)
        {
            UoW.GetRepository<IScheduledEventWriteRepository>().Delete(events, true);
            DeleteDataFromDataBase();
        }

        private ScheduledEvent ArrangeDatabase()
        {
            var testDoctor = UoW.GetRepository<IDoctorReadRepository>().GetAll().Include(d => d.Room).FirstOrDefault(x => x.UserName == "testDoctorUsername");
            var testPatient = UoW.GetRepository<IPatientReadRepository>().GetAll().Where(x => x.UserName == "testPatientUsername").FirstOrDefault();

            ScheduledEvent scheduledEvent = new ScheduledEvent(0, false, false, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(3).Day), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(3).Day),
                new DateTime(), testPatient.Id, testDoctor.Id, testDoctor);
            UoW.GetRepository<IScheduledEventWriteRepository>().Add(scheduledEvent);
           
            return scheduledEvent;
        }
    }
}
