using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.SurgeryRooms
{
    public class SurgeryRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISurgeryRoomRepository _repo;

        public SurgeryRoomService(IUnitOfWork unitOfWork, ISurgeryRoomRepository repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }

        public async Task<SurgeryRoomDTO> AddAsync(CreatingSurgeryRoomDto dto)
        {
            var surgeryRoom = new SurgeryRoom(
                new RoomNumber(int.Parse(dto.RoomNumber)),
                new RoomType(dto.RoomType),
                new Capacity(dto.Capacity),
                new Status(dto.Status)
            );

            await _repo.AddAsync(surgeryRoom);
            await _unitOfWork.CommitAsync();

            return new SurgeryRoomDTO(
                surgeryRoom.Id.AsGuid(),
                surgeryRoom.RoomNumber.ToString(),
                surgeryRoom.Capacity,
                surgeryRoom.Status.ToString(),
                surgeryRoom.Type.ToString()
            );
        }

        public async Task<List<SurgeryRoomDTO>> GetAllAsync()
        {
            var rooms = await _repo.GetAllAsync();
            return rooms.Select(surgeryRoom => new SurgeryRoomDTO(
                surgeryRoom.Id.AsGuid(),
                surgeryRoom.RoomNumber.ToString(),
                surgeryRoom.Capacity,
                surgeryRoom.Type.ToString(),
                surgeryRoom.Status.ToString()
            )).ToList();
        }
    }
}
