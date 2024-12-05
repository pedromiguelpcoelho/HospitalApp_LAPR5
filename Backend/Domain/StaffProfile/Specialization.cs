using System;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;

public class Specialization : IValueObject
{
    public string Value { get; private set; }

    protected Specialization()
    {
    }

    public Specialization(string value, string role)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Specialization cannot be null or empty.");
        }

        
        if (role == "Doctor" && !IsValidDoctorSpecialization(value))
        {
            throw new ArgumentException($"A Doctor cannot have the specialization {value}. Valid specializations for Doctor are: Orthopaedist, Anaesthetist.");
        }

        if (role == "Nurse" && !IsValidNurseSpecialization(value))
        {
            throw new ArgumentException($"A Nurse cannot have the specialization {value}. Valid specializations for Nurse are: Instrumenting Nurse, Circulating Nurse, Nurse Anaesthetist, Medical Action Assistant.");
        }

        if (role == "Other" && !IsValidOtherSpecialization(value))
        {
            throw new ArgumentException($"An Other cannot have the specialization {value}. Valid specialization for Other is: X-ray Technician.");
        }

        Value = value; 
    }

    
    private static bool IsValidDoctorSpecialization(string specialization)
    {
        var validDoctors = new HashSet<string>
        {
            "Orthopaedist",
            "Anaesthetist"
        };
        return validDoctors.Contains(specialization);
    }

    
    private static bool IsValidNurseSpecialization(string specialization)
    {
        var validNurses = new HashSet<string>
        {
            "Instrumenting Nurse",
            "Circulating Nurse",
            "Nurse Anaesthetist",
            "Medical Action Assistant"
        };
        return validNurses.Contains(specialization);
    }

    
    private static bool IsValidOtherSpecialization(string specialization)
    {
        var validOthers = new HashSet<string>
        {
            "X-ray Technician"
        };
        return validOthers.Contains(specialization);
    }

   
    public static implicit operator string(Specialization specialization) => specialization.Value;

    public override string ToString() => Value; 
}
