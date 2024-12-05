using System;

namespace DDDSample1.Domain.Events
{
    public class StaffDeletedEvent
    {
        public Guid StaffId { get; private set; }
        public string Name { get; private set; }
        public string LicenseNumber { get; private set; }
        public DateTime DeletedAt { get; private set; }

        public StaffDeletedEvent(Guid staffId, string name, string licenseNumber)
        {
            StaffId = staffId;
            Name = name;
            LicenseNumber = licenseNumber;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
