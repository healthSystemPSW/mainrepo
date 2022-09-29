using Hospital.MedicalRecords.Model;
using Hospital.RoomsAndEquipment.Model;
using Hospital.Schedule.Model;
using Hospital.Schedule.Service;
using Hospital.SharedModel.Model;
using HospitalUnitTests.Base;
using Shouldly;
using System;
using Xunit;

namespace HospitalUnitTests
{
    public class CancelingAppointmentTests : BaseTest
    {
      
        public CancelingAppointmentTests(BaseFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Appointment_should_be_cancelled()
        {
            //Arrange
            ScheduledEvent events = CreateEventWithDate(DateTime.Now.AddDays(3), DateTime.Now.AddDays(3));
          
            Patient testPatient = events.Patient;
            //Act
            testPatient.CancelAppointment(events.Id);
            //Assert
            testPatient.ScheduledEvents[0].IsCanceled.ShouldBeTrue();
        }

        [Fact]
        public void Appointment_should_not_be_cancelled()
        {
            //Arrange
            ScheduledEvent events = CreateEventWithDate(DateTime.Now.AddDays(2), DateTime.Now.AddDays(2));
          
            Patient testPatient = events.Patient;
            //Act
            testPatient.CancelAppointment(events.Id);
            //Assert
            testPatient.ScheduledEvents[0].IsCanceled.ShouldBeFalse();
        }

        private ScheduledEvent CreateEventWithDate(DateTime startDate, DateTime endDate)
        {
            ClearDbContext();
            Patient testPatient = new(1, "testPatient", new MedicalRecord());
            Context.Patients.Add(testPatient);

            Doctor testDoctor = new(2, new Shift().Id, new Specialization(), new Room());
            Context.Doctors.Add(testDoctor);

            ScheduledEvent scheduledEvent = new(1, 0, false, false, startDate, endDate,
                       new DateTime(), testPatient.Id, testDoctor.Id, testDoctor);

            Context.ScheduledEvents.Add(scheduledEvent);

            Context.SaveChanges();
            return scheduledEvent;
        }



        /*  [Fact]
      public void Finished_appointment_should_not_be_cancelled()
      {

          ScheduledEvent events = _scheduledEventService.GetScheduledEvent(1);
          events.SetToDone();
          Patient testPatient = events.Patient;
          testPatient.CancelAppointment(events.Id);
          _scheduledEventService.GetCanceledUserEvents("testPatient").Count.ShouldBe(0);
      */

    }
}