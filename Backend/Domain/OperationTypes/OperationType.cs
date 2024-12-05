using System;
using System.Collections.Generic;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.StaffProfile;

public class OperationType : Entity<OperationTypeId>, IAggregateRoot
{
    public Name Name { get; private set; } 
    public List<StaffId> RequiredStaff { get; private set; } 
    public int EstimatedDuration { get; private set; } 
    public bool isActive { get; private set; }

    // Construtor padrão para o Entity Framework
    private OperationType() { }

    public OperationType(Name name, List<StaffId> requiredStaff, int estimatedDuration)
    {
        if (name == null) throw new ArgumentNullException(nameof(name), "Name is required.");
        if (requiredStaff == null || requiredStaff.Count == 0) throw new ArgumentNullException(nameof(requiredStaff), "RequiredStaff is required and must have at least one element.");
        if (estimatedDuration <= 0) throw new ArgumentException("EstimatedDuration must be greater than zero.", nameof(estimatedDuration));

        this.Id = new OperationTypeId(Guid.NewGuid());
        this.Name = name;
        this.RequiredStaff = requiredStaff;
        this.EstimatedDuration = estimatedDuration;
        this.isActive = true;
    }

    public void UpdateDetails(Name name, List<StaffId> requiredStaff, int estimatedDuration)
    {
        if (name == null) throw new ArgumentNullException(nameof(name), "Name is required.");
        if (requiredStaff == null || requiredStaff.Count == 0) throw new ArgumentNullException(nameof(requiredStaff), "RequiredStaff is required and must have at least one element.");
        if (estimatedDuration <= 0) throw new ArgumentException("EstimatedDuration must be greater than zero.", nameof(estimatedDuration));

        this.Name = name;
        this.RequiredStaff = requiredStaff;
        this.EstimatedDuration = estimatedDuration;
    }

    public void Deactivate()
    {
        this.isActive = false;
    }
}