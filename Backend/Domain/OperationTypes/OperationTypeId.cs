using System;
using DDDSample1.Domain.Shared;
using Newtonsoft.Json;

namespace DDDSample1.Domain.OperationTypes
{
    public class OperationTypeId : EntityId
    {
        // Construtor padrão para o Entity Framework
        public OperationTypeId() : base(Guid.NewGuid()) { } 

        [JsonConstructor]
        public OperationTypeId(Guid value) : base(value) { }

        public OperationTypeId(string value) : base(value) { }

        // Cria um Guid a partir de uma string
        override
        protected object createFromString(string text)
        {
            return new Guid(text);
        }

        // Retorna o valor do Guid como string
        override
        public string AsString()
        {
            Guid obj = (Guid)base.ObjValue;
            return obj.ToString();
        }

        // Retorna o valor como Guid
        public Guid AsGuid()
        {
            return (Guid)base.ObjValue;
        }
    }
}
