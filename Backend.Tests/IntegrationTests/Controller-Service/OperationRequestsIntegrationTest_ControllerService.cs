using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Domain.Shared;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.OperationRequests;
using DDDSample1.Infrastructure.OperationTypes;
using DDDSample1.Infrastructure.Patients;
using DDDSample1.Infrastructure.StaffProfile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Backend.Tests.IntegrationTests
{
    public class OperationRequestsIntegration_ControllerService
    {
        private readonly DbContextOptions<DDDSample1DbContext> _dbContextOptions;
        private readonly OperationRequestService _service;
        private readonly OperationRequestsController _controller;

        public OperationRequestsIntegration_ControllerService()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DDDSample1DbContext>()
                .UseInMemoryDatabase("DB_ORIT_CS")
                .Options;

            var dbContext = new DDDSample1DbContext(_dbContextOptions);
            var unitOfWork = new UnitOfWork(dbContext);

            _service = new OperationRequestService(
                unitOfWork,
                new OperationRequestRepository(dbContext),
                new OperationTypeRepository(dbContext),
                new PatientProfileRepository(dbContext),
                new StaffRepository(dbContext),
                new OperationRequestCreatedEventHandler(Mock.Of<ILogger<OperationRequestCreatedEventHandler>>()),
                new OperationRequestDeletedEventHandler(Mock.Of<ILogger<OperationRequestDeletedEventHandler>>()),
                new OperationRequestUpdatedEventHandler(Mock.Of<ILogger<OperationRequestUpdatedEventHandler>>())
            );

            _controller = new OperationRequestsController(_service);
        }

        private async Task CleanDatabase()
        {
            using var dbContext = new DDDSample1DbContext(_dbContextOptions);
            dbContext.RemoveRange(dbContext.OperationRequests);
            dbContext.RemoveRange(dbContext.OperationTypes);
            dbContext.RemoveRange(dbContext.PatientProfiles);
            dbContext.RemoveRange(dbContext.Staffs);
            await dbContext.SaveChangesAsync();
        }

        private async Task<(Guid operationTypeId, Guid patientId, Guid doctorId)> AddTestData()
        {
            using var dbContext = new DDDSample1DbContext(_dbContextOptions);
            var staffId = new StaffId(Guid.NewGuid());
            var requiredStaff = new List<StaffId> { staffId };

            var operationType = new OperationType(new Name("Test Operation"), requiredStaff, 60);
            var patientProfile = new PatientProfile(new FirstName("John"), new LastName("Doe"), new FullName("John Doe"), DateTime.Now.AddYears(-30), new Email("john.doe@example.com"), new ContactInformation(966666666));
            var doctorProfile = new Staff("Jane", "Doe", "Doctor", "Orthopaedist", "jane.doe@example.com", "966666667");

            dbContext.AddRange(operationType, patientProfile, doctorProfile);
            await dbContext.SaveChangesAsync();

            return (operationType.Id.AsGuid(), patientProfile.Id.AsGuid(), doctorProfile.Id.AsGuid());
        }

        [Fact]
        public async Task AddOperationRequest_ShouldReturnCreatedResult()
        {
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var dto = new CreatingOperationRequestDto(patientId, doctorId, operationTypeId, Priority.Emergency, DateTime.UtcNow.AddDays(1));

            var result = await _controller.AddOperationRequest(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<OperationRequestDto>(createdResult.Value.GetType().GetProperty("result").GetValue(createdResult.Value));
            Assert.Equal(dto.PatientId, returnValue.PatientId);
        }

        [Fact]
        public async Task UpdateOperationRequest_ShouldReturnOkResult()
        {
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var requestId = Guid.NewGuid();

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), Priority.Emergency, DateTime.UtcNow.AddDays(1));
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
                requestId = operationRequest.Id.AsGuid();
            }

            var dto = new UpdatingOperationRequestDto
            {
                Id = requestId,
                Priority = Priority.Urgent,
                SuggestedDeadline = DateTime.UtcNow.AddDays(2)
            };

            var result = await _controller.UpdateOperationRequest(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OperationRequestDto>(okResult.Value.GetType().GetProperty("result").GetValue(okResult.Value));
            Assert.Equal(dto.Priority, returnValue.Priority);
            Assert.Equal(dto.SuggestedDeadline, returnValue.SuggestedDeadline);
        }

        [Fact]
        public async Task DeleteOperationRequest_ShouldReturnOkResult()
        {
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var requestId = Guid.NewGuid();

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), Priority.Emergency, DateTime.UtcNow.AddDays(1));
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
                requestId = operationRequest.Id.AsGuid();
            }

            var result = await _controller.DeleteOperationRequest(requestId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value);
            Assert.Equal("Operation request successfully deleted.", returnValue);
        }

        [Fact]
        public async Task GetOperationRequestById_ShouldReturnOperationRequest()
        {
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);

            OperationRequest operationRequest;
            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.GetById(operationRequest.Id.AsGuid());

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<OperationRequestDto>(okResult.Value.GetType().GetProperty("operationRequest").GetValue(okResult.Value));
            Assert.Equal(operationRequest.Id.AsGuid(), returnValue.Id);
        }

        [Fact]
        public async Task GetAllOperationRequests_ShouldReturnAllOperationRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest1 = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                var operationRequest2 = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate.AddDays(1));
                dbContext.OperationRequests.AddRange(operationRequest1, operationRequest2);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Equal(2, returnValue.Count);
        }
        
        [Fact]
        public async Task Search_ByDoctorId_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search("Jane Doe", null, null, null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(doctorId, returnValue.First().DoctorId);
        }

        [Fact]
        public async Task Search_ByPriority_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search(null, priority, null, null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(priority, returnValue.First().Priority);
        }

        [Fact]
        public async Task Search_ByPatientName_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search(null, null, null, "John Doe", null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(patientId, returnValue.First().PatientId);
        }

        [Fact]
        public async Task Search_ByOperationTypeName_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search(null, null, "Test Operation", null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(operationTypeId, returnValue.First().OperationTypeId);
        }
        
        [Fact]
        public async Task Search_ByExpectedDueDate_ShouldReturnMatchingRequests()
        {
            await CleanDatabase(); 
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search(null, null, null, null, expectedDueDate, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(expectedDueDate, returnValue.First().SuggestedDeadline);
        }

        [Fact]
        public async Task Search_ByRequestDate_ShouldReturnMatchingRequests()
        {
            await CleanDatabase(); 
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);
            var requestDate = DateTime.UtcNow.Date;

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
                operationRequest.GetType().GetProperty("RequestDate").SetValue(operationRequest, requestDate);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search(null, null, null, null, null, requestDate);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(requestDate, returnValue.First().RequestDate.Date);
        }
        
        [Fact]
        public async Task Search_ByDoctorIdAndPriority_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search("Jane Doe", priority, null, null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(doctorId, returnValue.First().DoctorId);
            Assert.Equal(priority, returnValue.First().Priority);
        }

        [Fact]
        public async Task Search_ByOperationTypeNameAndPatientName_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search(null, null, "Test Operation", "John Doe", null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(operationTypeId, returnValue.First().OperationTypeId);
            Assert.Equal(patientId, returnValue.First().PatientId);
        }

        [Fact]
        public async Task Search_ByExpectedDueDateAndRequestDate_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);
            var requestDate = DateTime.UtcNow.Date;

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
                operationRequest.GetType().GetProperty("RequestDate").SetValue(operationRequest, requestDate);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search(null, null, null, null, expectedDueDate, requestDate);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(expectedDueDate, returnValue.First().SuggestedDeadline);
            Assert.Equal(requestDate, returnValue.First().RequestDate.Date);
        }
        
        [Fact]
        public async Task Search_ByDoctorIdPriorityAndExpectedDueDate_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search("Jane Doe", priority, null, null, expectedDueDate, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(doctorId, returnValue.First().DoctorId);
            Assert.Equal(priority, returnValue.First().Priority);
            Assert.Equal(expectedDueDate, returnValue.First().SuggestedDeadline);
        }

        [Fact]
        public async Task Search_ByOperationTypeNamePatientNameAndRequestDate_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);
            var requestDate = DateTime.UtcNow.Date;

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
                operationRequest.GetType().GetProperty("RequestDate").SetValue(operationRequest, requestDate);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search(null, null, "Test Operation", "John Doe", null, requestDate);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(operationTypeId, returnValue.First().OperationTypeId);
            Assert.Equal(patientId, returnValue.First().PatientId);
            Assert.Equal(requestDate, returnValue.First().RequestDate.Date);
        }

        [Fact]
        public async Task Search_ByDoctorIdPriorityExpectedDueDateAndRequestDate_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);
            var requestDate = DateTime.UtcNow.Date;

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
                operationRequest.GetType().GetProperty("RequestDate").SetValue(operationRequest, requestDate);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search("Jane Doe", priority, null, null, expectedDueDate, requestDate);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(doctorId, returnValue.First().DoctorId);
            Assert.Equal(priority, returnValue.First().Priority);
            Assert.Equal(expectedDueDate, returnValue.First().SuggestedDeadline);
            Assert.Equal(requestDate, returnValue.First().RequestDate.Date);
        }

        [Fact]
        public async Task Search_ByOperationTypeNamePatientNameExpectedDueDateAndRequestDate_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);
            var requestDate = DateTime.UtcNow.Date;

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
                operationRequest.GetType().GetProperty("RequestDate").SetValue(operationRequest, requestDate);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search(null, null, "Test Operation", "John Doe", expectedDueDate, requestDate);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(operationTypeId, returnValue.First().OperationTypeId);
            Assert.Equal(patientId, returnValue.First().PatientId);
            Assert.Equal(expectedDueDate, returnValue.First().SuggestedDeadline);
            Assert.Equal(requestDate, returnValue.First().RequestDate.Date);
        }
        
        [Fact]
        public async Task Search_ByAllCriteria_ShouldReturnMatchingRequests()
        {
            await CleanDatabase();
            var (operationTypeId, patientId, doctorId) = await AddTestData();
            var priority = Priority.Urgent;
            var expectedDueDate = DateTime.UtcNow.AddDays(1);
            var requestDate = DateTime.UtcNow.Date;

            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                var operationRequest = new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate);
                dbContext.OperationRequests.Add(operationRequest);
                await dbContext.SaveChangesAsync();
                operationRequest.GetType().GetProperty("RequestDate").SetValue(operationRequest, requestDate);
                await dbContext.SaveChangesAsync();
            }

            var result = await _controller.Search("Jane Doe", priority, "Test Operation", "John Doe", expectedDueDate, requestDate);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
            Assert.Equal(doctorId, returnValue.First().DoctorId);
            Assert.Equal(priority, returnValue.First().Priority);
            Assert.Equal(operationTypeId, returnValue.First().OperationTypeId);
            Assert.Equal(patientId, returnValue.First().PatientId);
            Assert.Equal(expectedDueDate, returnValue.First().SuggestedDeadline);
            Assert.Equal(requestDate, returnValue.First().RequestDate.Date);
        }
    }
}