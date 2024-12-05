using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.OperationTypes;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using DDDSample1.Domain.StaffProfile;

namespace DDDSample1.Infrastructure.OperationTypes
{
    internal class OperationTypeEntityTypeConfiguration : IEntityTypeConfiguration<OperationType>
    {
        public void Configure(EntityTypeBuilder<OperationType> builder)
        {
            // Define a tabela correspondente e a chave primária
            builder.ToTable("OperationTypes")
                .HasKey(o => o.Id);

            // Mapeia o ID personalizado
            builder.Property(o => o.Id)
            .HasConversion(
                id => id.Value, // Converte de OperationTypeId para Guid
                value => new OperationTypeId(value)); // Converte de Guid para OperationTypeId


            // Mapeia a propriedade Name
            builder.Property(e => e.Name)
                .HasConversion(
                    v => v.Value,
                    v => new Name(v));

        
            // Mapeia a lista RequiredStaff como uma propriedade JSON (dependendo do suporte do banco de dados)
            builder.Property(o => o.RequiredStaff)
            .HasConversion(
                staffList => string.Join(',', staffList.Select(s => s.Value)), // Converte a lista para string
                staffString => staffString.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(id => new StaffId(Guid.Parse(id))).ToList()) // Converte a string para lista
            .Metadata.SetValueComparer(
                new ValueComparer<List<StaffId>>(
                    (c1, c2) => c1.SequenceEqual(c2), // Comparação de elementos
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Geração de hash
                    c => c.ToList())); // Clonagem da lista


            // Mapeia a propriedade EstimatedDuration
            builder.Property(o => o.EstimatedDuration)
                .IsRequired(); // Propriedade obrigatória

            // Mapeia a propriedade isActive
            builder.Property(o => o.isActive)
                .IsRequired(); // Propriedade obrigatória
        }
    }
}