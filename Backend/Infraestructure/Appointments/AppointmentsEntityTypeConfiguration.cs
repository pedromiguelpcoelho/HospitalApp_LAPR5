using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Appointments;
using System.Collections.Generic;
using DDDSample1.Domain.OperationRequests;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using DDDSample1.Domain.SurgeryRooms;
using DDDSample1.Domain.StaffProfile;
using System;
using System.Linq;

namespace DDDSample1.Infrastructure.Appointments
{
    internal class AppointmentEntityTypeConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            // Define a tabela correspondente e a chave primária
            builder.ToTable("Appointments")
                .HasKey(a => a.Id);

            // Mapeia o ID personalizado
            builder.Property(a => a.Id)
                .HasConversion(
                    id => id.Value, // Converte de AppointmentId para Guid
                    value => new AppointmentId(value)) // Converte de Guid para AppointmentId
                .IsRequired();

            // Mapeia o RequestId
            builder.Property(a => a.RequestId)
                .HasConversion(
                    requestId => requestId.Value, // Converte de OperationRequestId para Guid
                    value => new OperationRequestId(value)) // Converte de Guid para OperationRequestId
                .IsRequired();

            // Mapeia o RoomId
            builder.Property(a => a.RoomId)
                .HasConversion(
                    roomId => roomId.Value, // Converte de SurgeryRoomId para Guid
                    value => new SurgeryRoomId(value)) // Converte de Guid para SurgeryRoomId
                .IsRequired();

            // Mapeia a data e hora do agendamento
            builder.Property(a => a.StartTime)
                .IsRequired();

            // Mapeia a duração do agendamento
            builder.Property(a => a.Duration)
                .IsRequired();

            builder.Property(a => a.Status)
            .HasConversion(
                status => status.ToString(), // Enum -> string
                value => Enum.Parse<AppointmentStatus>(value)) // string -> Enum
            .IsRequired();


      // Mapeia a lista RequiredStaff como uma propriedade JSON (dependendo do suporte do banco de dados)
            builder.Property(o => o.AssignedStaff)
            .HasConversion(
                staffList => string.Join(',', staffList.Select(s => s.Value)), // Converte a lista para string
                staffString => staffString.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(id => new StaffId(Guid.Parse(id))).ToList()) // Converte a string para lista
            .Metadata.SetValueComparer(
                new ValueComparer<List<StaffId>>(
                    (c1, c2) => c1.SequenceEqual(c2), // Comparação de elementos
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Geração de hash
                    c => c.ToList())); // Clonagem da lista


        }
    }
}
