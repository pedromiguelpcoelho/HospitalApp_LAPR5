using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DDDSample1.Domain.StaffProfile;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Policy = "AdminsOrDoctors")]
    public class StaffController : ControllerBase
    {
        private readonly StaffService _service;

        public StaffController(StaffService service)
        {
            _service = service;
        }

        // POST: api/staff
        [HttpPost]
        public async Task<IActionResult> AddStaff(CreatingStaffDto dto)
        {

             // Check if email is already in use
            var existingStaffWithEmail = await _service.GetByEmailAsync(dto.Email);
            if (existingStaffWithEmail != null)
            {
                return Conflict(new { Message = "The email is already in use by another staff member." });
            }

            // Check if phone number is already in use
            var existingStaffWithPhoneNumber = await _service.GetByPhoneNumberAsync(dto.PhoneNumber);
            if (existingStaffWithPhoneNumber != null)
            {
                return Conflict(new { Message = "The phone number is already in use by another staff member." });
            }

            var createdStaff = await _service.AddAsync(dto);
            var result = new StaffDTO
            {
                Id = createdStaff.Id,                        
                FirstName = createdStaff.FirstName,             
                LastName = createdStaff.LastName,               
                Name = createdStaff.Name,
                Role = createdStaff.Role,                        
                LicenseNumber = createdStaff.LicenseNumber,    
                Specialization = createdStaff.Specialization,          
                Email = createdStaff.Email,                     
                PhoneNumber = createdStaff.PhoneNumber,        
                IsActive = createdStaff.IsActive  
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/staff/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditStaffProfile(Guid id, [FromBody] CreatingStaffDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedStaff = await _service.UpdateAsync(id, dto);

            if (updatedStaff == null)
            {
                return NotFound(new { Message = "Staff member not found or update failed." });
            }

            return Ok(new
            {
                FirstName = updatedStaff.FirstName,
                LastName = updatedStaff.LastName,
                Name = updatedStaff.Name,
                Specialization = updatedStaff.Specialization,
                Email = updatedStaff.Email,
                PhoneNumber = updatedStaff.PhoneNumber,
                IsActive = updatedStaff.IsActive
               
            });
        }

           // DELETE: api/staff/{id} (Deactivate Staff Profile)
           [HttpDelete("{id}")]
            public async Task<IActionResult> DeactivateStaff(Guid id)
            {
                var staff = await _service.GetByIdAsync(id);

                if (staff == null)
                {
                    return NotFound(new { Message = "Staff member not found." });
                }

                var isDeactivated = await _service.DeleteAsync(id);
                if (!isDeactivated)
                {
                    return BadRequest(new { Message = "Failed to deactivate staff profile." });
                }

                return Ok(new { Message = "Staff profile successfully deactivated." });
        }


        // GET: api/staff
        [HttpGet("search")]
        public async Task<ActionResult<List<StaffDTO>>> SearchCriteriaStaffProfiles(
            [FromQuery] string name, 
            [FromQuery] string specialization,
            [FromQuery] string email,
            [FromQuery] string phoneNumber,
            [FromQuery] string licenseNumber, 
            [FromQuery]  bool? isActive = null)
        
        {    
            var staffList = await _service.SearchStaffProfilesAsync(name,email ,phoneNumber,specialization, licenseNumber, isActive);
            return Ok(staffList);
        }


        // GET: api/staff
         [HttpGet("all")]
        public async Task<ActionResult<StaffDTO>> GetAllStaff()
        {
            var staffList = await _service.GetAllAsync();
            return Ok(staffList);
        } 
    

        // GET: api/staff/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var staff = await _service.GetByIdAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return Ok(staff);
        }

    }
}
