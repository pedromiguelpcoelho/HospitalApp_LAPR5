using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Appointments
{
    public interface IAppointmentRepository : IRepository<Appointment, AppointmentId>
    {
        
        Task<List<Appointment>> SearchAsync(
            Guid? roomId, 
            AppointmentStatus? status, 
            DateTime? dateTime, 
            DateTime? dueDate, 
            List<Guid> staffIds); 

        
        Task<Appointment> GetByOperationRequestIdAsync(Guid requestId);

    
        Task<List<Appointment>> GetByPatientIdAsync(Guid patientId);

        Task<List<Appointment>> GetUpcomingAppointmentsForRoomAsync(Guid roomId, int maxResults);

        
        Task<List<Appointment>> GetAppointmentsByStaffIdAsync(Guid staffId);
        
        
        Task<List<Appointment>> GetAppointmentsBetweenDatesAsync(DateTime startDate, DateTime endDate);
    }
}
