using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Infrastructure;
using DDDSample1.Domain.Events;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Infrastructure.OperationTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Backend.Tests.IntegrationTests
{
    public class OperationTypesIntegration_ServiceRepository
    {
        private readonly DbContextOptions<DDDSample1DbContext> _dbContextOptions;
        private readonly OperationTypeService _service;
        private readonly OperationTypeRepository _repository;

        public OperationTypesIntegration_ServiceRepository()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DDDSample1DbContext>()
                .UseInMemoryDatabase("DB_OTIT_SR")
                .Options;

            var dbContext = new DDDSample1DbContext(_dbContextOptions);
            var unitOfWork = new UnitOfWork(dbContext);

            _repository = new OperationTypeRepository(dbContext);
            _service = new OperationTypeService(
                unitOfWork,
                _repository,
                new OperationTypeCreatedEventHandler(Mock.Of<ILogger<OperationTypeCreatedEventHandler>>())
            );
        }

        private async Task CleanDatabase()
        {
            using var dbContext = new DDDSample1DbContext(_dbContextOptions);
            dbContext.RemoveRange(dbContext.OperationTypes);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task AddOperationType_ShouldAddOperationTypeToDatabase()
        {
            await CleanDatabase();

            var dto = new CreatingOperationTypeDto("Appendectomy", new List<string> { Guid.NewGuid().ToString() }, 60);

            var result = await _service.AddAsync(dto);

            var operationType = await _repository.GetByIdAsync(new OperationTypeId(result.Id));
            Assert.NotNull(operationType);
            Assert.Equal("Appendectomy", operationType.Name.Value);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOperationTypes()
        {
            await CleanDatabase();

            var dto1 = new CreatingOperationTypeDto("Appendectomy", new List<string> { Guid.NewGuid().ToString() }, 60);
            var dto2 = new CreatingOperationTypeDto("Cholecystectomy", new List<string> { Guid.NewGuid().ToString() }, 90);
            await _service.AddAsync(dto1);
            await _service.AddAsync(dto2);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOperationType()
        {
            await CleanDatabase();

            var dto = new CreatingOperationTypeDto("Appendectomy", new List<string> { Guid.NewGuid().ToString() }, 60);
            var addedOperationType = await _service.AddAsync(dto);

            var result = await _service.GetByIdAsync(addedOperationType.Id);

            Assert.NotNull(result);
            Assert.Equal("Appendectomy", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOperationType()
        {
            await CleanDatabase();

            var dto = new CreatingOperationTypeDto("Appendectomy", new List<string> { Guid.NewGuid().ToString() }, 60);
            var addedOperationType = await _service.AddAsync(dto);

            var updateDto = new CreatingOperationTypeDto("Cholecystectomy", new List<string> { Guid.NewGuid().ToString() }, 90);

            var result = await _service.UpdateAsync(addedOperationType.Id, updateDto);

            Assert.NotNull(result);
            Assert.Equal("Cholecystectomy", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveOperationTypeFromDatabase()
        {
            await CleanDatabase();

            var dto = new CreatingOperationTypeDto("Appendectomy", new List<string> { Guid.NewGuid().ToString() }, 60);
            var addedOperationType = await _service.AddAsync(dto);

            var result = await _service.DeleteAsync(new OperationTypeId(addedOperationType.Id));

            var operationType = await _repository.GetByIdAsync(new OperationTypeId(addedOperationType.Id));
            Assert.Null(operationType);
        }

        [Fact]
        public async Task DeactivateAsync_ShouldDeactivateOperationType()
        {
            await CleanDatabase();

            var dto = new CreatingOperationTypeDto("Appendectomy", new List<string> { Guid.NewGuid().ToString() }, 60);
            var addedOperationType = await _service.AddAsync(dto);

            var result = await _service.DeactivateAsync(addedOperationType.Id);

            var operationType = await _repository.GetByIdAsync(new OperationTypeId(addedOperationType.Id));
            Assert.False(operationType.isActive);
        }

        [Fact]
        public async Task SearchByName_ShouldReturnCorrectResults()
        {
            await CleanDatabase();

            var dto1 = new CreatingOperationTypeDto("Appendectomy", new List<string> { Guid.NewGuid().ToString() }, 60);
            var dto2 = new CreatingOperationTypeDto("Cholecystectomy", new List<string> { Guid.NewGuid().ToString() }, 90);
            await _service.AddAsync(dto1);
            await _service.AddAsync(dto2);

            var result = await _service.SearchAsync(null, "Appendectomy", null);

            Assert.Single(result);
            Assert.Equal("Appendectomy", result[0].Name);
        }

        [Fact]
        public async Task SearchById_ShouldReturnCorrectResult()
        {
            await CleanDatabase();

            var dto = new CreatingOperationTypeDto("Appendectomy", new List<string> { Guid.NewGuid().ToString() }, 60);
            var addedOperationType = await _service.AddAsync(dto);

            var result = await _service.SearchAsync(addedOperationType.Id, null, null);

            Assert.Single(result);
            Assert.Equal("Appendectomy", result[0].Name);
        }

        [Fact]
        public async Task SearchByIsActive_ShouldReturnCorrectResults()
        {
            await CleanDatabase();

            var dto1 = new CreatingOperationTypeDto("Appendectomy", new List<string> { Guid.NewGuid().ToString() }, 60);
            var dto2 = new CreatingOperationTypeDto("Cholecystectomy", new List<string> { Guid.NewGuid().ToString() }, 90);
            var addedOperationType1 = await _service.AddAsync(dto1);
            var addedOperationType2 = await _service.AddAsync(dto2);

            await _service.DeactivateAsync(addedOperationType1.Id);

            var result = await _service.SearchAsync(null, null, false);

            Assert.Single(result);
            Assert.Equal("Appendectomy", result[0].Name);
        }

        [Fact]
        public async Task UpdateNonExistentOperationType_ShouldReturnNull()
        {
            await CleanDatabase();

            var updateDto = new CreatingOperationTypeDto("Cholecystectomy", new List<string> { Guid.NewGuid().ToString() }, 90);

            var result = await _service.UpdateAsync(Guid.NewGuid(), updateDto);

            Assert.Null(result);
        }
    }
}