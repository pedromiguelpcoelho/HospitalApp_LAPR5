using System;
using DDDSample1.Domain.Shared;
using Newtonsoft.Json;

namespace DDDSample1.Domain.Patients {
    public class PatientProfileId : EntityId {
        public PatientProfileId() : base(Guid.NewGuid()) { }

        [JsonConstructor]
        public PatientProfileId(Guid value) : base(value) { }

        public PatientProfileId(string value) : base(value) { }

        override
            protected object createFromString(string text) {
            return new Guid(text);
        }

        override
            public string AsString() {
            Guid obj = (Guid)base.ObjValue;
            return obj.ToString();
        }

        public Guid AsGuid() {
            return (Guid)base.ObjValue;
        }
    }
}