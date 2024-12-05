using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.SurgeryRooms;

namespace DDDSample1.Domain.SurgeryRooms
{
    public interface ISurgeryRoomRepository: IRepository<SurgeryRoom, SurgeryRoomId>
    {

    }
    
}   