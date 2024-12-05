using System;

namespace DDDSample1.Domain.Patients
{
    public static class PatientProfileExceptions
    {
        public class InvalidEmailException() : Exception("A profile with the same email already exists.");

        public class InvalidContactInformationException() : Exception("A profile with the same contact information already exists.");



    }
}

