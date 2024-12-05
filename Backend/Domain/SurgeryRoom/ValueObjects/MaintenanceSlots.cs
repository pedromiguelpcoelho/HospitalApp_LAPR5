using System;
using DDDSample1.Domain.Shared;

public class MaintenanceSlot
{
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    public MaintenanceSlot(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
        {
            throw new BusinessRuleValidationException("End time must be after start time.");
        }

        StartTime = startTime;
        EndTime = endTime;
    }

    public override string ToString() => $"{StartTime:dd/MM/yyyy HH:mm:ss} - {EndTime:dd/MM/yyyy HH:mm:ss}";
}