using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using DDDNetCore.Controllers;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Events.Handlers;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Backend.Tests
{
    public class OperationRequestsControllerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IOperationRequestRepository> _repoOrMock;
        private readonly Mock<IOperationTypeRepository> _repoOtMock;
        private readonly Mock<IPatientProfileRepository> _repoPMock;
        private readonly Mock<IStaffRepository> _repoSMock;
        private readonly Mock<OperationRequestService> _serviceMock;
        private readonly OperationRequestsController _controller;

        public OperationRequestsControllerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repoOrMock = new Mock<IOperationRequestRepository>();
            _repoOtMock = new Mock<IOperationTypeRepository>();
            _repoPMock = new Mock<IPatientProfileRepository>();
            _repoSMock = new Mock<IStaffRepository>();

            var createdEventHandler = new OperationRequestCreatedEventHandler(Mock.Of<ILogger<OperationRequestCreatedEventHandler>>());
            var deletedEventHandler = new OperationRequestDeletedEventHandler(Mock.Of<ILogger<OperationRequestDeletedEventHandler>>());
            var updatedEventHandler = new OperationRequestUpdatedEventHandler(Mock.Of<ILogger<OperationRequestUpdatedEventHandler>>());

            _serviceMock = new Mock<OperationRequestService>(
                _unitOfWorkMock.Object,
                _repoOrMock.Object,
                _repoOtMock.Object,
                _repoPMock.Object,
                _repoSMock.Object,
                createdEventHandler,
                deletedEventHandler,
                updatedEventHandler
            );
            _controller = new OperationRequestsController(_serviceMock.Object);
        }

        private void SetupTestData()
        {
            var staffIdMock = new Mock<StaffId>(Guid.NewGuid());
            var requiredStaff = new List<StaffId> { staffIdMock.Object };

            var operationTypeMock = new Mock<OperationType>(new Name("Appendectomy"), requiredStaff, 60);
            var patientProfileMock = new Mock<PatientProfile>(new FirstName("Pedro"), new LastName("Coelho"), new FullName("Pedro Coelho"), new DateTime(2004, 1, 2), new Email("pedro02012004@example.com"), new ContactInformation(961309771));
            var staffMock = new Mock<Staff>("Luna", "Silva", "Doctor", "Orthopaedist", "luna.sp.silva@example.com", "966666667");

            _repoOtMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationTypeId>())).ReturnsAsync(operationTypeMock.Object);
            _repoPMock.Setup(repo => repo.GetByIdAsync(It.IsAny<PatientProfileId>())).ReturnsAsync(patientProfileMock.Object);
            _repoSMock.Setup(repo => repo.GetByIdAsync(It.IsAny<StaffId>())).ReturnsAsync(staffMock.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);
        }

        [Fact]
        public async Task AddOperationRequest_ShouldReturnCreatedResult()
        {
            var dto = new CreatingOperationRequestDto(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Priority.Emergency, DateTime.UtcNow.AddDays(1));
            var createdRequest = new Mock<OperationRequest>(new PatientProfileId(dto.PatientId), new StaffId(dto.DoctorId), new OperationTypeId(dto.OperationTypeId), dto.Priority, dto.SuggestedDeadline);

            SetupTestData();
            _repoOrMock.Setup(repo => repo.AddAsync(It.IsAny<OperationRequest>())).Returns(Task.FromResult(createdRequest.Object));

            var result = await _controller.AddOperationRequest(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<OperationRequestDto>(createdResult.Value.GetType().GetProperty("result").GetValue(createdResult.Value));
            Assert.Equal(dto.PatientId, returnValue.PatientId);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult()
        {
            var id = Guid.NewGuid();
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));
            _repoOrMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(request.Object);

            var result = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<OperationRequestDto>(okResult.Value.GetType().GetProperty("operationRequest").GetValue(okResult.Value));
            Assert.Equal(request.Object.Id.AsGuid(), returnValue.Id);
        }

        [Fact]
        public async Task UpdateOperationRequest_ShouldReturnOkResult()
        {
            var dto = new UpdatingOperationRequestDto { Id = Guid.NewGuid(), Priority = Priority.Emergency, SuggestedDeadline = DateTime.UtcNow.AddDays(1) };
            var doctorId = Guid.NewGuid();
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));

            SetupTestData();
            _repoOrMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(request.Object);

            var result = await _controller.UpdateOperationRequest(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OperationRequestDto>(okResult.Value.GetType().GetProperty("result").GetValue(okResult.Value));
            Assert.Equal(dto.Priority, returnValue.Priority);
        }

        [Fact]
        public async Task UpdateOperationRequest_OnlyPriority_ShouldReturnOkResult()
        {
            var dto = new UpdatingOperationRequestDto { Id = Guid.NewGuid(), Priority = Priority.Urgent, SuggestedDeadline = DateTime.UtcNow.AddDays(1) };
            var doctorId = Guid.NewGuid();
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));

            SetupTestData();
            _repoOrMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(request.Object);

            var result = await _controller.UpdateOperationRequest(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OperationRequestDto>(okResult.Value.GetType().GetProperty("result").GetValue(okResult.Value));
            Assert.Equal(dto.Priority, returnValue.Priority);
        }

        [Fact]
        public async Task UpdateOperationRequest_OnlySuggestedDeadline_ShouldReturnOkResult()
        {
            var dto = new UpdatingOperationRequestDto { Id = Guid.NewGuid(), Priority = Priority.Emergency, SuggestedDeadline = DateTime.UtcNow.AddDays(2) };
            var doctorId = Guid.NewGuid();
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));

            SetupTestData();
            _repoOrMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(request.Object);

            var result = await _controller.UpdateOperationRequest(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OperationRequestDto>(okResult.Value.GetType().GetProperty("result").GetValue(okResult.Value));
            Assert.Equal(dto.SuggestedDeadline, returnValue.SuggestedDeadline);
        }

        [Fact]
        public async Task UpdateOperationRequest_BothPriorityAndSuggestedDeadline_ShouldReturnOkResult()
        {
            var dto = new UpdatingOperationRequestDto { Id = Guid.NewGuid(), Priority = Priority.Urgent, SuggestedDeadline = DateTime.UtcNow.AddDays(2) };
            var doctorId = Guid.NewGuid();
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));

            SetupTestData();
            _repoOrMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(request.Object);

            var result = await _controller.UpdateOperationRequest(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<OperationRequestDto>(okResult.Value.GetType().GetProperty("result").GetValue(okResult.Value));
            Assert.Equal(dto.Priority, returnValue.Priority);
            Assert.Equal(dto.SuggestedDeadline, returnValue.SuggestedDeadline);
        }

        [Fact]
        public async Task DeleteOperationRequest_ShouldReturnOkResult()
        {
            var doctorId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));

            SetupTestData();
            _repoOrMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(request.Object);

            var result = await _controller.DeleteOperationRequest(requestId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            _repoOrMock.Verify(repo => repo.Remove(request.Object), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Search_ShouldReturnOkResult()
        {
            var doctorId = Guid.NewGuid();
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(It.IsAny<string>(), It.IsAny<Priority?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(doctorId.ToString(), Priority.Emergency, null, null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Search_ByDoctorId_ShouldReturnOkResult()
        {
            var doctorId = Guid.NewGuid();
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(doctorId.ToString(), null, null, null, null, null))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(doctorId.ToString(), null, null, null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Search_ByPriority_ShouldReturnOkResult()
        {
            var priority = Priority.Emergency;
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), priority, DateTime.UtcNow.AddDays(1));
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(null, priority, null, null, null, null))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(null, priority, null, null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Search_ByOperationTypeName_ShouldReturnOkResult()
        {
            var operationTypeName = "Test Operation";
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(null, null, operationTypeName, null, null, null))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(null, null, operationTypeName, null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Search_ByPatientName_ShouldReturnOkResult()
        {
            var patientName = "John Doe";
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(null, null, null, patientName, null, null))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(null, null, null, patientName, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Search_ByExpectedDueDate_ShouldReturnOkResult()
        {
            var expectedDueDate = DateTime.UtcNow.AddDays(1);
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, expectedDueDate);
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(null, null, null, null, expectedDueDate, null))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(null, null, null, null, expectedDueDate, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Search_ByRequestDate_ShouldReturnOkResult()
        {
            var requestDate = DateTime.UtcNow;
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(null, null, null, null, null, requestDate))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(null, null, null, null, null, requestDate);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }
        
        [Fact]
        public async Task Search_ByOperationTypeNameAndPatientName_ShouldReturnOkResult()
        {
            var operationTypeName = "Appendectomy";
            var patientName = "Pedro Coelho";
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, DateTime.UtcNow.AddDays(1));
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(null, null, operationTypeName, patientName, null, null))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(null, null, operationTypeName, patientName, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }
        
        [Fact]
        public async Task Search_ByDoctorIdAndPriority_ShouldReturnOkResult()
        {
            var doctorId = Guid.NewGuid();
            var priority = Priority.Urgent;
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), priority, DateTime.UtcNow.AddDays(1));
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(doctorId.ToString(), priority, null, null, null, null))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(doctorId.ToString(), priority, null, null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }
        
        [Fact]
        public async Task Search_ByExpectedDueDateAndRequestDate_ShouldReturnOkResult()
        {
            var expectedDueDate = DateTime.UtcNow.AddDays(1);
            var requestDate = DateTime.UtcNow;
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, expectedDueDate);
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(null, null, null, null, expectedDueDate, requestDate))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(null, null, null, null, expectedDueDate, requestDate);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task Search_WithAllCriteria_ShouldReturnOkResult()
        {
            var doctorId = Guid.NewGuid();
            var priority = Priority.Emergency;
            var operationTypeName = "Test Operation";
            var patientName = "John Doe";
            var expectedDueDate = DateTime.UtcNow.AddDays(1);
            var requestDate = DateTime.UtcNow;

            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), priority, expectedDueDate);
            var requests = new List<Mock<OperationRequest>> { request };

            SetupTestData();
            _repoOrMock.Setup(repo => repo.SearchAsync(doctorId.ToString(), priority, operationTypeName, patientName, expectedDueDate, requestDate))
                .ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.Search(doctorId.ToString(), priority, operationTypeName, patientName, expectedDueDate, requestDate);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult()
        {
            var request = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Emergency, DateTime.UtcNow.AddDays(1));
            var requests = new List<Mock<OperationRequest>> { request };

            _repoOrMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(requests.ConvertAll(r => r.Object));

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OperationRequestDto>>(okResult.Value.GetType().GetProperty("operationRequests").GetValue(okResult.Value));
            Assert.Single(returnValue);
        }
    }
}