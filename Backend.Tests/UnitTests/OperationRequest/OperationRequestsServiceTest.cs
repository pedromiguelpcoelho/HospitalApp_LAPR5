using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Domain.Events.Handlers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Backend.Tests
{
    public class OperationRequestsServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IOperationRequestRepository> _repoOrMock;
        private readonly Mock<IOperationTypeRepository> _repoOtMock;
        private readonly Mock<IPatientProfileRepository> _repoPMock;
        private readonly Mock<IStaffRepository> _repoSMock;
        private readonly OperationRequestService _service;

        public OperationRequestsServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repoOrMock = new Mock<IOperationRequestRepository>();
            _repoOtMock = new Mock<IOperationTypeRepository>();
            _repoPMock = new Mock<IPatientProfileRepository>();
            _repoSMock = new Mock<IStaffRepository>();

            var loggerMock = new Mock<ILogger<OperationRequestCreatedEventHandler>>();
            var loggerDeletedMock = new Mock<ILogger<OperationRequestDeletedEventHandler>>();
            var loggerUpdatedMock = new Mock<ILogger<OperationRequestUpdatedEventHandler>>();

            _service = new OperationRequestService(
                _unitOfWorkMock.Object,
                _repoOrMock.Object,
                _repoOtMock.Object,
                _repoPMock.Object,
                _repoSMock.Object,
                new OperationRequestCreatedEventHandler(loggerMock.Object),
                new OperationRequestDeletedEventHandler(loggerDeletedMock.Object),
                new OperationRequestUpdatedEventHandler(loggerUpdatedMock.Object)
            );
        }

        private void SetupCommonMocks()
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
        public async Task GetAllAsync_ShouldReturnAllOperationRequests()
        {
            var operationRequests = new List<Mock<OperationRequest>>
            {
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, DateTime.Now.AddDays(1)),
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Elective, DateTime.Now.AddDays(1))
            };

            _repoOrMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(operationRequests.ConvertAll(or => or.Object));

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOperationRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var operationRequestMock = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, DateTime.Now.AddDays(1));

            _repoOrMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(operationRequestMock.Object);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(operationRequestMock.Object.Id.AsGuid(), result.Id);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnCreatedOperationRequest()
        {
            // Arrange
            var dto = new CreatingOperationRequestDto(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Priority.Urgent, DateTime.Now.AddDays(1));
            var operationRequestMock = new Mock<OperationRequest>(new PatientProfileId(dto.PatientId), new StaffId(dto.DoctorId), new OperationTypeId(dto.OperationTypeId), dto.Priority, dto.SuggestedDeadline);

            SetupCommonMocks();
            _repoOrMock.Setup(repo => repo.AddAsync(It.IsAny<OperationRequest>())).ReturnsAsync(operationRequestMock.Object);
            _repoOrMock.Setup(repo => repo.SearchAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new List<OperationRequest>());

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.PatientId, result.PatientId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedOperationRequest()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var dto = new UpdatingOperationRequestDto { Id = Guid.NewGuid(), Priority = Priority.Elective, SuggestedDeadline = DateTime.Now.AddDays(1) };
            var operationRequestMock = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, DateTime.Now.AddDays(1));

            SetupCommonMocks();
            _repoOrMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(operationRequestMock.Object);

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Priority, result.Priority);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var operationRequestId = Guid.NewGuid();
            var operationRequestMock = new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, DateTime.Now.AddDays(1));

            SetupCommonMocks();
            _repoOrMock.Setup(repo => repo.GetByIdAsync(It.IsAny<OperationRequestId>())).ReturnsAsync(operationRequestMock.Object);

            // Act
            var result = await _service.DeleteAsync(operationRequestId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingOperationRequests_ByDoctorId()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var operationRequests = new List<Mock<OperationRequest>>
            {
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, DateTime.Now.AddDays(1)),
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Elective, DateTime.Now.AddDays(1))
            };

            _repoOrMock.Setup(repo => repo.SearchAsync(doctorId.ToString(), null, null, null, null, null))
                .ReturnsAsync(operationRequests.Select(or => or.Object).Where(or => or.DoctorId.AsGuid() == doctorId).ToList());

            // Act
            var result = await _service.SearchAsync(doctorId.ToString(), null, null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, or => Assert.Equal(doctorId.ToString(), or.DoctorId.ToString()));
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingOperationRequests_ByPatientName()
        {
            // Arrange
            var patientName = "Pedro Coelho";
            var patientId = Guid.NewGuid();
            var operationRequests = new List<Mock<OperationRequest>>
            {
                new Mock<OperationRequest>(new PatientProfileId(patientId), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, DateTime.Now.AddDays(1))
            };

            _repoOrMock.Setup(repo => repo.SearchAsync(null, null, null, patientName, null, null))
                .ReturnsAsync(operationRequests.Select(or => or.Object).Where(or => or.PatientId.AsGuid() == patientId).ToList());

            // Act
            var result = await _service.SearchAsync(null, null, null, patientName, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(patientId, result.First().PatientId);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingOperationRequests_ByPriority()
        {
            // Arrange
            var priority = Priority.Urgent;
            var operationRequests = new List<Mock<OperationRequest>>
            {
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, DateTime.Now.AddDays(1)),
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Elective, DateTime.Now.AddDays(1))
            };

            _repoOrMock.Setup(repo => repo.SearchAsync(null, priority, null, null, null, null))
                .ReturnsAsync(operationRequests.Select(or => or.Object).Where(or => or.Priority == priority).ToList());

            // Act
            var result = await _service.SearchAsync(null, priority, null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.All(result, or => Assert.Equal(priority, or.Priority));
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingOperationRequests_ByOperationTypeName()
        {
            // Arrange
            var operationTypeName = "Appendecectomy";
            var operationTypeId = Guid.NewGuid();
            var operationRequests = new List<Mock<OperationRequest>>
            {
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(operationTypeId), Priority.Urgent, DateTime.Now.AddDays(1)),
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Elective, DateTime.Now.AddDays(1))
            };

            _repoOrMock.Setup(repo => repo.SearchAsync(null, null, operationTypeName, null, null, null))
                       .ReturnsAsync(operationRequests.Select(or => or.Object).Where(or => or.OperationTypeId.AsGuid() == operationTypeId).ToList());

            // Act
            var result = await _service.SearchAsync(null, null, operationTypeName, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.All(result, or => Assert.Equal(operationTypeId, or.OperationTypeId));
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingOperationRequests_ByExpectedDueDate()
        {
            // Arrange
            var expectedDueDate = DateTime.Now.AddDays(1);
            var operationRequests = new List<Mock<OperationRequest>>
            {
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(Guid.NewGuid()), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, expectedDueDate)
            };

            _repoOrMock.Setup(repo => repo.SearchAsync(null, null, null, null, expectedDueDate, null))
                .ReturnsAsync(operationRequests.Select(or => or.Object).Where(or => or.SuggestedDeadline.Date == expectedDueDate.Date).ToList());

            // Act
            var result = await _service.SearchAsync(null, null, null, null, expectedDueDate, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.All(result, or => Assert.Equal(expectedDueDate.Date, or.SuggestedDeadline.Date));
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingOperationRequests_ByDoctorIdAndPriority()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var priority = Priority.Urgent;
            var operationRequests = new List<Mock<OperationRequest>>
            {
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Urgent, DateTime.Now.AddDays(1)),
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(Guid.NewGuid()), Priority.Elective, DateTime.Now.AddDays(1))
            };

            _repoOrMock.Setup(repo => repo.SearchAsync(doctorId.ToString(), priority, null, null, null, null))
                       .ReturnsAsync(operationRequests.Select(or => or.Object).Where(or => or.DoctorId.AsGuid() == doctorId && or.Priority == priority).ToList());

            // Act
            var result = await _service.SearchAsync(doctorId.ToString(), priority, null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(doctorId, result.First().DoctorId);
            Assert.Equal(priority, result.First().Priority);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingOperationRequests_ByOperationTypeNameAndPatientName()
        {
            // Arrange
            var operationTypeName = "Appendectomy";
            var patientName = "Pedro Coelho";
            var patientId = Guid.NewGuid();
            var operationTypeId = Guid.NewGuid();
            var operationRequests = new List<Mock<OperationRequest>>
            {
                new Mock<OperationRequest>(new PatientProfileId(patientId), new StaffId(Guid.NewGuid()), new OperationTypeId(operationTypeId), Priority.Urgent, DateTime.Now.AddDays(1))
            };

            _repoOrMock.Setup(repo => repo.SearchAsync(null, null, operationTypeName, patientName, null, null))
                .ReturnsAsync(operationRequests.Select(or => or.Object).Where(or => or.OperationTypeId.AsGuid() == operationTypeId && or.PatientId.AsGuid() == patientId).ToList());

            // Act
            var result = await _service.SearchAsync(null, null, operationTypeName, patientName, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(operationTypeId, result.First().OperationTypeId);
            Assert.Equal(patientId, result.First().PatientId);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMatchingOperationRequests_ByDoctorIdPriorityAndOperationTypeName()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var priority = Priority.Urgent;
            var operationTypeName = "Appendectomy";
            var operationTypeId = Guid.NewGuid();
            var operationRequests = new List<Mock<OperationRequest>>
            {
                new Mock<OperationRequest>(new PatientProfileId(Guid.NewGuid()), new StaffId(doctorId), new OperationTypeId(operationTypeId), Priority.Urgent, DateTime.Now.AddDays(1))
            };

            _repoOrMock.Setup(repo => repo.SearchAsync(doctorId.ToString(), priority, operationTypeName, null, null, null))
                .ReturnsAsync(operationRequests.Select(or => or.Object).Where(or => or.DoctorId.AsGuid() == doctorId && or.Priority == priority && or.OperationTypeId.AsGuid() == operationTypeId).ToList());

            // Act
            var result = await _service.SearchAsync(doctorId.ToString(), priority, operationTypeName, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(doctorId, result.First().DoctorId);
            Assert.Equal(priority, result.First().Priority);
            Assert.Equal(operationTypeId, result.First().OperationTypeId);
        }

        // [Fact]
        // public async Task SearchAsync_ShouldReturnMatchingOperationRequests_ByAllCriteria()
        // {
        //     var doctorId = Guid.NewGuid();
        //     var priority = Priority.Urgent;
        //     var operationTypeName = "Appendectomy";
        //     var patientName = "Pedro Coelho";
        //     var expectedDueDate = DateTime.Now.AddDays(1);
        //     var requestDate = DateTime.UtcNow.Date;
        //     var patientId = Guid.NewGuid();
        //     var operationTypeId = Guid.NewGuid();
        //     var operationRequests = new List<OperationRequest>
        //     {
        //         new OperationRequest(new PatientProfileId(patientId), new StaffId(doctorId), new OperationTypeId(operationTypeId), priority, expectedDueDate)
        //     };
        //
        //     operationRequests[0].GetType().GetProperty("RequestDate").SetValue(operationRequests[0], requestDate);
        //
        //     _repoOrMock.Setup(repo => repo.SearchAsync(doctorId, priority, operationTypeName, patientName, expectedDueDate, requestDate))
        //                .ReturnsAsync(operationRequests.Where(or =>
        //                    or.DoctorId.AsGuid() == doctorId &&
        //                    or.Priority == priority &&
        //                    or.OperationTypeId.AsGuid() == operationTypeId &&
        //                    or.PatientId.AsGuid() == patientId &&
        //                    or.SuggestedDeadline.Date == expectedDueDate.Date &&
        //                    or.RequestDate.Date == requestDate).ToList());
        //
        //     var result = await _service.SearchAsync(doctorId, priority, operationTypeName, patientName, expectedDueDate, requestDate);
        //
        //     Assert.NotNull(result);
        //     Assert.Single(result);
        //     Assert.Equal(doctorId, result.First().DoctorId);
        //     Assert.Equal(priority, result.First().Priority);
        //     Assert.Equal(operationTypeId, result.First().OperationTypeId);
        //     Assert.Equal(patientId, result.First().PatientId);
        //     Assert.Equal(expectedDueDate.Date, result.First().SuggestedDeadline.Date);
        //     Assert.Equal(requestDate, result.First().RequestDate.Date);
        // }
    }
}