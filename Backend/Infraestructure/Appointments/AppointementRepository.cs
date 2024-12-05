using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.SurgeryRooms;
using DDDSample1.Domain.StaffProfile;
using Microsoft.EntityFrameworkCore;
using DDDSample1.Infrastructure.Shared;

namespace DDDSample1.Infrastructure.Appointments
{
    public class AppointmentRepository : BaseRepository<Appointment, AppointmentId>, IAppointmentRepository
    {
        private readonly DDDSample1DbContext _context;

        public AppointmentRepository(DDDSample1DbContext context) : base(context.Appointments)
        {
            _context = context;
        }

        public async Task<List<Appointment>> SearchAsync(
            Guid? roomId, 
            AppointmentStatus? status, 
            DateTime? dateTime, 
            DateTime? dueDate, 
            List<Guid> staffIds)
        {
            var query = _context.Appointments.AsQueryable();

            if (roomId.HasValue)
            {
                query = query.Where(a => a.RoomId == new SurgeryRoomId(roomId.Value));
            }

            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            if (dateTime.HasValue)
            {
                query = query.Where(a => a.StartTime.Date == dateTime.Value.Date);
            }

            if (dueDate.HasValue)
            {
                query = query.Where(a => a.StartTime.Date <= dueDate.Value.Date);
            }

            if (staffIds != null && staffIds.Any())
            {
                var staffIdsConverted = staffIds.Select(id => new StaffId(id)).ToList();
                query = query.Where(a => a.AssignedStaff.Any(s => staffIdsConverted.Contains(s)));
            }

            return await query.ToListAsync();
        }

        public async Task<Appointment> GetByOperationRequestIdAsync(Guid requestId)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.RequestId == new OperationRequestId(requestId));
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(Guid patientId)
        {
            
            return await _context.Appointments
                .Where(a => a.RequestId == new OperationRequestId(patientId)) 
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsForRoomAsync(Guid roomId, int maxResults)
        {
            return await _context.Appointments
                .Where(a => a.RoomId == new SurgeryRoomId(roomId) && a.StartTime > DateTime.UtcNow)
                .OrderBy(a => a.StartTime)
                .Take(maxResults)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByStaffIdAsync(Guid staffId)
        {
            return await _context.Appointments
                .Where(a => a.AssignedStaff.Any(s => s == new StaffId(staffId)))
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Appointments
                .Where(a => a.StartTime >= startDate && a.StartTime <= endDate)
                .ToListAsync();
        }
    }
}

