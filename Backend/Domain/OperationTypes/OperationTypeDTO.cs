
using System;

using System.Collections.Generic;



namespace DDDSample1.Domain.OperationTypes

{

    public class OperationTypeDTO

    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<string> RequiredStaff { get; set; }  

        public int EstimatedDuration { get; set; }

        public bool isActive { get; set; }

    }

}
