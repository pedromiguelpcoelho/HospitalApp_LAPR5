using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.SurgeryRooms;
using System.Collections.Generic;
using System;

namespace DDDSample1.Infrastructure.StaffProfile
{
    internal class SurgeryRoomEntityTypeConfiguration : IEntityTypeConfiguration<SurgeryRoom>
    {
        public void Configure(EntityTypeBuilder<SurgeryRoom> builder)
        {
            // Define a tabela correspondente e a chave primária
            builder.ToTable("SurgeryRoom")
                .HasKey(s => s.Id);

            // Mapeia o ID personalizado
            builder.Property(s => s.Id)
                .HasConversion(
                    id => id.Value, // Converte de SurgeryRoomId para Guid
                    value => new SurgeryRoomId(value)); // Converte de Guid para SurgeryRoomId

            // Mapeia RoomNumber
            builder.Property(s => s.RoomNumber)
                .HasConversion(
                    roomNumber => roomNumber.Value,
                    value => new RoomNumber(value))
                .IsRequired();

            // Mapeia RoomType
            builder.Property(s => s.Type)
                .HasConversion(
                    type => type.Value,
                    value => new RoomType(value))
                .IsRequired();

            // Mapeia Capacity
            builder.Property(s => s.Capacity)
                .HasConversion(
                    capacity => capacity.Value,
                    value => new Capacity(value))
                .IsRequired();

            // Mapeia Status
            builder.Property(s => s.Status)
                .HasConversion(
                    status => status.Value,
                    value => new Status(value))
                .IsRequired();

            /* // Mapeia AssignedEquipment
            builder.Property(s => s.AssignedEquipment)
                .HasConversion(
                    assignedEquipment => string.Join(",", assignedEquipment),
                    value => new List<string>(value.Split(',', StringSplitOptions.RemoveEmptyEntries)))
                .IsRequired();

            // Mapeia MaintenanceSlots
            builder.OwnsMany(s => s.MaintenanceSlots, a =>
            {
                a.WithOwner().HasForeignKey("SurgeryRoomId");
                a.Property<int>("Id");
                a.HasKey("Id");
                a.Property(ms => ms.StartTime).IsRequired();
                a.Property(ms => ms.EndTime).IsRequired();
            }); */
        }
    }
}