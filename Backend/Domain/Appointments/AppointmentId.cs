using System;
using DDDSample1.Domain.Shared;
using Newtonsoft.Json;

namespace DDDSample1.Domain.Appointments
{
    public class AppointmentId : EntityId
    {
        public AppointmentId() : base(Guid.NewGuid()) { }

        [JsonConstructor]
        public AppointmentId(Guid value) : base(value) { }

        public AppointmentId(string value) : base(value) { }

        protected override object createFromString(string text)
        {
            return new Guid(text);
        }

        public override string AsString()
        {
            return ((Guid)ObjValue).ToString();
        }

        public Guid AsGuid()
        {
            return (Guid)ObjValue;
        }
    }
}
