using System;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.SurgeryRooms {
    public class SurgeryRoom : Entity<SurgeryRoomId>, IAggregateRoot
    {
        public RoomNumber RoomNumber { get; set; }
        public RoomType Type { get; set; }
        public Capacity Capacity { get; set; }
        public Status Status { get; set; }
        //public List<string> AssignedEquipment { get; set; }
        //public List<MaintenanceSlot> MaintenanceSlots { get; set; }

        // Construtor sem parâmetros necessário para o EF Core
        protected SurgeryRoom() { }

        public SurgeryRoom(RoomNumber roomNumber, RoomType type, Capacity capacity, Status status)
        {
            this.Id = new SurgeryRoomId(Guid.NewGuid());
            this.RoomNumber = roomNumber;
            this.Type = type;
            this.Capacity = capacity;
            this.Status = status;
            //this.AssignedEquipment = assignedEquipment;
            //this.MaintenanceSlots = maintenanceSlots;
        }

        public void UpdateDetails(RoomNumber roomNumber, RoomType type, Capacity capacity, Status status)
        {
            this.RoomNumber = roomNumber;
            this.Type = type;
            this.Capacity = capacity;
            this.Status = status;
            //this.AssignedEquipment = assignedEquipment;
            //this.MaintenanceSlots = maintenanceSlots;
        }
    }
}