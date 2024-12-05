using System.Threading.Tasks;
using DDDSample1.Domain.HospitalMaps;
using Microsoft.AspNetCore.Mvc;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalMapController : ControllerBase
    {
        private readonly HospitalMapService _hospitalMapService;

        public HospitalMapController(HospitalMapService hospitalMapService)
        {
            this._hospitalMapService = hospitalMapService;
        }

        // GET: api/hospitalMap
        [HttpGet]
        public async Task<IActionResult> GetHospitalMap()
        {
            var hospitalMap = await _hospitalMapService.GetHospitalMapAsync();
            
            if (hospitalMap == null)
            {
                return NotFound("Hospital Map not found.");
            }

            return Ok(hospitalMap);
        }
    }
}

