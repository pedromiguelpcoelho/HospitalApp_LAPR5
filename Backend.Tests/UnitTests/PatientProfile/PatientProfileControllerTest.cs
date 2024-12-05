using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDNetCore.Controllers;
using DDDNetCore.Domain.Email;
using Microsoft.Extensions.Logging;
using DDDSample1.Domain.Events.Handlers;
using Coravel.Queuing.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Backend.Tests
{
    public class PatientProfileControllerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPatientProfileRepository> _repoMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ILogger<PatientProfileCreatedEventHandler>> _loggerCreatedMock;
        private readonly Mock<ILogger<PatientProfileDeletedEventHandler>> _loggerDeletedMock;
        private readonly Mock<ITokenManager> _tokenManagerMock;
        private readonly Mock<ILogger<PatientProfileUpdatedEventHandler>> _loggerUpdatedMock;
        private readonly Mock<IQueue> _queueMock;
        private readonly Mock<PatientDeletionService> _deletionServiceMock;
        private readonly PatientProfileService _service;
        private readonly PatientProfileController _controller;

        public PatientProfileControllerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<IPatientProfileRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _loggerCreatedMock = new Mock<ILogger<PatientProfileCreatedEventHandler>>();
            _loggerDeletedMock = new Mock<ILogger<PatientProfileDeletedEventHandler>>();
            _tokenManagerMock = new Mock<ITokenManager>();
            _loggerUpdatedMock = new Mock<ILogger<PatientProfileUpdatedEventHandler>>();
            _queueMock = new Mock<IQueue>();
            _deletionServiceMock = new Mock<PatientDeletionService>(Mock.Of<ILogger<PatientDeletionService>>(), Mock.Of<IServiceProvider>(), Mock.Of<IEmailService>());

            var createdEventHandler = new PatientProfileCreatedEventHandler(_loggerCreatedMock.Object);
            var deletedEventHandler = new PatientProfileDeletedEventHandler(_loggerDeletedMock.Object);
            var updatedEventHandler = new PatientProfileUpdatedEventHandler(_loggerUpdatedMock.Object);

            _service = new PatientProfileService(
                _unitOfWorkMock.Object,
                _repoMock.Object,
                _emailServiceMock.Object,
                createdEventHandler,
                deletedEventHandler,
                _tokenManagerMock.Object,
                updatedEventHandler,
                _queueMock.Object,
                Mock.Of<ILogger<PatientProfileService>>(),
                _deletionServiceMock.Object
            );

            _controller = new PatientProfileController(_service);
        }

        private CreatingPatientProfileDto CreateTestPatientProfileDto()
        {
            return new CreatingPatientProfileDto
            {
                FirstName = "John",
                LastName = "Doe",
                FullName = "John Doe",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Email = "john.doe@example.com",
                ContactInformation = 966666666
            };
        }

        private Mock<PatientProfile> CreateMockPatientProfile()
        {
            return new Mock<PatientProfile>(
                new FirstName("John"), new LastName("Doe"), new FullName("John Doe"),
                DateTime.Now.AddYears(-30), new Email("john.doe@example.com"), new ContactInformation(966666666)
            );
        }
        
        private Mock<PatientProfile> CreateMockPatientProfile(CreatingPatientProfileDto dto)
        {
            return new Mock<PatientProfile>(
                new FirstName(dto.FirstName),
                new LastName(dto.LastName),
                new FullName(dto.FullName),
                dto.DateOfBirth,
                new Email(dto.Email),
                new ContactInformation(dto.ContactInformation)
            );
        }
        
        private PatientProfileDTO CreatePatientProfileDto(PatientProfile profile)
        {
            return new PatientProfileDTO
            {
                Id = profile.Id.AsGuid(), FirstName = profile.FirstName.Value,
                LastName = profile.LastName.Value, FullName = profile.FullName.Value,
                DateOfBirth = profile.DateOfBirth, Email = profile.Email.Value,
                ContactInformation = profile.ContactInformation.Value
            };
        }
        
        [Fact]
        public async Task AddPatientProfile_ShouldReturnCreatedResult()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var mockProfile = new Mock<PatientProfile>(
                new FirstName(dto.FirstName),
                new LastName(dto.LastName),
                new FullName(dto.FullName),
                dto.DateOfBirth,
                new Email(dto.Email),
                new ContactInformation(dto.ContactInformation)
            );

            _repoMock.Setup(repo => repo.AddAsync(It.IsAny<PatientProfile>())).Returns(Task.FromResult(mockProfile.Object));
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.AddPatientProfile(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<PatientProfileDTO>(createdResult.Value);
            Assert.Equal(dto.FirstName, returnValue.FirstName);
        }

        [Fact]
        public async Task UpdatePatientProfile_ShouldReturnOkResult()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var mockProfile = CreateMockPatientProfile(dto);
            var updatedProfileDto = CreatePatientProfileDto(mockProfile.Object);

            _repoMock.Setup(repo => repo.FindByEmailAsync(dto.Email)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.UpdatePatientProfile(dto, dto.Email, null, dto.FullName, dto.DateOfBirth.ToString(), dto.ContactInformation.ToString());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PatientProfileDTO>(okResult.Value);
            Assert.Equal(dto.FirstName, returnValue.FirstName);
        }

        [Fact]
        public async Task UpdatePatientProfile_WithEmail_ShouldReturnOkResult()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var mockProfile = CreateMockPatientProfile(dto);
            var updatedProfileDto = CreatePatientProfileDto(mockProfile.Object);

            _repoMock.Setup(repo => repo.FindByEmailAsync(dto.Email)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.UpdatePatientProfile(dto, dto.Email, null, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PatientProfileDTO>(okResult.Value);
            Assert.Equal(dto.FirstName, returnValue.FirstName);
        }

        [Fact]
        public async Task UpdatePatientProfile_WithFullName_ShouldReturnOkResult()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var mockProfile = CreateMockPatientProfile(dto);
            var updatedProfileDto = CreatePatientProfileDto(mockProfile.Object);

            _repoMock.Setup(repo => repo.FindByFullNameAsync(dto.FullName)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.UpdatePatientProfile(dto, null, null, dto.FullName, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PatientProfileDTO>(okResult.Value);
            Assert.Equal(dto.FirstName, returnValue.FirstName);
        }

        [Fact]
        public async Task UpdatePatientProfile_WithPhoneNumber_ShouldReturnOkResult()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var mockProfile = CreateMockPatientProfile(dto);
            var updatedProfileDto = CreatePatientProfileDto(mockProfile.Object);

            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(dto.ContactInformation)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.UpdatePatientProfile(dto, null, null, null, null, dto.ContactInformation.ToString());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PatientProfileDTO>(okResult.Value);
            Assert.Equal(dto.FirstName, returnValue.FirstName);
        }

        [Fact]
        public async Task UpdatePatientProfile_WithMedicalRecordNumber_ShouldReturnOkResult()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var medicalRecordNumber = "202302000012";
            var mockProfile = CreateMockPatientProfile(dto);
            
            mockProfile.Object.MedicalRecordNumber = medicalRecordNumber;
            
            var updatedProfileDto = CreatePatientProfileDto(mockProfile.Object);

            _repoMock.Setup(repo => repo.FindByMedicalRecordNumberAsync(medicalRecordNumber)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.UpdatePatientProfile(dto, null, medicalRecordNumber, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PatientProfileDTO>(okResult.Value);
            Assert.Equal(dto.FirstName, returnValue.FirstName);
        }
        
        [Fact]
        public async Task UpdatePatientProfilePatient_ShouldReturnOkResult()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var mockProfile = CreateMockPatientProfile(dto);
            var updatedProfileDto = CreatePatientProfileDto(mockProfile.Object);

            _repoMock.Setup(repo => repo.FindByEmailAsync(dto.Email)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, dto.Email)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.UpdatePatientProfilePatient(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PatientProfileDTO>(okResult.Value);
            Assert.Equal(dto.FirstName, returnValue.FirstName);
            Assert.Equal(dto.LastName, returnValue.LastName);
            Assert.Equal(dto.FullName, returnValue.FullName);
            Assert.Equal(dto.DateOfBirth, returnValue.DateOfBirth);
            Assert.Equal(dto.Email, returnValue.Email);
            Assert.Equal(dto.ContactInformation, returnValue.ContactInformation);
            _repoMock.Verify(repo => repo.FindByEmailAsync(dto.Email), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeletePatientProfile_ByMedicalRecordNumber_ShouldReturnOkResult()
        {
            // Arrange
            var medicalRecordNumber = "20013557766";
            var mockProfile = CreateMockPatientProfile();
            mockProfile.Object.MedicalRecordNumber = medicalRecordNumber;

            _repoMock.Setup(repo => repo.FindByMedicalRecordNumberAsync(medicalRecordNumber)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.DeletePatientProfile(null, medicalRecordNumber, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _repoMock.Verify(repo => repo.FindByMedicalRecordNumberAsync(medicalRecordNumber), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
            _queueMock.Verify(queue => queue.QueueAsyncTask(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Fact]
        public async Task DeletePatientProfile_ByDateOfBirth_ShouldReturnOkResult()
        {
            // Arrange
            var dateOfBirth = DateTime.Now.AddYears(-30).ToString("yyyy-MM-dd");
            var mockProfile = CreateMockPatientProfile();

            _repoMock.Setup(repo => repo.FindByDateOfBirthAsync(DateTime.Parse(dateOfBirth))).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.DeletePatientProfile(dateOfBirth, null, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _repoMock.Verify(repo => repo.FindByDateOfBirthAsync(DateTime.Parse(dateOfBirth)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
            _queueMock.Verify(queue => queue.QueueAsyncTask(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Fact]
        public async Task DeletePatientProfile_ByFullName_ShouldReturnOkResult()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var mockProfile = CreateMockPatientProfile(dto);
            _repoMock.Setup(repo => repo.FindByFullNameAsync(dto.FullName)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.DeletePatientProfile(fullName: dto.FullName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _repoMock.Verify(repo => repo.FindByFullNameAsync(dto.FullName), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
            _queueMock.Verify(queue => queue.QueueAsyncTask(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Fact]
        public async Task DeletePatientProfile_ByPhoneNumber_ShouldReturnOkResult()
        {
            // Arrange
            var phoneNumber = "966666666";
            var mockProfile = CreateMockPatientProfile();

            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(long.Parse(phoneNumber))).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.DeletePatientProfile(null, null, null, null, phoneNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _repoMock.Verify(repo => repo.FindByPhoneNumberAsync(long.Parse(phoneNumber)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
            _queueMock.Verify(queue => queue.QueueAsyncTask(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ByEmail_ShouldReturnPatientProfile()
        {
            // Arrange
            var email = "john.doe@example.com";
            var mockProfile = CreateMockPatientProfile();

            _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(mockProfile.Object);

            // Act
            var result = await _service.GetAllAsync(email: email);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(email, result[0].Email);
        }

        [Fact]
        public async Task GetAllAsync_ByMedicalRecordNumber_ShouldReturnPatientProfile()
        {
            // Arrange
            var medicalRecordNumber = "200703884455";
            var mockProfile = CreateMockPatientProfile();
            mockProfile.Object.MedicalRecordNumber = medicalRecordNumber;

            _repoMock.Setup(repo => repo.FindByMedicalRecordNumberAsync(medicalRecordNumber)).ReturnsAsync(mockProfile.Object);

            // Act
            var result = await _service.GetAllAsync(medicalRecordNumber: medicalRecordNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(medicalRecordNumber, result[0].MedicalRecordNumber);
        }
        
        [Fact]
        public async Task GetAllAsync_ByDateOfBirth_ShouldReturnPatientProfile()
        {
            // Arrange
            var dateOfBirth = DateTime.Now.AddYears(-30).ToString("yyyy-MM-dd");
            var mockProfile = CreateMockPatientProfile();
            mockProfile.Object.DateOfBirth = DateTime.Parse(dateOfBirth);

            _repoMock.Setup(repo => repo.FindByDateOfBirthAsync(DateTime.Parse(dateOfBirth))).ReturnsAsync(mockProfile.Object);

            // Act
            var result = await _service.GetAllAsync(dateOfBirth: dateOfBirth);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(dateOfBirth, result[0].DateOfBirth.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public async Task GetAllAsync_ByFullName_ShouldReturnPatientProfile()
        {
            // Arrange
            var fullName = "John Doe";
            var mockProfile = CreateMockPatientProfile();
            mockProfile.Object.FullName = new FullName(fullName);

            _repoMock.Setup(repo => repo.FindByFullNameAsync(fullName)).ReturnsAsync(mockProfile.Object);

            // Act
            var result = await _service.GetAllAsync(fullName: fullName);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(fullName, result[0].FullName);
        }

        [Fact]
        public async Task GetAllAsync_ByPhoneNumber_ShouldReturnPatientProfile()
        {
            // Arrange
            var phoneNumber = "966666666";
            var mockProfile = CreateMockPatientProfile();
            mockProfile.Object.ContactInformation = new ContactInformation(long.Parse(phoneNumber));

            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(long.Parse(phoneNumber))).ReturnsAsync(mockProfile.Object);

            // Act
            var result = await _service.GetAllAsync(phoneNumber: phoneNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(phoneNumber, result[0].ContactInformation.ToString());
        }
        
        [Fact]
        public async Task ConfirmAccountDeletion_ShouldReturnOkResult()
        {
            // Arrange
            var token = Guid.NewGuid().ToString();
            var email = "luna.sp.1737@gmail.com";
            var mockProfile = CreateMockPatientProfile();
            mockProfile.Object.Email = new Email(email);

            _tokenManagerMock.Setup(manager => manager.TryGetToken(token, out email)).Returns(true);
            _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, email)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.ConfirmAccountDeletion(token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Account deletion confirmed.", okResult.Value);
            _repoMock.Verify(repo => repo.FindByEmailAsync(email), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
            _queueMock.Verify(queue => queue.QueueAsyncTask(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Fact]
        public async Task RequestAccountDeletion_ShouldReturnOkResult()
        {
            // Arrange
            var email = "luna.sp.1737@gmail.com";
            var mockProfile = CreateMockPatientProfile();
            mockProfile.Object.Email = new Email(email);
            _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(mockProfile.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, email)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.RequestAccountDeletion();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Confirmation email sent.", okResult.Value);
            _repoMock.Verify(repo => repo.FindByEmailAsync(email), Times.Once);
        }
    }
}