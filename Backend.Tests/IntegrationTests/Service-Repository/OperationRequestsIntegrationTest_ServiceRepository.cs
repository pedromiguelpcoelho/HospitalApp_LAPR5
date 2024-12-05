using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.OperationRequests;
using DDDSample1.Infrastructure.OperationTypes;
using DDDSample1.Infrastructure.Patients;
using DDDSample1.Infrastructure.StaffProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class OperationRequestsIntegration
{
    private readonly DbContextOptions<DDDSample1DbContext> _dbContextOptions;
    private readonly OperationRequestService _service;

    public OperationRequestsIntegration()
    {
        _dbContextOptions = new DbContextOptionsBuilder<DDDSample1DbContext>()
            .UseInMemoryDatabase("DB_ORIT_SR")
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
    public async Task AddOperationRequest_ShouldAddToDatabase()
    {
        var (operationTypeId, patientId, doctorId) = await AddTestData();
        var dto = new CreatingOperationRequestDto(patientId, doctorId, operationTypeId, Priority.Emergency, DateTime.UtcNow.AddDays(1));

        var result = await _service.AddAsync(dto);

        using var dbContext = new DDDSample1DbContext(_dbContextOptions);
        var operationRequest = await dbContext.OperationRequests.FindAsync(new OperationRequestId(result.Id));
        Assert.NotNull(operationRequest);
        Assert.Equal(dto.Priority, operationRequest.Priority);
        Assert.Equal(dto.SuggestedDeadline, operationRequest.SuggestedDeadline);
        Assert.Equal(dto.PatientId, operationRequest.PatientId.AsGuid());
        Assert.Equal(dto.DoctorId, operationRequest.DoctorId.AsGuid());
        Assert.Equal(dto.OperationTypeId, operationRequest.OperationTypeId.AsGuid());
    }

    [Fact]
    public async Task UpdateOperationRequest_ShouldUpdateInDatabase()
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

        var result = await _service.UpdateAsync(dto);

        using var dbContextVerify = new DDDSample1DbContext(_dbContextOptions);
        var operationRequestVerify = await dbContextVerify.OperationRequests.FindAsync(new OperationRequestId(result.Id));
        Assert.NotNull(operationRequestVerify);
        Assert.Equal(dto.Priority, operationRequestVerify.Priority);
        Assert.Equal(dto.SuggestedDeadline, operationRequestVerify.SuggestedDeadline);
        Assert.Equal(patientId, operationRequestVerify.PatientId.AsGuid());
        Assert.Equal(doctorId, operationRequestVerify.DoctorId.AsGuid());
        Assert.Equal(operationTypeId, operationRequestVerify.OperationTypeId.AsGuid());
    }

    [Fact]
    public async Task DeleteOperationRequest_ShouldRemoveFromDatabase()
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

        var isDeleted = await _service.DeleteAsync(requestId);

        Assert.True(isDeleted);
        using var dbContextVerify = new DDDSample1DbContext(_dbContextOptions);
        var operationRequestVerify = await dbContextVerify.OperationRequests.FindAsync(new OperationRequestId(requestId));
        Assert.Null(operationRequestVerify);
    }

    [Fact]
    public async Task SearchOperationRequests_ByDoctorId_ShouldReturnMatchingRequests()
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

        var result = await _service.SearchAsync("Jane Doe", null, null, null, null, null);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(doctorId, result.First().DoctorId);
    }

    [Fact]
    public async Task SearchOperationRequests_ByPriority_ShouldReturnMatchingRequests()
    {
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

        var result = await _service.SearchAsync(null, priority, null, null, null, null);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(priority, result.First().Priority);
    }

    [Fact]
    public async Task SearchOperationRequests_ByOperationTypeName_ShouldReturnMatchingRequests()
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

        var result = await _service.SearchAsync(null, null, "Test Operation", null, null, null);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(operationTypeId, result.First().OperationTypeId);
    }

    [Fact]
    public async Task SearchOperationRequests_ByPatientName_ShouldReturnMatchingRequests()
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
        }

        var result = await _service.SearchAsync(null, null, null, "John Doe", null, null);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(patientId, result.First().PatientId);
    }

    [Fact]
    public async Task SearchOperationRequests_ByAllCriteria_ShouldReturnMatchingRequests()
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

        var result = await _service.SearchAsync("Jane Doe", priority, "Test Operation", "John Doe", expectedDueDate, requestDate);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(doctorId, result.First().DoctorId);
        Assert.Equal(priority, result.First().Priority);
        Assert.Equal(operationTypeId, result.First().OperationTypeId);
        Assert.Equal(patientId, result.First().PatientId);
        Assert.Equal(expectedDueDate, result.First().SuggestedDeadline);
        Assert.Equal(requestDate, result.First().RequestDate.Date);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnOperationRequest()
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

        var result = await _service.GetByIdAsync(operationRequest.Id.AsGuid());

        Assert.NotNull(result);
        Assert.Equal(operationRequest.Id.AsGuid(), result.Id);
        Assert.Equal(doctorId, result.DoctorId);
        Assert.Equal(priority, result.Priority);
        Assert.Equal(operationTypeId, result.OperationTypeId);
        Assert.Equal(patientId, result.PatientId);
        Assert.Equal(expectedDueDate, result.SuggestedDeadline);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllOperationRequests()
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

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}