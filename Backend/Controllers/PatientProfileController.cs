using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DDDSample1.Domain.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientProfileController : ControllerBase
    {
        private readonly PatientProfileService _service;

        public PatientProfileController(PatientProfileService service)
        {
            _service = service;
        }


        //-----------------------------------------------------------------------------------------
        //ADD METHOD
        [Authorize (Policy = "AdminsOnly")]
        [HttpPost]
        public async Task<IActionResult> AddPatientProfile(CreatingPatientProfileDto dto)
        {
            try {

                var createdPatientProfile = await _service.AddAsync(dto);
                var result = new PatientProfileDTO
                {
                    Id = createdPatientProfile.Id,
                    FirstName = createdPatientProfile.FirstName,
                    LastName = createdPatientProfile.LastName,
                    FullName = createdPatientProfile.FullName,
                    DateOfBirth = createdPatientProfile.DateOfBirth,
                    Email = createdPatientProfile.Email,
                    ContactInformation = createdPatientProfile.ContactInformation,
                    MedicalRecordNumber = createdPatientProfile.MedicalRecordNumber
                };

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            
            } 
            catch (PatientProfileExceptions.InvalidEmailException ex) 
            {
                return Conflict(new { message = ex.Message });
            } 
            catch (PatientProfileExceptions.InvalidContactInformationException ex) 
            {
                return Conflict(new { message = ex.Message });

            } 
            catch (Exception ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        //-----------------------------------------------------------------------------------------
        //UPDATE METHO
        [Authorize(Policy = "AdminsOnly")]        
        [HttpPut("update")]
        public async Task<IActionResult> UpdatePatientProfile(
            [FromBody] CreatingPatientProfileDto dto,
            [FromQuery] string email = null,
            [FromQuery] string medicalRecordNumber = null,
            [FromQuery] string fullName = null,
            [FromQuery] string dateOfBirth = null,
            [FromQuery] string phoneNumber = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPatientProfile =
                await _service.UpdateAsync(dto, email, medicalRecordNumber, fullName, dateOfBirth, phoneNumber);
            if (updatedPatientProfile == null)
            {
                return NotFound(new { Message = "Patient profile not found." });
            }

            return Ok(updatedPatientProfile);
        }

        [Authorize(Policy = "PatientsOnly")]
        [HttpPut("updatePatient")]
        public async Task<IActionResult> UpdatePatientProfilePatient([FromBody] CreatingPatientProfileDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User email not found.");
            }

            var updatedPatientProfile = await _service.UpdateAsyncPatient(dto, userEmail);
            if (updatedPatientProfile == null)
            {
                return NotFound(new { Message = "The email you entered is not registered. Please try again." });
            }

            return Ok(updatedPatientProfile);
}

        //-----------------------------------------------------------------------------------------
        //DELETE METHOD
        [Authorize(Policy = "AdminsOnly")]        
        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePatientProfile(
            [FromQuery] string dateOfBirth = null,
            [FromQuery] string medicalRecordNumber = null,
            [FromQuery] string email = null, 
            [FromQuery] string fullName = null, 
            [FromQuery] string phoneNumber = null)
        {
            if (string.IsNullOrWhiteSpace(medicalRecordNumber) && string.IsNullOrWhiteSpace(email) &&
                string.IsNullOrWhiteSpace(fullName) && string.IsNullOrWhiteSpace(phoneNumber) &&
                string.IsNullOrWhiteSpace(dateOfBirth))
            {
                return BadRequest(new
                {
                    Message ="At least one of the following parameters must be provided: medicalRecordNumber, email or fullName."
                });
            }

            var isDeleted = await _service.DeleteAsync(dateOfBirth, medicalRecordNumber, email, fullName, phoneNumber);
            if (!isDeleted)
            {
                return NotFound(new { Message = "Patient profile not found." });
            }

            return Ok(new { Message = "Patient profile successfully deleted." });
        }


        //-----------------------------------------------------------------------------------------
        //GET METHODS
     
        [HttpGet("id/{id}")]
        [Authorize(Policy = "AdminsOnly")]
        public async Task<ActionResult<PatientProfileDTO>> GetById(Guid id)
        {
            var patientProfile = await _service.GetByIdAsync(id);
            if (patientProfile == null)
            {
                return NotFound();
            }

            return Ok(patientProfile);
        }

        [Authorize(Policy = "AdminsOrDoctors")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PatientProfileDTO>>> GetAll(
            [FromQuery] string email = null,
            [FromQuery] string medicalRecordNumber = null,
            [FromQuery] string fullName = null,
            [FromQuery] string dateOfBirth = null,
            [FromQuery] string phoneNumber = null)
        {
            var patientProfiles =
                await _service.GetAllAsync(email, medicalRecordNumber, fullName, dateOfBirth, phoneNumber);
            return Ok(patientProfiles);
        }

        //-----------------------------------------------------------------------------------------
        // ACCOUNT DELETION METHODS

        [Authorize(Policy = "PatientsOnly")]
        [HttpPost("request-deletion")]
        public async Task<IActionResult> RequestAccountDeletion()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User email not found.");
            }

            await _service.RequestAccountDeletionByEmailAsync(userEmail);
            return Ok("Confirmation email sent.");
        }

        [Authorize(Policy = "PatientsOnly")]
        [HttpPost("confirm-deletion")]
        public async Task<IActionResult> ConfirmAccountDeletion([FromQuery] string token)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User email not found.");
            }

            await _service.ConfirmAndDeleteAccountAsync(token, userEmail);
            return Ok("Account deletion confirmed.");
        }
    }
}