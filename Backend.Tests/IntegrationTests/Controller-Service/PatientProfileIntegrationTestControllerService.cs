using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Infrastructure;
using DDDNetCore.Controllers;
using DDDNetCore.Domain.Email;
using DDDSample1.Domain.Events.Handlers;
using Microsoft.AspNetCore.Mvc;
using Coravel.Queuing.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Backend.Tests.IntegrationTests.ControllerService
{
    public class PatientProfileIntegrationTestControllerService
    {
        private readonly DbContextOptions<DDDSample1DbContext> _dbContextOptions;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPatientProfileRepository> _repoMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ITokenManager> _tokenManagerMock;
        private readonly PatientProfileService _service;
        private readonly PatientProfileController _controller;
        public PatientProfileIntegrationTestControllerService()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DDDSample1DbContext>()
                .UseInMemoryDatabase("DB_PPIT_CS")
                .Options;

            var dbContext = new DDDSample1DbContext(_dbContextOptions);
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<IPatientProfileRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _tokenManagerMock = new Mock<ITokenManager>();
            var queueMock = new Mock<IQueue>();
            var loggerMock = new Mock<ILogger<PatientProfileService>>();
            var deletionServiceMock = new Mock<PatientDeletionService>(Mock.Of<ILogger<PatientDeletionService>>(), Mock.Of<IServiceProvider>(), Mock.Of<IEmailService>());

            var createdEventHandler = new PatientProfileCreatedEventHandler(Mock.Of<ILogger<PatientProfileCreatedEventHandler>>());
            var deletedEventHandler = new PatientProfileDeletedEventHandler(Mock.Of<ILogger<PatientProfileDeletedEventHandler>>());
            var updatedEventHandler = new PatientProfileUpdatedEventHandler(Mock.Of<ILogger<PatientProfileUpdatedEventHandler>>());

            _service = new PatientProfileService(
                _unitOfWorkMock.Object,
                _repoMock.Object,
                _emailServiceMock.Object,
                createdEventHandler,
                deletedEventHandler,
                _tokenManagerMock.Object,
                updatedEventHandler,
                queueMock.Object,
                loggerMock.Object, 
                deletionServiceMock.Object 
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
                Email = $"john.doe{Guid.NewGuid()}@example.com", // Ensure unique email
                ContactInformation = long.Parse($"9{new Random().Next(10000000, 99999999)}") // Ensure unique and valid contact information
            };
        }
        
        private Mock<PatientProfile> CreateMockPatientProfile(CreatingPatientProfileDto dto)
        {
            var mockProfile = new Mock<PatientProfile>(
                new FirstName(dto.FirstName),
                new LastName(dto.LastName),
                new FullName(dto.FullName),
                dto.DateOfBirth,
                new Email(dto.Email),
                new ContactInformation(dto.ContactInformation)
            );

            return mockProfile;
        }

        private async Task CleanDatabase()
        {
            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                dbContext.PatientProfiles.RemoveRange(dbContext.PatientProfiles);
                await dbContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task AddPatientProfile_ShouldReturnCreatedResult()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();

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
            var mockProfile = new Mock<PatientProfile>(
                new FirstName(dto.FirstName),
                new LastName(dto.LastName),
                new FullName(dto.FullName),
                dto.DateOfBirth,
                new Email(dto.Email),
                new ContactInformation(dto.ContactInformation)
            );

            _repoMock.Setup(repo => repo.FindByEmailAsync(dto.Email)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.UpdatePatientProfile(dto, dto.Email, null, dto.FullName, dto.DateOfBirth.ToString(), dto.ContactInformation.ToString());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PatientProfileDTO>(okResult.Value);
            Assert.Equal(dto.FirstName, returnValue.FirstName);
        }

        // [Fact]
        // public async Task DeletePatientProfile_ShouldReturnOkResult()
        // {
        //     // Arrange
        //     var email = "john.doe@example.com";
        //     var mockProfile = new Mock<PatientProfile>(
        //         new FirstName("John"),
        //         new LastName("Doe"),
        //         new FullName("John Doe"),
        //         DateTime.Now.AddYears(-30),
        //         new Email(email),
        //         new ContactInformation(966666666)
        //     );

        //     _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(mockProfile.Object);
        //     _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        //     // Act
        //     var result = await _controller.DeletePatientProfile(null, null, email, null, null);

        //     // Assert
        //     var okResult = Assert.IsType<OkObjectResult>(result);
        //     _repoMock.Verify(repo => repo.Remove(mockProfile.Object), Times.Once);
        //     _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        // }

        [Fact]
        public async Task ConfirmAccountDeletion_ShouldReturnOkResult()
        {
            // Arrange
            var token = Guid.NewGuid().ToString();
            var email = "luna.sp.1737@gmail.com";
            var mockProfile = new Mock<PatientProfile>(
                new FirstName("Luna"), new LastName("Silva"), new FullName("Luna Silva"),
                DateTime.Now.AddYears(-30), new Email(email), new ContactInformation(962398532)
            );
            _tokenManagerMock.Setup(manager => manager.TryGetToken(token, out email)).Returns(true);
            _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(mockProfile.Object);
            _repoMock.Setup(repo => repo.Remove(mockProfile.Object));
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Mock the User.Claims collection
            var userClaims = new List<Claim> { new Claim(ClaimTypes.Email, email) };
            var user = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.ConfirmAccountDeletion(token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Account deletion confirmed.", okResult.Value);
        }

[Fact]
        public async Task RequestAccountDeletion_ShouldReturnOkResult()
        {
            // Arrange
            var email = "luna.sp.1737@gmail.com";
            var mockProfile = new Mock<PatientProfile>(
                new FirstName("Luna"), new LastName("Silva"), new FullName("Luna Silva"),
                DateTime.Now.AddYears(-30), new Email(email), new ContactInformation(962398532)
            );
            _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(mockProfile.Object);
            _emailServiceMock.Setup(service => service.SendAccountDeletionConfirmationEmailAsync(email, It.IsAny<string>())).Returns(Task.CompletedTask);

            var userClaims = new List<Claim> { new Claim(ClaimTypes.Email, email) };
            var user = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.RequestAccountDeletion();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Confirmation email sent.", okResult.Value);
        }
        
        [Fact]
        public async Task GetById_ShouldReturnPatientProfile()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var mockProfile = CreateMockPatientProfile(dto);
            var id = mockProfile.Object.Id;

            _repoMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(mockProfile.Object);

            // Act
            var result = await _controller.GetById(id.AsGuid());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PatientProfileDTO>(okResult.Value);
            Assert.Equal(dto.FirstName, returnValue.FirstName);
        }
        
        [Fact]
        public async Task GetAll_ShouldReturnAllPatientProfiles()
        {
            // Arrange
            var profiles = new List<Mock<PatientProfile>>
            {
                CreateMockPatientProfile(new CreatingPatientProfileDto { FirstName = "John", LastName = "Doe", FullName = "John Doe", DateOfBirth = DateTime.Now.AddYears(-30), Email = "john.doe@example.com", ContactInformation = 966666666 }),
                CreateMockPatientProfile(new CreatingPatientProfileDto { FirstName = "Jane", LastName = "Doe", FullName = "Jane Doe", DateOfBirth = DateTime.Now.AddYears(-25), Email = "jane.doe@example.com", ContactInformation = 933333333 })
            };
            _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(profiles.ConvertAll(p => p.Object));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PatientProfileDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
        
        [Fact]
        public async Task UpdatePatientProfilePatient_ShouldReturnOkResult()
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
    }
}