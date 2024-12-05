using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Domain.SurgeryRooms;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.OperationRequests;

namespace DDDSample1.Domain.Appointments
{
    public class AppointmentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAppointmentRepository repoAppointment;
        private readonly IStaffRepository repoStaff;
        private readonly ISurgeryRoomRepository repoSurgeryRoom;
        private readonly IPatientProfileRepository repoPatient;

        public AppointmentService(
            IUnitOfWork unitOfWork,
            IAppointmentRepository repoAppointment,
            IStaffRepository repoStaff,
            ISurgeryRoomRepository repoSurgeryRoom,
            IPatientProfileRepository repoPatient
        )
        {
            this.unitOfWork = unitOfWork;
            this.repoAppointment = repoAppointment;
            this.repoStaff = repoStaff;
            this.repoSurgeryRoom = repoSurgeryRoom;
            this.repoPatient = repoPatient;
        }

        
        public async Task<AppointmentDto> AddAsync(CreatingAppointmentDto dto)
        {
            
            await ValidateStaffIdAsync(dto.AssignedStaff);
            await ValidateRoomAvailabilityAsync(dto.RoomId, dto.DateTime, dto.Duration);
                 
            var appointment = new Appointment(
                new OperationRequestId(dto.RequestId), 
                new SurgeryRoomId(dto.RoomId), 
                dto.DateTime, 
                dto.Duration, 
                dto.Status, 
                new List<StaffId>(dto.AssignedStaff.Select(id => new StaffId(id))));

            
            await repoAppointment.AddAsync(appointment);
            await unitOfWork.CommitAsync();


            return await MapToDto(appointment);
        }

        public async Task<Appointment> GetByIdAsync(Guid id)
        {
            var appointment = await repoAppointment.GetByIdAsync(new AppointmentId(id));
            return appointment;
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            var appointments = await repoAppointment.GetAllAsync();
            var appointmentDtos = new List<AppointmentDto>();

            foreach (var appointment in appointments)
            {
                var appointmentDto = await MapToDto(appointment);
                appointmentDtos.Add(appointmentDto);
            }

            return appointmentDtos;
        }
       
        private async Task ValidateStaffIdAsync(List<Guid> staffIds)
        {
            foreach (var staffId in staffIds)
            {
                var staff = await repoStaff.GetByIdAsync(new StaffId(staffId));
                if (staff == null)
                {
                    throw new ArgumentException($"Staff member with ID {staffId} does not exist.");
                }
            }
        }

        
        private async Task ValidateRoomAvailabilityAsync(Guid roomId, DateTime dateTime, TimeSpan duration)
        {
            var room = await repoSurgeryRoom.GetByIdAsync(new SurgeryRoomId(roomId));
            if (room == null)
            {
                throw new ArgumentException("Surgery room not found.");
            }

           
            var appointments = await repoAppointment.SearchAsync(roomId, null, dateTime, null, null);
            foreach (var appt in appointments)
            {
                if (appt.StartTime < dateTime.Add(duration) && appt.StartTime.Add(appt.Duration) > dateTime)
                {
                    throw new ArgumentException("The room is already booked for the selected time.");
                }
            }
        }

        
        private async Task ValidatePatientAppointmentAsync(Guid patientId)
        {
            var existingAppointments = await repoAppointment.GetByPatientIdAsync(patientId);
            if (existingAppointments.Any())
            {
                throw new ArgumentException("Patient already has an open appointment.");
            }
        }

        
        private async Task<AppointmentDto> MapToDto(Appointment appointment)
        {
            var staff = await repoStaff.GetByIdAsync(appointment.AssignedStaff.First()); 
            var room = await repoSurgeryRoom.GetByIdAsync(appointment.RoomId);

            return new AppointmentDto
            {
                Id = appointment.Id.AsGuid(),
                RoomId = appointment.RoomId.AsGuid(),
                DateTime = appointment.StartTime,
                Duration = appointment.Duration,
                Status = appointment.Status,
                AssignedStaff = appointment.AssignedStaff.Select(s => s.AsGuid()).ToList(),
            };
        }
    }
}
