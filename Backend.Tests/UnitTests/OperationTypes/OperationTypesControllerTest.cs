using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.StaffProfile;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Backend.Tests
{
    public class OperationTypeServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IOperationTypeRepository> _repoMock;
        private readonly OperationTypeCreatedEventHandler _eventHandler;
        private readonly OperationTypeService _service;

        public OperationTypeServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<IOperationTypeRepository>();
            
            // Create a mock logger
            var loggerMock = new Mock<ILogger<OperationTypeCreatedEventHandler>>();
            
            // Instantiate the event handler with the mock logger
            _eventHandler = new OperationTypeCreatedEventHandler(loggerMock.Object);
            
            // Use the real event handler in the service
            _service = new OperationTypeService(_unitOfWorkMock.Object, _repoMock.Object, _eventHandler);
        }

        [Fact]
        public async Task AddOperationType_ShouldReturnCreatedOperationType()
        {
            // Arrange
            var dto = new CreatingOperationTypeDto("Appendectomy", 
                        new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }, 60);

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Appendectomy", result.Name);
            Assert.True(result.isActive);
        }


        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOperationTypes()
        {
            // Arrange
            var operationTypes = new List<OperationType>
            {
                new OperationType(new Name("Appendectomy"), new List<StaffId> { new StaffId(Guid.NewGuid()), new StaffId(Guid.NewGuid()) }, 60),
                new OperationType(new Name("Cholecystectomy"), new List<StaffId> { new StaffId(Guid.NewGuid()), new StaffId(Guid.NewGuid()) }, 90)
            };
            _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(operationTypes);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOperationType()
        {
            // Arrange
            var id = Guid.NewGuid();
            var operationType = new OperationType(new Name("Appendectomy"), new List<StaffId> { new StaffId(Guid.NewGuid()), new StaffId(Guid.NewGuid()) }, 60);
            _repoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationTypeId>())).ReturnsAsync(operationType);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Appendectomy", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedOperationType()
        {
            // Arrange
            var id = Guid.NewGuid();
            var operationType = new OperationType(new Name("Appendectomy"), new List<StaffId> { new StaffId(Guid.NewGuid()), new StaffId(Guid.NewGuid()) }, 60);
            _repoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationTypeId>())).ReturnsAsync(operationType);
            
            // Use valid Guid strings for RequiredStaff
            var dto = new CreatingOperationTypeDto("Cholecystectomy", new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() }, 90);

            // Act
            var result = await _service.UpdateAsync(id, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cholecystectomy", result.Name);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldReturnTrue()
        {
            // Arrange
            var id = Guid.NewGuid();
            var operationType = new OperationType(new Name("Appendectomy"), new List<StaffId> { new StaffId(Guid.NewGuid()), new StaffId(Guid.NewGuid()) }, 60);
            _repoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationTypeId>())).ReturnsAsync(operationType);

            // Act
            var result = await _service.DeactivateAsync(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnDeletedOperationType()
        {
            // Arrange
            var id = new OperationTypeId(Guid.NewGuid());
            var operationType = new OperationType(new Name("Appendectomy"), new List<StaffId> { new StaffId(Guid.NewGuid()), new StaffId(Guid.NewGuid()) }, 60);
            _repoMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(operationType);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Appendectomy", result.Name);
        }
    }
}