using System;
using System.Linq;
using Hospital.MedicalRecords.Model;
using Hospital.RoomsAndEquipment.Model;
using Hospital.Schedule.Model;
using Hospital.Schedule.Service;
using Hospital.SharedModel.Model;
using Hospital.SharedModel.Model.Enumerations;
using HospitalUnitTests.Base;
using Shouldly;
using Xunit;

namespace HospitalUnitTests
{
    public class BlockingMaliciousPatientsTests : BaseTest
    {
        private readonly BlockingService _blockingService;

        public BlockingMaliciousPatientsTests(BaseFixture fixture) : base(fixture)
        {
            _blockingService = new BlockingService(UoW);
        }

        [Fact]
        public void Patient_should_be_malicious()
        {
            #region Arange

            ClearDbContext();
            //Context.Rooms.Add(new Room { Id = 1, Name = "Ordination" });
            (Doctor testDoctor, Patient testPatient) = ArrangeData();
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(-100),
                 DateTime.Now.AddDays(-100), DateTime.Now.AddDays(-110), testPatient.Id, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, false, true, DateTime.Now.AddDays(-18),
                DateTime.Now.AddDays(-18), new DateTime(), testPatient.Id, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(-22),
                 DateTime.Now.AddDays(-22), DateTime.Now.AddDays(-27), testPatient.Id, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(-4),
                 DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-14), testPatient.Id, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(7),
                 DateTime.Now.AddDays(7), DateTime.Now, testPatient.Id, 1, testDoctor));

            Context.SaveChanges();
            #endregion
          
            var maliciousPatients = _blockingService.GetMaliciousPatients();
            maliciousPatients.ShouldNotBe(null);
            maliciousPatients.ShouldContain(testPatient);
        }

        [Fact]
        public void Patient_should_not_be_malicious()
        {
            #region Arange

            ClearDbContext();
            (Doctor testDoctor, Patient testPatient) = ArrangeData();

            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(-100),
                 DateTime.Now.AddDays(-100), DateTime.Now.AddDays(-110), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, false, true, DateTime.Now.AddDays(-22),
                DateTime.Now.AddDays(-22), new DateTime(), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, false, true, DateTime.Now.AddDays(-19),
                DateTime.Now.AddDays(-19), new DateTime(), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(-4),
                 DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-14), 2, 1, testDoctor));

            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(7),
                 DateTime.Now.AddDays(7), DateTime.Now.AddDays(-1), 2, 1, testDoctor));

            Context.SaveChanges();
            #endregion

            var maliciousPatients = _blockingService.GetMaliciousPatients();
            maliciousPatients.ShouldNotBe(null);
            maliciousPatients.ShouldNotContain(testPatient);
        }

        [Fact]
        public void Patient_should_be_malicious_and_blocked()
        {
            #region Arrange

            ClearDbContext();
            (Doctor testDoctor, Patient testPatient) = ArrangeData();

            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(-100),
                 DateTime.Now.AddDays(-100), DateTime.Now.AddDays(-110), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, false, true, DateTime.Now.AddDays(-18),
                DateTime.Now.AddDays(-18), new DateTime(), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(-22),
                 DateTime.Now.AddDays(-22), DateTime.Now.AddDays(-27), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, false, true, DateTime.Now.AddDays(-19),
                DateTime.Now.AddDays(-19), new DateTime(), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(-4),
                 DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-14), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(7),
                 DateTime.Now.AddDays(7), DateTime.Now, 2, 1, testDoctor));

            Context.SaveChanges();

            #endregion

            _blockingService.BlockMaliciousPatient(testPatient);
           /*var maliciousPatients = _blockingService.GetMaliciousPatients().ToList();
            maliciousPatients.ShouldNotContain(testPatient);*/
            testPatient.IsBlocked.ShouldBe(true);

        }
        [Fact]
        public void Patient_should_not_be_blocked()
        {
            #region Arange

            ClearDbContext();
            (Doctor testDoctor, Patient testPatient) = ArrangeData();

            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(-100),
                 DateTime.Now.AddDays(-100), DateTime.Now.AddDays(-110), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, false, true, DateTime.Now.AddDays(-22),
                DateTime.Now.AddDays(-22), new DateTime(), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, false, true, DateTime.Now.AddDays(-19),
                DateTime.Now.AddDays(-19), new DateTime(), 2, 1, testDoctor));
            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(-4),
                 DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-14), 2, 1, testDoctor));

            Context.ScheduledEvents.Add(new ScheduledEvent(ScheduledEventType.Appointment, true, false, DateTime.Now.AddDays(7),
                 DateTime.Now.AddDays(7), DateTime.Now.AddDays(-1), 2, 1, testDoctor));

            Context.SaveChanges();
            #endregion
          
            _blockingService.BlockMaliciousPatient(testPatient);
            testPatient.IsBlocked.ShouldBe(false);           
        }

        private (Doctor doctor, Patient patient) ArrangeData()
        {
            Context.Shifts.Add(new Shift(3, "first", 7, 15));
            var room = new Room()
            {
                Id = 2,
                BuildingName = "TestHospital",
                Description = "TestHospital",
                FloorNumber = 2,
                Name = "TestRoom"
            };
            Context.Rooms.Add(room);
            var testDoctor = new Doctor(1, 3, new Specialization("General Practice", ""), room);
            Context.Doctors.Add(testDoctor);

            var testPatient = new Patient(2, "testPatient", new MedicalRecord(1, null, 0, 0, 1, null));
            Context.Patients.Add(testPatient);
            return (testDoctor, testPatient);
        }
    }
}
