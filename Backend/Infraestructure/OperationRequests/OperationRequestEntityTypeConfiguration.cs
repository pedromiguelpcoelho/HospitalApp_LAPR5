using System;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.StaffProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDSample1.Infrastructure.OperationRequests
{
    internal class OperationRequestEntityTypeConfiguration : IEntityTypeConfiguration<OperationRequest>
    {
        public void Configure(EntityTypeBuilder<OperationRequest> builder)
        {
            builder.ToTable("OperationRequests")
                .HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasConversion(
                    id => id.Value,
                    value => new OperationRequestId(value));

            builder.Property(o => o.PatientId)
                .IsRequired()
                .HasConversion(
                    id => id.Value,
                    value => new PatientProfileId(value));

            builder.Property(o => o.DoctorId)
                .IsRequired()
                .HasConversion(
                    id => id.Value,
                    value => new StaffId(value));

            builder.Property(o => o.OperationTypeId)
                .IsRequired()
                .HasConversion(
                    id => id.Value,
                    value => new OperationTypeId(value));

            builder.Property(o => o.Priority)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (Priority)Enum.Parse(typeof(Priority), v))
                .HasMaxLength(10);

            builder.Property(o => o.SuggestedDeadline)
                .IsRequired();
            
            builder.Property(o => o.RequestDate)
                .IsRequired();
        }
    }
}