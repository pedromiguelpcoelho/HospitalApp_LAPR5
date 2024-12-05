using System;

namespace DDDSample1.Domain.SurgeryRooms {
    public class SurgeryRoomDTO {
        public Guid Id { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }

        public SurgeryRoomDTO(Guid id, string roomNumber, int capacity, string RoomType, string status) {
            this.Id = id;
            this.RoomNumber = roomNumber;
            this.Capacity = capacity;
            this.RoomType = RoomType;
            this.Status = status;
    }
}
}