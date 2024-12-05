using System;
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
using System.Collections.Generic;
using Coravel.Queuing.Interfaces;

namespace Backend.Tests
{
    public class PatientProfileIntegrationTestServiceRepository
    {
        private readonly DbContextOptions<DDDSample1DbContext> _dbContextOptions;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPatientProfileRepository> _repoMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ITokenManager> _tokenManagerMock;
        private readonly Mock<IQueue> _queueMock;
        private readonly Mock<PatientDeletionService> _deletionServiceMock;
        private readonly Mock<ILogger<PatientProfileService>> _loggerMock;
        private readonly PatientProfileService _service;

        public PatientProfileIntegrationTestServiceRepository()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DDDSample1DbContext>()
                .UseInMemoryDatabase("DB_PPIT_SR")
                .Options;

            var dbContext = new DDDSample1DbContext(_dbContextOptions);
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<IPatientProfileRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _tokenManagerMock = new Mock<ITokenManager>();
            _queueMock = new Mock<IQueue>();
            _deletionServiceMock = new Mock<PatientDeletionService>(Mock.Of<ILogger<PatientDeletionService>>(), Mock.Of<IServiceProvider>(), Mock.Of<IEmailService>());
            _loggerMock = new Mock<ILogger<PatientProfileService>>();

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
                _queueMock.Object,
                _loggerMock.Object,
                _deletionServiceMock.Object
            );
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

        private async Task CleanDatabase()
        {
            using (var dbContext = new DDDSample1DbContext(_dbContextOptions))
            {
                dbContext.PatientProfiles.RemoveRange(dbContext.PatientProfiles);
                await dbContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewPatientProfile()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
            Assert.Equal(dto.LastName, result.LastName);
            Assert.Equal(dto.FullName, result.FullName);
            Assert.Equal(dto.DateOfBirth, result.DateOfBirth);
            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(dto.ContactInformation, result.ContactInformation);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePatientProfile()
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
            var result = await _service.UpdateAsyncPatient(dto, email: dto.Email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
            Assert.Equal(dto.LastName, result.LastName);
            Assert.Equal(dto.FullName, result.FullName);
            Assert.Equal(dto.DateOfBirth, result.DateOfBirth);
            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(dto.ContactInformation, result.ContactInformation);
            _repoMock.Verify(repo => repo.FindByEmailAsync(dto.Email), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsyncPatient_ShouldUpdatePatientProfile()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var profile = new Mock<PatientProfile>(new FirstName(dto.FirstName), new LastName(dto.LastName), new FullName(dto.FullName), dto.DateOfBirth, new Email(dto.Email), new ContactInformation(dto.ContactInformation));

            _repoMock.Setup(repo => repo.FindByEmailAsync(dto.Email)).ReturnsAsync(profile.Object);
            _repoMock.Setup(repo => repo.FindByEmailAndExcludedIdAsync(dto.Email, profile.Object.Id)).ReturnsAsync((PatientProfile)null);
            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(It.IsAny<long>())).ReturnsAsync((PatientProfile)null);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            var result = await _service.UpdateAsyncPatient(dto, email: dto.Email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
            _repoMock.Verify(repo => repo.FindByEmailAsync(dto.Email), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeletePatientProfile()
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
            var result = await _service.DeleteAsync(email: dto.Email);

            // Assert
            Assert.True(result);
            _repoMock.Verify(repo => repo.FindByEmailAsync(dto.Email), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
            _queueMock.Verify(queue => queue.QueueAsyncTask(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPatientProfile()
        {
            // Arrange
            var dto = CreateTestPatientProfileDto();
            var profile = new Mock<PatientProfile>(new FirstName(dto.FirstName), new LastName(dto.LastName), new FullName(dto.FullName), dto.DateOfBirth, new Email(dto.Email), new ContactInformation(dto.ContactInformation));
            var id = profile.Object.Id;

            _repoMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(profile.Object);

            // Act
            var result = await _service.GetByIdAsync(id.AsGuid());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPatientProfiles()
        {
            // Arrange
            var profiles = new List<Mock<PatientProfile>>
            {
                new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Now.AddYears(-30), new Email("joao.sousa@example.com"), new ContactInformation(966666666)),
                new Mock<PatientProfile>(new FirstName("Jane"), new LastName("Doe"), new FullName("Jane Doe"), DateTime.Now.AddYears(-25), new Email("jane.doe@example.com"), new ContactInformation(933333333))
            };
            _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(profiles.ConvertAll(p => p.Object));

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task ConfirmAndDeleteAccountAsync_ShouldDeleteAccount()
        {
            // Arrange
            var email = "luna.sp.1737@gmail.com";
            var mockProfile = new Mock<PatientProfile>(
                new FirstName("Luna"),
                new LastName("Sp"),
                new FullName("Luna Sp"),
                DateTime.Now.AddYears(-30),
                new Email(email),
                new ContactInformation(923456789)
            );

            _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(mockProfile.Object);
            _repoMock.Setup(repo => repo.GetByIdAsync(mockProfile.Object.Id)).ReturnsAsync(mockProfile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            // Act
            await _deletionServiceMock.Object.ConfirmAndDeleteAccountAsync(mockProfile.Object.Id, _repoMock.Object, _unitOfWorkMock.Object, Mock.Of<ILogger>());

            // Assert
            _repoMock.Verify(repo => repo.GetByIdAsync(mockProfile.Object.Id), Times.Once);
            _repoMock.Verify(repo => repo.Remove(mockProfile.Object), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task RequestAccountDeletionByEmailAsync_ShouldSendConfirmationEmail()
        {
            // Arrange
            var email = "luna.sp.1737@gmail.com";
            var profile = new Mock<PatientProfile>(new FirstName("Luna"), new LastName("Silva"), new FullName("Luna Silva"), DateTime.Now.AddYears(-30), new Email(email), new ContactInformation(962398532));
            _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(profile.Object);
            _emailServiceMock.Setup(service => service.SendAccountDeletionConfirmationEmailAsync(email, It.IsAny<string>())).Returns(Task.CompletedTask);

            string capturedToken = null;
            string capturedEmail = null;
            TimeSpan capturedTimeSpan = default;

            _tokenManagerMock.Setup(manager => manager.SetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()))
                .Callback<string, string, TimeSpan>((token, email, timeSpan) =>
                {
                    capturedToken = token;
                    capturedEmail = email;
                    capturedTimeSpan = timeSpan;
                });

            // Act
            await _service.RequestAccountDeletionByEmailAsync(email);

            // Assert
            _repoMock.Verify(repo => repo.FindByEmailAsync(email), Times.Once);
            _emailServiceMock.Verify(service => service.SendAccountDeletionConfirmationEmailAsync(email, It.IsAny<string>()), Times.Once);
            Assert.NotNull(capturedToken);
            Assert.Equal(email, capturedEmail);
            Assert.Equal(TimeSpan.FromHours(1), capturedTimeSpan);
        }
    }
}