using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Domain.Shared;
using DDDNetCore.Domain.Email;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DDDSample1.Tests
{
    public class StaffServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IStaffRepository> _repoMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly StaffService _staffService;
  
        public StaffServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<IStaffRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _staffService = new StaffService(
                _unitOfWorkMock.Object,
                _repoMock.Object,
                _emailServiceMock.Object,
                new StaffCreatedEventHandler(Mock.Of<ILogger<StaffCreatedEventHandler>>()),
                new StaffDeletedEventHandler(Mock.Of<ILogger<StaffDeletedEventHandler>>()),
                new StaffUpdatedEventHandler(Mock.Of<ILogger<StaffUpdatedEventHandler>>())
            );
        }

        [Fact]
        public async Task SearchStaffProfilesAsync_ShouldReturnStaffDTOList_WhenStaffExists()
        {
            var id = Guid.NewGuid();
            var staffId = new StaffId(id);
            var staffList = new List<Staff>
            {
                new Staff("John", "Doe", "Doctor", "Orthopaedist", "john.doe@example.com", "923456789")
            };
            _repoMock.Setup(repo => repo.SearchStaffProfilesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool?>()))
                      .ReturnsAsync(staffList);

            // Name
            var resultName = await _staffService.SearchStaffProfilesAsync("John Doe", null, null, null, null, null);
            Assert.NotNull(resultName);
            Assert.Single(resultName);
            Assert.Equal("john.doe@example.com", resultName[0].Email);

            // Email
            var resultEmail = await _staffService.SearchStaffProfilesAsync(null, "john.doe@example.com", null, null, null, null);
            Assert.NotNull(resultEmail);
            Assert.Single(resultEmail);
            Assert.Equal("john.doe@example.com", resultEmail[0].Email);

            // Phone Number
            var resultPhoneNumber = await _staffService.SearchStaffProfilesAsync(null, null, "923456789", null, null, null);
            Assert.NotNull(resultPhoneNumber);
            Assert.Single(resultPhoneNumber);
            Assert.Equal("john.doe@example.com", resultPhoneNumber[0].Email);

            // Specialization
            var resultSpecialization = await _staffService.SearchStaffProfilesAsync(null, null, null, "Orthopaedist", null, null);
            Assert.NotNull(resultSpecialization);
            Assert.Single(resultSpecialization);
            Assert.Equal("john.doe@example.com", resultSpecialization[0].Email);

            // License Number
            var resultLicenseNumber = await _staffService.SearchStaffProfilesAsync(null, null, null, null, "D202400001", null);
            Assert.NotNull(resultLicenseNumber);
            Assert.Single(resultLicenseNumber);
            Assert.Equal("john.doe@example.com", resultLicenseNumber[0].Email);

            // Is Active
            var resultActive = await _staffService.SearchStaffProfilesAsync(null, null, null, null, null, true);
            Assert.NotNull(resultActive);
            Assert.Single(resultActive);
            Assert.Equal("john.doe@example.com", resultActive[0].Email);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnStaffDTO_WhenStaffExists()
        {
            var id = Guid.NewGuid();
            var staffId = new StaffId(id);
            var staff = new Staff("Jane", "Doe", "Nurse", "Instrumenting Nurse", "jane.doe@example.com", "987654321");
            _repoMock.Setup(repo => repo.GetByIdAsync(staffId))
                      .ReturnsAsync(staff); 

            // Act
            var result = await _staffService.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNewStaffDTO_WhenAddingStaff()
        {
            // Arrange
            var dto = new CreatingStaffDto(
                "Alice",
                "Smith",
                new Role("Other").Value,
                new Specialization("X-ray Technician", "Other").Value,
                "alice.smith@example.com",
                "923123123"
            );
            var staff = new Staff(dto.FirstName, dto.LastName, dto.Role, dto.Specialization, dto.Email, dto.PhoneNumber);
            _repoMock.Setup(repo => repo.AddAsync(It.IsAny<Staff>())).ReturnsAsync(staff); 
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1); 

            // Act
            var result = await _staffService.AddAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("alice.smith@example.com", result.Email);
        }

        /*[Fact]
        public async Task UpdateAsync_ShouldSendEmail_WhenUpdatingStaff()
        {
            // Arrange
            var id = Guid.NewGuid();
            var staffId = new StaffId(id);
            var existingStaff = new Staff("Bob", "Brown", "Nurse", "Circulating Nurse", "bob.brown@example.com", "321321321");
            var updateDto = new CreatingStaffDto(
                "Robert",
                "Brown",
                new Role("Nurse").Value,
                new Specialization("Medical Action Assistant", "Nurse").Value,
                "luna.sp.1737@gmail.com",
                "321321321"
            );

            _repoMock.Setup(repo => repo.GetByIdAsync(staffId)).ReturnsAsync(existingStaff);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _staffService.UpdateAsync(id, updateDto);

            // Assert
            _emailServiceMock.Verify(es => es.SendEmailAsync(existingStaff.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.Equal("Medical Action Assistant", result.Specialization);
        }*/

        [Fact]
        public async Task DeactivateAsync_ShouldReturnTrue_WhenStaffExists()
        {
            var id = Guid.NewGuid();
            var staffId = new StaffId(id);
            var staff = new Staff("Eve", "White", "Other", "X-ray Technician", "eve.white@example.com", "956456456");
            _repoMock.Setup(repo => repo.GetByIdAsync(staffId)).ReturnsAsync(staff); 
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1); 

            // Act
            var result = await _staffService.DeactivateAsync(id);

            // Assert
            Assert.True(result);
            Assert.False(staff.IsActive);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnStaffDTO_WhenDeletingStaff()
        {
            var id = Guid.NewGuid();
            var staffId = new StaffId(id);
            var staff = new Staff("Charlie", "Black", "Other", "X-ray Technician", "charlie.black@example.com", "954654654");
            _repoMock.Setup(repo => repo.GetByIdAsync(staffId)).ReturnsAsync(staff); 
            _repoMock.Setup(repo => repo.Remove(It.IsAny<Staff>())).Verifiable();
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1); 


           
            var result = await _staffService.DeactivateAsync(id);

        
             Assert.True(result);
            Assert.False(staff.IsActive);
        }
    }
}
