using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Patients;
using System;

namespace DDDSample1.Infrastructure.Patients {
    internal class PatientProfileEntityTypeConfiguration : IEntityTypeConfiguration<PatientProfile> {
        public void Configure(EntityTypeBuilder<PatientProfile> builder) {
            // Define the corresponding table and primary key
            builder.ToTable("PatientProfiles")
                .HasKey(p => p.Id);

            // Map the custom ID
            builder.Property(p => p.Id)
                .HasConversion(
                    id => id.Value, // Convert from PatientProfileId to Guid
                    value => new PatientProfileId(value)); // Convert from Guid to PatientProfileId

            // Map the properties
            builder.Property(e => e.FirstName)
                .HasConversion(
                    v => v.Value,
                    v => new FirstName(v));

            builder.Property(e => e.LastName)
                .HasConversion(
                    v => v.Value,
                    v => new LastName(v));
            
            builder.Property(e => e.FullName)
                .HasConversion(
                    v => v.Value,
                    v => new FullName(v));

            builder.Property(p => p.DateOfBirth)
                .IsRequired()
                .HasColumnType("DATE");

            builder.Property(e => e.Email)
                .HasConversion(
                    v => v.Value,
                    v => new Email(v));

            builder.Property(e => e.ContactInformation)
                .HasConversion(
                    v => v.Value,
                    v => new ContactInformation(v));

            builder.Property(p => p.Gender)
                .HasConversion(
                    g => g.ToString(),
                    g => (Gender)Enum.Parse(typeof(Gender), g)); 

            builder.Property(p => p.EmergencyContact)
                .HasColumnType("BIGINT");

            builder.Property(e => e.MedicalRecordNumber)
                .HasConversion(
                    v => v.Value,
                    v => new MedicalRecordNumber(v));

            builder.Property(p => p.AllergiesMedicalCond)
                .HasMaxLength(500); 

            builder.Property(p => p.AppointmentHistory)
                .HasMaxLength(1000);

            builder.Property(p => p.IsMarkedForDeletion)
                .IsRequired();

            builder.Property(p => p.DeletionScheduledDate)
                .IsRequired(false);  
        }
    }
}