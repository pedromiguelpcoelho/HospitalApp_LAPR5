using System;
using System.Collections.Generic;
using Xunit;
using DDDSample1.Domain.SurgeryRooms;

namespace DDDSample1.Tests.Domain.SurgeryRooms
{
    public class SurgeryRoomTest
    {
        [Fact]
        public void Constructor_ShouldCreateSurgeryRoom()
        {
            // Arrange
            var roomNumber = new RoomNumber(101);
            var type = new RoomType("General");
            var capacity = new Capacity(2);
            var status = new Status("Available");
            var assignedEquipment = new List<string> { "Scalpel", "Forceps" };
            var maintenanceSlots = new List<MaintenanceSlot> { new MaintenanceSlot(DateTime.Now, DateTime.Now.AddHours(1)) };

            // Act
            var surgeryRoom = new SurgeryRoom(roomNumber, type, capacity, status, assignedEquipment, maintenanceSlots);

            // Assert
            Assert.NotNull(surgeryRoom.Id);
            Assert.Equal(roomNumber, surgeryRoom.RoomNumber);
            Assert.Equal(type, surgeryRoom.Type);
            Assert.Equal(capacity, surgeryRoom.Capacity);
            Assert.Equal(status, surgeryRoom.Status);
            Assert.Equal(assignedEquipment, surgeryRoom.AssignedEquipment);
            Assert.Equal(maintenanceSlots, surgeryRoom.MaintenanceSlots);
        }

        [Fact]
        public void UpdateDetails_ShouldUpdateSurgeryRoomDetails()
        {
            // Arrange
            var roomNumber = new RoomNumber(101);
            var type = new RoomType("General");
            var capacity = new Capacity(2);
            var status = new Status("Available");
            var assignedEquipment = new List<string> { "Scalpel", "Forceps" };
            var maintenanceSlots = new List<MaintenanceSlot> { new MaintenanceSlot(DateTime.Now, DateTime.Now.AddHours(1)) };

            var surgeryRoom = new SurgeryRoom(roomNumber, type, capacity, status, assignedEquipment, maintenanceSlots);

            var newRoomNumber = new RoomNumber(102);
            var newType = new RoomType("Special");
            var newCapacity = new Capacity(3);
            var newStatus = new Status("Occupied");
            var newAssignedEquipment = new List<string> { "Scalpel", "Forceps", "Suction" };
            var newMaintenanceSlots = new List<MaintenanceSlot> { new MaintenanceSlot(DateTime.Now, DateTime.Now.AddHours(2)) };

            // Act
            surgeryRoom.UpdateDetails(newRoomNumber, newType, newCapacity, newStatus, newAssignedEquipment, newMaintenanceSlots);

            // Assert
            Assert.Equal(newRoomNumber, surgeryRoom.RoomNumber);
            Assert.Equal(newType, surgeryRoom.Type);
            Assert.Equal(newCapacity, surgeryRoom.Capacity);
            Assert.Equal(newStatus, surgeryRoom.Status);
            Assert.Equal(newAssignedEquipment, surgeryRoom.AssignedEquipment);
            Assert.Equal(newMaintenanceSlots, surgeryRoom.MaintenanceSlots);
        }
    }
}