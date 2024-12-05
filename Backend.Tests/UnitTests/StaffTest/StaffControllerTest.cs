using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Domain.Shared;
using DDDSample1.Infrastructure;
using DDDNetCore.Controllers;
using DDDNetCore.Domain.Email;
using DDDSample1.Domain.Events;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Infrastructure.StaffProfile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Backend.Tests
{
    public class StaffControllerTests
    {
        private readonly StaffService _service;
        private readonly StaffController _controller;

        // Mocks
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IStaffRepository> _repoMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        public StaffControllerTests()
        {
            // Initialize mocks
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<IStaffRepository>();
            _emailServiceMock = new Mock<IEmailService>();

            // Create the service with mocks
            _service = new StaffService(
                _unitOfWorkMock.Object,
                _repoMock.Object,
                _emailServiceMock.Object,
                new StaffCreatedEventHandler(Mock.Of<ILogger<StaffCreatedEventHandler>>()),
                new StaffDeletedEventHandler(Mock.Of<ILogger<StaffDeletedEventHandler>>()),
                new StaffUpdatedEventHandler(Mock.Of<ILogger<StaffUpdatedEventHandler>>())
            );

            // Initialize the controller with the service
            _controller = new StaffController(_service);
        }

    [Fact]
public async Task AddStaff_ShouldReturnCreatedStaff_WhenValidDto()
{
    // Arrange
    var id = Guid.NewGuid();
    var staffId = new StaffId(id);
    var dto = new CreatingStaffDto("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "923456789");
    
    // Simulating that the email and phone number do not exist
    _repoMock.Setup(s => s.GetByEmailAsync(dto.Email)).ReturnsAsync((Staff)null);
    _repoMock.Setup(s => s.GetByPhoneNumberAsync(dto.PhoneNumber)).ReturnsAsync((Staff)null);

    // Setup to add the new staff
    var createdStaff = new Staff(dto.FirstName, dto.LastName, dto.Role, dto.Specialization, dto.Email, dto.PhoneNumber);
    _repoMock.Setup(s => s.AddAsync(It.IsAny<Staff>())).ReturnsAsync(createdStaff);

    // Act
    var result = await _controller.AddStaff(dto);

    // Assert
    var actionResult = Assert.IsType<CreatedAtActionResult>(result);
    var staffDto = Assert.IsType<StaffDTO>(actionResult.Value);
    Assert.Equal(createdStaff.Name, staffDto.Name);
    Assert.Equal(createdStaff.Email, staffDto.Email);
}


        [Fact]
        public async Task AddStaff_ShouldReturnConflict_WhenEmailAlreadyExists()
        {
             var id = Guid.NewGuid();
            var staffId = new StaffId(id);
            var dto = new CreatingStaffDto("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "923456789");

            var existingStaff = new Staff("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "923456789");
            _repoMock.Setup(s => s.GetByEmailAsync(dto.Email)).ReturnsAsync(existingStaff);

            // Act
            var result = await _controller.AddStaff(dto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("The email is already in use by another staff member.", conflictResult.Value.GetType().GetProperty("Message").GetValue(conflictResult.Value));
        }

        [Fact]
        public async Task AddStaff_ShouldReturnConflict_WhenPhoneNumberAlreadyExists()
        {
            var id = Guid.NewGuid();
            var staffId = new StaffId(id);
            var dto = new CreatingStaffDto("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "923456789");
            var existingStaff = new Staff("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "923456789");
            _repoMock.Setup(s => s.GetByEmailAsync(dto.Email)).ReturnsAsync((Staff)null);
            _repoMock.Setup(s => s.GetByPhoneNumberAsync(dto.PhoneNumber)).ReturnsAsync(existingStaff);

            // Act
            var result = await _controller.AddStaff(dto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("The phone number is already in use by another staff member.", conflictResult.Value.GetType().GetProperty("Message").GetValue(conflictResult.Value));
        }

       /* [Fact]
public async Task EditStaffProfile_ShouldReturnOk_WhenStaffIsUpdated()
{
    // Arrange
    var id = Guid.NewGuid();
    var staffId = new StaffId(id);
    var dto = new CreatingStaffDto("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "123456789");
    var updatedStaff = new Staff(dto.FirstName, "Alonso", dto.Role, dto.Specialization, dto.Email, dto.PhoneNumber);
    _repoMock.Setup(repo => repo.GetByIdAsync(staffId)).ReturnsAsync(updatedStaff);

    // Act
    var result = await _controller.EditStaffProfile(id, dto);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var staffDto = Assert.IsType<StaffDTO>(okResult.Value);
    
    // Ensure that the updated name is asserted correctly
    Assert.Equal(updatedStaff.Name, staffDto.Name);
}*/


        [Fact]
        public async Task EditStaffProfile_ShouldReturnNotFound_WhenStaffDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var staffId = new StaffId(id);
            var dto = new CreatingStaffDto("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "923456789");
            

            // Act
            var result = await _controller.EditStaffProfile(id, dto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Staff member not found or update failed.", notFoundResult.Value.GetType().GetProperty("Message").GetValue(notFoundResult.Value));
        }

        [Fact]
        public async Task DeactivateStaff_ShouldReturnOk_WhenStaffIsDeactivated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var staffId = new StaffId(id);
            var staff = new Staff("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "923456789");
            _repoMock.Setup(s => s.GetByIdAsync(staffId)).ReturnsAsync(staff);
           

            // Act
            var result = await _controller.DeactivateStaff(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Staff profile successfully deactivated.", okResult.Value.GetType().GetProperty("Message").GetValue(okResult.Value));
        }

       [Fact]
public async Task DeactivateStaff_ShouldReturnNotFound_WhenStaffDoesNotExist()
{
    // Arrange
    var id = Guid.NewGuid();
    var staffId = new StaffId(id);
    
    
    _repoMock.Setup(s => s.GetByIdAsync(staffId)).ReturnsAsync((Staff)null);

    // Act
    var result = await _controller.DeactivateStaff(id);

    // Assert
    var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
    Assert.Equal("Staff member not found.", notFoundResult.Value.GetType().GetProperty("Message").GetValue(notFoundResult.Value));
}


       

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenStaffExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var staffId = new StaffId(id);
            var staff = new Staff("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "923456789");
            _repoMock.Setup(s => s.GetByIdAsync(staffId)).ReturnsAsync(staff);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var staffDto = Assert.IsType<StaffDTO>(okResult.Value);
            Assert.Equal(staff.Name, staffDto.Name);
        }

       [Fact]
public async Task GetById_ShouldReturnNotFound_WhenStaffDoesNotExist()
{
    // Arrange
    var id = Guid.NewGuid();
    var staffId = new StaffId(id);
    _repoMock.Setup(s => s.GetByIdAsync(staffId)).ReturnsAsync((Staff)null);

    // Act
    var result = await _controller.GetById(id);

    
    var notFoundResult = Assert.IsType<NotFoundResult>(result);
 }

    }
}
