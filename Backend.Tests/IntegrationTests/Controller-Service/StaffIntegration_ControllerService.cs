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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Backend.Tests.IntegrationTests
{
    public class StaffIntegration_ControllerService
    {
        private readonly DbContextOptions<DDDSample1DbContext> _dbContextOptions;
        private readonly StaffService _service;
        private readonly StaffController _controller;

        public StaffIntegration_ControllerService()
        {
            // Configuração do banco de dados em memória
            _dbContextOptions = new DbContextOptionsBuilder<DDDSample1DbContext>()
                .UseInMemoryDatabase("DB_StaffIntegration_CS")
                .Options;

            var dbContext = new DDDSample1DbContext(_dbContextOptions);
            var unitOfWork = new UnitOfWork(dbContext);

            // Inicialização do serviço com um mock do logger
            _service = new StaffService(
                unitOfWork,
                new StaffRepository(dbContext),
                Mock.Of<IEmailService>(),
                new StaffCreatedEventHandler(Mock.Of<ILogger<StaffCreatedEventHandler>>()),
                new StaffDeletedEventHandler(Mock.Of<ILogger<StaffDeletedEventHandler>>()),
                new StaffUpdatedEventHandler(Mock.Of<ILogger<StaffUpdatedEventHandler>>())
            );

            // Inicialização do controller com um mock do logger
            _controller = new StaffController(_service);
        }

        private async Task CleanDatabase()
        {
            using var dbContext = new DDDSample1DbContext(_dbContextOptions);
            dbContext.RemoveRange(dbContext.Staffs);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task AddStaff_ShouldReturnCreatedResult()
        {
            var dto = new CreatingStaffDto(
                "Bob",
                "Brown",
                new Role("Nurse").Value,
                new Specialization("Nurse Anaesthetist", "Nurse").Value,
                "bob.brown@example.com",
                "987654321"
            );

            var result = await _controller.AddStaff(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<StaffDTO>(createdResult.Value);
            Assert.Equal(dto.Email, returnValue.Email);
        }

 /*       [Fact]
public async Task GetAllStaff_ShouldReturnAllStaff()
{
    await CleanDatabase();

    var dto1 = new CreatingStaffDto(
        "Alice",
        "Smith",
        new Role("Doctor").Value,
        new Specialization("Anaesthetist", "Doctor").Value,
        "alice.smith@example.com",
        "123456789"
    );

    var dto2 = new CreatingStaffDto(
        "Charlie",
        "Brown",
        new Role("Doctor").Value,
        new Specialization("Orthopaedist", "Doctor").Value,
        "charlie.brown@example.com",
        "987654321"
    );

    await _service.AddAsync(dto1);
    await _service.AddAsync(dto2);

    var result = await _controller.GetAllStaff();

    var okResult = Assert.IsType<ActionResult<List<StaffDTO>>>(result);
    var returnValue = Assert.IsType<List<StaffDTO>>(okResult.Value); 
    Assert.Equal(2, returnValue.Count); 
}*/



        [Fact]
        public async Task GetById_ShouldReturnStaff()
        {
            var dto = new CreatingStaffDto(
                "Eve",
                "Johnson",
                new Role("Nurse").Value,
                new Specialization("Medical Action Assistant", "Nurse").Value,
                "eve.johnson@example.com",
                "956789123"
            );

            var addedStaff = await _service.AddAsync(dto);
            var result = await _controller.GetById(addedStaff.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<StaffDTO>(okResult.Value); 
            Assert.Equal(addedStaff.Id, returnValue.Id);
        }

        [Fact]
        public async Task DeactivateStaff_ShouldReturnOkResult()
        {
            var dto = new CreatingStaffDto("Test Staff", "User", "Doctor", "Anaesthetist", "test@example.com", "923456789");
            var addedStaff = await _service.AddAsync(dto);

            var result = await _controller.DeactivateStaff(addedStaff.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value; 
            var message = returnValue.GetType().GetProperty("Message").GetValue(returnValue);
    
            Assert.Equal("Staff profile successfully deactivated.", message);
        }
    }
}
