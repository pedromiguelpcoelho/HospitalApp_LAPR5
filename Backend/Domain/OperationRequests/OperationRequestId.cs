using System;
using DDDSample1.Domain.Shared;
using Newtonsoft.Json;

namespace DDDSample1.Domain.OperationRequests
{
    public class OperationRequestId : EntityId
    {
        public OperationRequestId() : base(Guid.NewGuid()) { }

        [JsonConstructor]
        public OperationRequestId(Guid value) : base(value) { }

        public OperationRequestId(string value) : base(value) { }

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