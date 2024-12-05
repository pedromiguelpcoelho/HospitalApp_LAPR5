using System;

namespace DDDSample1.Domain.OperationRequests
{
    public static class OperationRequestExceptions
    {
        public class InvalidSuggestedDeadlineException() : Exception("Suggested deadline must be in the future.");
        
        public class InvalidOperationTypeIdException() : Exception("Operation type ID must be valid.");
        
        public class InvalidPatientIdException() : Exception("Patient ID must be valid.");
        
        public class InvalidDoctorIdException() : Exception("Doctor ID must be valid.");
        
        public class DuplicateOpenRequestException()
            : Exception("An open request for the same patient and operation type already exists.");
        
        // public class InvalidDoctorOrOperationTypeException() : Exception("Invalid doctor or operation type.");
        //
        // public class DoctorSpecializationMismatchException()
        //     : Exception("Doctor's specialization does not match the operation type.");
    }
}

