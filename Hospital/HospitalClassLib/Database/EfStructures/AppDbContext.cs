using Hospital.EventStoring.Model;
using Hospital.MedicalRecords.Model;
using Hospital.RoomsAndEquipment.Model;
using Hospital.Schedule.Model;
using Hospital.SharedModel.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Database.EfStructures
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<HospitalTreatment> HospitalTreatments { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<MedicationInventory> MedicationInventory { get; set; }
        public DbSet<MedicationIngredient> MedicationIngredients { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Referral> Referrals { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomInventory> RoomInventories { get; set; }
        public DbSet<ScheduledEvent> ScheduledEvents { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<AnsweredQuestion> AnsweredQuestions { get; set; }
        public DbSet<AnsweredSurvey> AnsweredSurveys { get; set; }
        public DbSet<EquipmentTransferEvent> EquipmentTransferEvents { get; set; }
        public DbSet<MedicationExpenditureLog> MedicationExpenditureLogs { get; set; }
        public DbSet<RoomRenovationEvent> RoomRenovationEvents { get; set; }
        public DbSet<StoredEvent> StoredEvents { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<OnCallDuty> OnCallDuties { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedule { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Room>().OwnsOne(r => r.RoomPosition);
            builder.Entity<Doctor>().HasOne(d => d.DoctorSchedule);
            builder.Entity<DoctorSchedule>().OwnsMany(ds => ds.Vacations);
            builder.Entity<OnCallDuty>().HasMany(oc => oc.DoctorsOnDuty).WithMany(ds => ds.OnCallDuties);
            builder.Entity<User>().OwnsOne(u => u.City, c => c.OwnsOne(x => x.Country));
            builder.Entity<Doctor>().OwnsOne(d => d.Specialization);
            builder.Entity<MedicalRecord>().OwnsOne(d => d.Measurements);
            builder.Entity<EquipmentTransferEvent>().OwnsOne(et => et.TimePeriod);
            builder.Entity<EquipmentTransferEvent>().HasOne(et => et.InitialRoomInventory);
            builder.Entity<EquipmentTransferEvent>().HasOne(et => et.DestinationRoomInventory);

            base.OnModelCreating(builder);
        }
    }
}