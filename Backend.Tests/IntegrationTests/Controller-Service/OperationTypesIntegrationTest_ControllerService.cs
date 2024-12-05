using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Infrastructure;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Events;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Infrastructure.OperationTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Backend.Tests.IntegrationTests
{
    public class OperationTypesIntegration_ControllerService
    {
        private readonly DbContextOptions<DDDSample1DbContext> _dbContextOptions;
        private readonly OperationTypeService _service;
        private readonly OperationTypesController _controller;

        public OperationTypesIntegration_ControllerService()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DDDSample1DbContext>()
                .UseInMemoryDatabase("DB_OTIT_CS")
                .Options;

            var dbContext = new DDDSample1DbContext(_dbContextOptions);
            var unitOfWork = new UnitOfWork(dbContext);

            _service = new OperationTypeService(
                unitOfWork,
                new OperationTypeRepository(dbContext),
                new OperationTypeCreatedEventHandler(Mock.Of<ILogger<OperationTypeCreatedEventHandler>>())
            );

            _controller = new OperationTypesController(_service, Mock.Of<ILogger<OperationTypesController>>());
        }
        
        private async Task CleanDatabase()
        {
            using var dbContext = new DDDSample1DbContext(_dbContextOptions);
            dbContext.RemoveRange(dbContext.OperationTypes);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task AddOperationType_ShouldReturnCreatedResult()
        {
            var dto = new CreatingOperationTypeDto("Test Operation Type", new List<string> { Guid.NewGuid().ToString() }, 60);

            var result = await _controller.AddOperationType(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<OperationTypeDTO>(createdResult.Value);
            Assert.Equal(dto.Name, returnValue.Name);
        }

        [Fact]
        public async Task GetOperationTypeById_ShouldReturnOperationType()
        {
            var dto = new CreatingOperationTypeDto("Test Operation Type", new List<string> { Guid.NewGuid().ToString() }, 60);
            var addedOperationType = await _service.AddAsync(dto);

            var result = await _controller.GetById(addedOperationType.Id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<OperationTypeDTO>(okResult.Value);
            Assert.Equal(addedOperationType.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetAllOperationTypes_ShouldReturnAllOperationTypes()
        {
            await CleanDatabase();
            
            var dto1 = new CreatingOperationTypeDto("Test Operation Type 1", new List<string> { Guid.NewGuid().ToString() }, 60);
            var dto2 = new CreatingOperationTypeDto("Test Operation Type 2", new List<string> { Guid.NewGuid().ToString() }, 90);
            await _service.AddAsync(dto1);
            await _service.AddAsync(dto2);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationTypeDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task UpdateOperationType_ShouldReturnOkResult()
        {
            var dto = new CreatingOperationTypeDto("Test Operation Type", new List<string> { Guid.NewGuid().ToString() }, 60);
            var addedOperationType = await _service.AddAsync(dto);

            var updateDto = new CreatingOperationTypeDto("Updated Operation Type", new List<string> { Guid.NewGuid().ToString() }, 90);

            var result = await _controller.UpdateOperationType(addedOperationType.Id, updateDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OperationTypeDTO>(okResult.Value);
            Assert.Equal(updateDto.Name, returnValue.Name);
        }

        [Fact]
        public async Task DeactivateOperationType_ShouldReturnOkResult()
        {
            var dto = new CreatingOperationTypeDto("Test Operation Type", new List<string> { Guid.NewGuid().ToString() }, 60);
            var addedOperationType = await _service.AddAsync(dto);

            var result = await _controller.DeactivateOperationType(addedOperationType.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("Message").GetValue(okResult.Value);
            Assert.Equal("Operation type successfully deactivated.", returnValue);
        }
    }
}