using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class CreatingOperationTypeDto
{
    public string Name { get; set; }
    public List<string> RequiredStaff { get; set; }
    public int EstimatedDuration { get; set; }

    public CreatingOperationTypeDto(string name, List<string> requiredStaff, int estimatedDuration)
    {
        this.Name = name;
        this.RequiredStaff = requiredStaff;
        this.EstimatedDuration = estimatedDuration;
    }
}