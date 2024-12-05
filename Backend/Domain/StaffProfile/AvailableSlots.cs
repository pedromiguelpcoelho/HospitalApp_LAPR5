using System;
using System.Collections.Generic;
using System.Linq;
using DDDSample1.Domain.Shared;

// Representa um slot de disponibilidade
public class AvailabilitySlot : IValueObject
{
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    // Construtor
    public AvailabilitySlot(DateTime startTime, DateTime endTime)
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("Start time must be earlier than end time.");
        }

        StartTime = startTime;
        EndTime = endTime;
    }

    // Verifica se este slot de disponibilidade se sobrepõe a outro
    public bool OverlapsWith(AvailabilitySlot other)
    {
        return (StartTime < other.EndTime) && (EndTime > other.StartTime);
    }

    public override string ToString()
    {
        // Formatação para incluir a data e o horário
        return $"{StartTime:yyyy-MM-dd}:{StartTime:HH:mm}-{EndTime:HH:mm}"; 
    }
}