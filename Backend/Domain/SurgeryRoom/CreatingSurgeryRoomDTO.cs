using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.SurgeryRooms {
    public class CreatingSurgeryRoomDto {
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }

        public CreatingSurgeryRoomDto(string roomNumber, string type, int capacity, string status) {
            this.RoomNumber = roomNumber;
            this.RoomType = type;
            this.Capacity = capacity;
            this.Status = status;
        }

        public CreatingSurgeryRoomDto() {
            // Parameterless constructor for JSON deserialization
        }
    }
}