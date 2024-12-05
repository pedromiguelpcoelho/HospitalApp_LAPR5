using Microsoft.EntityFrameworkCore;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Domain.SurgeryRooms;
using DDDSample1.Domain.Appointments;
using DDDSample1.Infrastructure.StaffProfile;
namespace DDDSample1.Infrastructure
{
    public class DDDSample1DbContext : DbContext
    {
        public DDDSample1DbContext(DbContextOptions<DDDSample1DbContext> options) : base(options)
        {}

        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<OperationRequest> OperationRequests { get; set; }
        public DbSet<PatientProfile> PatientProfiles { get; set; }
        public DbSet<SurgeryRoom> SurgeryRooms { get; set; }
        public DbSet<Staff> Staffs { get; set; }      
        public DbSet<Appointment> Appointments { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new OperationTypes.OperationTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationRequests.OperationRequestEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new Patients.PatientProfileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StaffProfile.StaffEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SurgeryRoomEntityTypeConfiguration());
             modelBuilder.ApplyConfiguration(new Appointments.AppointmentEntityTypeConfiguration());
        }
    }
}