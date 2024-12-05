using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Domain.Events;
using DDDSample1.Domain.Events.Handlers;
using DDDNetCore.Domain.Email;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.StaffProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Backend.Tests.IntegrationTests
{
    public class StaffServiceIntegrationTests
    {
        private readonly DbContextOptions<DDDSample1DbContext> _dbContextOptions;
        private readonly StaffService _service;
        private readonly StaffRepository _repository;

        public StaffServiceIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DDDSample1DbContext>()
                .UseInMemoryDatabase("DB_StaffServiceIntegrationTests")
                .Options;

            var dbContext = new DDDSample1DbContext(_dbContextOptions);
            var unitOfWork = new UnitOfWork(dbContext);
            
            _repository = new StaffRepository(dbContext);
            _service = new StaffService(
                unitOfWork,
                _repository,
                Mock.Of<IEmailService>(),
                new StaffCreatedEventHandler(Mock.Of<ILogger<StaffCreatedEventHandler>>()),
                new StaffDeletedEventHandler(Mock.Of<ILogger<StaffDeletedEventHandler>>()),
                new StaffUpdatedEventHandler(Mock.Of<ILogger<StaffUpdatedEventHandler>>())
            );
        }

        private async Task CleanDatabase()
        {
            using var dbContext = new DDDSample1DbContext(_dbContextOptions);
            dbContext.RemoveRange(dbContext.Staffs);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task AddAsync_ShouldAddStaffToDatabase()
        {
            await CleanDatabase();

            // Arrange
            var dto = new CreatingStaffDto(
                "Alice",
                "Smith",
                new Role("Doctor").Value,
                new Specialization("Orthopaedist", "Doctor").Value,
                "alice.smith@example.com",
                "923456789"
            );

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            var staff = await _repository.GetByIdAsync(new StaffId(result.Id));
            Assert.NotNull(staff);
            Assert.Equal("Alice", staff.FirstName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllStaff()
        {
            await CleanDatabase();

            // Arrange
            var dto1 = new CreatingStaffDto(
                "Bob",
                "Brown",
                new Role("Nurse").Value,
                new Specialization("Circulating Nurse", "Nurse").Value,
                "bob.brown@example.com",
                "987654321"
            );

            var dto2 = new CreatingStaffDto(
                "Charlie",
                "Black",
                new Role("Doctor").Value,
                new Specialization("Anaesthetist", "Doctor").Value,
                "charlie.black@example.com",
                "956789123"
            );

            await _service.AddAsync(dto1);
            await _service.AddAsync(dto2);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnStaff()
        {
            await CleanDatabase();

            // Arrange
            var dto = new CreatingStaffDto(
                "Diana",
                "White",
                new Role("Doctor").Value,
                new Specialization("Anaesthetist", "Doctor").Value,
                "diana.white@example.com",
                "921654987"
            );

            var addedStaff = await _service.AddAsync(dto);

            // Act
            var result = await _service.GetByIdAsync(addedStaff.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Diana", result.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateStaff()
        {
            await CleanDatabase();

            // Arrange
            var dto = new CreatingStaffDto(
                "Edward",
                "Green",
                new Role("Doctor").Value,
                new Specialization("Orthopaedist", "Doctor").Value,
                "edward.green@example.com",
                "954321987"
            );

            var addedStaff = await _service.AddAsync(dto);

            var updateDto = new CreatingStaffDto(
                "Eddie",
                "Green",
                new Role("Nurse").Value,
                new Specialization("Nurse Anaesthetist", "Nurse").Value,
                "eddie.green@example.com",
                "954321987"
            );

            // Act
            var result = await _service.UpdateAsync(addedStaff.Id, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Eddie", result.FirstName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveStaffFromDatabase()
        {
            await CleanDatabase();

            // Arrange
            var dto = new CreatingStaffDto(
                "Fiona",
                "Blue",
                new Role("Nurse").Value,
                new Specialization("Medical Action Assistant", "Nurse").Value,
                "fiona.blue@example.com",
                "921987654"
            );

            var addedStaff = await _service.AddAsync(dto);

            // Act
            var result = await _service.DeleteAsync(addedStaff.Id); // Pass the Guid directly

            // Assert
            var staff = await _repository.GetByIdAsync(new StaffId(addedStaff.Id));
            Assert.Null(staff);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldDeactivateStaff()
        {
            await CleanDatabase();

            // Arrange
            var dto = new CreatingStaffDto(
                "George",
                "Red",
                new Role("Doctor").Value,
                new Specialization("Anaesthetist", "Doctor").Value,
                "george.red@example.com",
                "987123654"
            );

            var addedStaff = await _service.AddAsync(dto);

            // Act
            var result = await _service.DeactivateAsync(addedStaff.Id);

            // Assert
            var staff = await _repository.GetByIdAsync(new StaffId(addedStaff.Id));
            Assert.False(staff.IsActive);
        }
    }
}
