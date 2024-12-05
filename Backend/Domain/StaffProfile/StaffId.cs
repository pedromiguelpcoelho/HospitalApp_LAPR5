using System;
using DDDSample1.Domain.Shared;
using Newtonsoft.Json;

namespace DDDSample1.Domain.StaffProfile
{
    public class StaffId : EntityId
    {
        // Default constructor generating a new Guid for the StaffId
        public StaffId() : base(Guid.NewGuid()) { }

        // Constructor to handle JSON deserialization with an explicit Guid
        [JsonConstructor]
        public StaffId(Guid value) : base(value) { }

        // Constructor to handle creation from a string value
        public StaffId(string value) : base(value) { }

        // Creates a Guid from a string representation
        override
        protected object createFromString(string text)
        {
            return new Guid(text);
        }

        // Returns the StaffId as a string
        override
        public string AsString()
        {
            Guid obj = (Guid)base.ObjValue;
            return obj.ToString();
        }

        // Returns the StaffId as a Guid
        public Guid AsGuid()
        {
            return (Guid)base.ObjValue;
        }
    }
}
