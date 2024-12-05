using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.SurgeryRooms;
using Microsoft.AspNetCore.Mvc;

namespace DDDSample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurgeryRoomController : ControllerBase
    {
        private readonly SurgeryRoomService _service;

        public SurgeryRoomController(SurgeryRoomService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<SurgeryRoomDto>> Create([FromBody] CreatingSurgeryRoomDto dto)
        {
            try
            {
                var room = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetAll), new { id = room.Id }, room);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<SurgeryRoomDto>>> GetAll()
        {
            var rooms = await _service.GetAllAsync();
            return Ok(rooms);
        }
    }

    public class SurgeryRoomDto
    {
    }
}
