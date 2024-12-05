using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDNetCore.Domain.Email;
using DDDSample1.Domain.Events.Handlers;
using Coravel.Queuing.Interfaces;

namespace Backend.Tests
{
    public class PatientProfileServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPatientProfileRepository> _repoMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ILogger<PatientProfileCreatedEventHandler>> _loggerCreatedMock;
        private readonly Mock<ILogger<PatientProfileDeletedEventHandler>> _loggerDeletedMock;
        private readonly Mock<ITokenManager> _tokenManagerMock;
        private readonly Mock<ILogger<PatientProfileUpdatedEventHandler>> _loggerUpdatedMock;
        private readonly PatientProfileService _service;

        public PatientProfileServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<IPatientProfileRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _loggerCreatedMock = new Mock<ILogger<PatientProfileCreatedEventHandler>>();
            _loggerDeletedMock = new Mock<ILogger<PatientProfileDeletedEventHandler>>();
            _tokenManagerMock = new Mock<ITokenManager>();
            _loggerUpdatedMock = new Mock<ILogger<PatientProfileUpdatedEventHandler>>();
            var queueMock = new Mock<IQueue>();
            var loggerMock = new Mock<ILogger<PatientProfileService>>();
            var deletionServiceMock = new Mock<PatientDeletionService>(Mock.Of<ILogger<PatientDeletionService>>(), Mock.Of<IServiceProvider>(), Mock.Of<IEmailService>());

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
                queueMock.Object,
                loggerMock.Object,
                deletionServiceMock.Object
            );
        }

        private CreatingPatientProfileDto CreateTestPatientProfileDto()
        {
            return new CreatingPatientProfileDto
            {
                FirstName = "João",
                LastName = "Sousa",
                FullName = "João Carneiro Sousa",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Email = "joao.sousa@example.com",
                ContactInformation = 966666666
            };
        }

        [Fact]
        public async Task AddAsync_ShouldReturnCreatedPatientProfile()
        {
            var dto = CreateTestPatientProfileDto();
            var profile = new Mock<PatientProfile>(new FirstName(dto.FirstName), new LastName(dto.LastName),
                new FullName(dto.FullName), dto.DateOfBirth, new Email(dto.Email),
                new ContactInformation(dto.ContactInformation));

            _repoMock.Setup(repo => repo.FindByEmailAsync(dto.Email)).ReturnsAsync((PatientProfile)null);
            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(It.IsAny<long>())).ReturnsAsync((PatientProfile)null);
            _repoMock.Setup(repo => repo.AddAsync(It.IsAny<PatientProfile>())).ReturnsAsync(profile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.AddAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedPatientProfile()
        {
            var dto = CreateTestPatientProfileDto();
            var profile = new Mock<PatientProfile>(new FirstName(dto.FirstName), new LastName(dto.LastName), new FullName(dto.FullName), dto.DateOfBirth, new Email(dto.Email), new ContactInformation(dto.ContactInformation));

            _repoMock.Setup(repo => repo.FindByEmailAsync(dto.Email)).ReturnsAsync(profile.Object);
            _repoMock.Setup(repo => repo.FindByEmailAndExcludedIdAsync(dto.Email, profile.Object.Id)).ReturnsAsync((PatientProfile)null);
            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(It.IsAny<long>())).ReturnsAsync((PatientProfile)null);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.UpdateAsync(dto, email: dto.Email);

            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ByEmail_ShouldReturnUpdatedPatientProfile()
        {
            var dto = CreateTestPatientProfileDto();
            var profile = new Mock<PatientProfile>(new FirstName(dto.FirstName), new LastName(dto.LastName), new FullName(dto.FullName), dto.DateOfBirth, new Email(dto.Email), new ContactInformation(dto.ContactInformation));
            _repoMock.Setup(repo => repo.FindByEmailAsync(dto.Email)).ReturnsAsync(profile.Object);
            _repoMock.Setup(repo => repo.FindByEmailAndExcludedIdAsync(dto.Email, profile.Object.Id)).ReturnsAsync((PatientProfile)null);
            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(It.IsAny<long>())).ReturnsAsync((PatientProfile)null);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.UpdateAsync(dto, email: dto.Email);

            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ByMedicalRecordNumber_ShouldReturnUpdatedPatientProfile()
        {
            var dto = CreateTestPatientProfileDto();
            var medicalRecordNumber = "200905114488";
            var profile = new Mock<PatientProfile>(new FirstName(dto.FirstName), new LastName(dto.LastName), new FullName(dto.FullName), dto.DateOfBirth, new Email(dto.Email), new ContactInformation(dto.ContactInformation)) { Object = { MedicalRecordNumber = medicalRecordNumber } };
            _repoMock.Setup(repo => repo.FindByMedicalRecordNumberAsync(medicalRecordNumber)).ReturnsAsync(profile.Object);
            _repoMock.Setup(repo => repo.FindByEmailAndExcludedIdAsync(dto.Email, profile.Object.Id)).ReturnsAsync((PatientProfile)null);
            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(It.IsAny<long>())).ReturnsAsync((PatientProfile)null);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.UpdateAsync(dto, medicalRecordNumber: medicalRecordNumber);

            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ByFullName_ShouldReturnUpdatedPatientProfile()
        {
            var dto = CreateTestPatientProfileDto();
            var fullName = "João Carneiro Sousa";
            var profile = new Mock<PatientProfile>(new FirstName(dto.FirstName), new LastName(dto.LastName), new FullName(fullName), dto.DateOfBirth, new Email(dto.Email), new ContactInformation(dto.ContactInformation));
            _repoMock.Setup(repo => repo.FindByFullNameAsync(fullName)).ReturnsAsync(profile.Object);
            _repoMock.Setup(repo => repo.FindByEmailAndExcludedIdAsync(dto.Email, profile.Object.Id)).ReturnsAsync((PatientProfile)null);
            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(It.IsAny<long>())).ReturnsAsync((PatientProfile)null);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.UpdateAsync(dto, fullName: fullName);

            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ByDateOfBirth_ShouldReturnUpdatedPatientProfile()
        {
            var dto = CreateTestPatientProfileDto();
            var dateOfBirth = DateTime.Now.AddYears(-30).ToString("yyyy-MM-dd");
            var profile = new Mock<PatientProfile>(new FirstName(dto.FirstName), new LastName(dto.LastName), new FullName(dto.FullName), DateTime.Parse(dateOfBirth), new Email(dto.Email), new ContactInformation(dto.ContactInformation));
            _repoMock.Setup(repo => repo.FindByDateOfBirthAsync(DateTime.Parse(dateOfBirth))).ReturnsAsync(profile.Object);
            _repoMock.Setup(repo => repo.FindByEmailAndExcludedIdAsync(dto.Email, profile.Object.Id)).ReturnsAsync((PatientProfile)null);
            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(It.IsAny<long>())).ReturnsAsync((PatientProfile)null);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.UpdateAsync(dto, dateOfBirth: dateOfBirth);

            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
        }
        
        [Fact]
        public async Task UpdateAsyncPatient_ShouldReturnUpdatedPatientProfile()
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
            Assert.Equal(dto.LastName, result.LastName);
            Assert.Equal(dto.FullName, result.FullName);
            Assert.Equal(dto.DateOfBirth, result.DateOfBirth);
            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(dto.ContactInformation, result.ContactInformation);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue()
        {
            var email = "joao.sousa@example.com";
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Now.AddYears(-30), new Email(email), new ContactInformation(966666666));

            _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(profile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.DeleteAsync(email: email);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ByEmail_ShouldReturnTrue()
        {
            var email = "joao.sousa@example.com";
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Now.AddYears(-30), new Email(email), new ContactInformation(966666666));
            _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(profile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.DeleteAsync(email: email);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ByMedicalRecordNumber_ShouldReturnTrue()
        {
            var medicalRecordNumber = "200502110066";
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Now.AddYears(-30), new Email("joao.sousa@example.com"), new ContactInformation(966666666)) { Object = { MedicalRecordNumber = medicalRecordNumber } };
            _repoMock.Setup(repo => repo.FindByMedicalRecordNumberAsync(medicalRecordNumber)).ReturnsAsync(profile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.DeleteAsync(medicalRecordNumber: medicalRecordNumber);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ByFullName_ShouldReturnTrue()
        {
            var fullName = "João Carneiro Sousa";
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName(fullName), DateTime.Now.AddYears(-30), new Email("joao.sousa@example.com"), new ContactInformation(966666666));
            _repoMock.Setup(repo => repo.FindByFullNameAsync(fullName)).ReturnsAsync(profile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.DeleteAsync(fullName: fullName);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ByDateOfBirth_ShouldReturnTrue()
        {
            var dateOfBirth = DateTime.Now.AddYears(-30).ToString("yyyy-MM-dd");
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Parse(dateOfBirth), new Email("joao.sousa@example.com"), new ContactInformation(966666666));
            _repoMock.Setup(repo => repo.FindByDateOfBirthAsync(DateTime.Parse(dateOfBirth))).ReturnsAsync(profile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.DeleteAsync(dateOfBirth: dateOfBirth);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ByPhoneNumber_ShouldReturnTrue()
        {
            var phoneNumber = "966666666";
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Now.AddYears(-30), new Email("joao.sousa@example.com"), new ContactInformation(long.Parse(phoneNumber)));
            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(long.Parse(phoneNumber))).ReturnsAsync(profile.Object);
            _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _service.DeleteAsync(phoneNumber: phoneNumber);

            Assert.True(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPatientProfiles()
        {
            var profiles = new List<Mock<PatientProfile>>
            {
                new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Now.AddYears(-30), new Email("joao.sousa@example.com"), new ContactInformation(966666666)),
                new Mock<PatientProfile>(new FirstName("Jane"), new LastName("Doe"), new FullName("Jane Doe"), DateTime.Now.AddYears(-25), new Email("jane.doe@example.com"), new ContactInformation(933333333))
            };
            _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(profiles.ConvertAll(p => p.Object));

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPatientProfile()
        {
            var id = Guid.NewGuid();
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Now.AddYears(-30), new Email("joao.sousa@example.com"), new ContactInformation(966666666));
            _repoMock.Setup(repo => repo.GetByIdAsync(new PatientProfileId(id))).ReturnsAsync(profile.Object);

            var result = await _service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("João", result.FirstName);
        }

        [Fact]
        public async Task GetAllAsync_ByEmail_ShouldReturnPatientProfile()
        {
            var email = "joao.sousa@example.com";
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Now.AddYears(-30), new Email(email), new ContactInformation(966666666));
            _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(profile.Object);

            var result = await _service.GetAllAsync(email: email);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(email, result[0].Email);
        }

        [Fact]
        public async Task GetAllAsync_ByMedicalRecordNumber_ShouldReturnPatientProfile()
        {
            var medicalRecordNumber = "200602114488";
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Now.AddYears(-30), new Email("joao.sousa@example.com"), new ContactInformation(966666666)) { Object = { MedicalRecordNumber = medicalRecordNumber } };
            _repoMock.Setup(repo => repo.FindByMedicalRecordNumberAsync(medicalRecordNumber)).ReturnsAsync(profile.Object);

            var result = await _service.GetAllAsync(medicalRecordNumber: medicalRecordNumber);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(medicalRecordNumber, result[0].MedicalRecordNumber);
        }

        [Fact]
        public async Task GetAllAsync_ByFullName_ShouldReturnPatientProfile()
        {
            var fullName = "João Carneiro Sousa";
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName(fullName), DateTime.Now.AddYears(-30), new Email("joao.sousa@example.com"), new ContactInformation(966666666));
            _repoMock.Setup(repo => repo.FindByFullNameAsync(fullName)).ReturnsAsync(profile.Object);

            var result = await _service.GetAllAsync(fullName: fullName);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(fullName, result[0].FullName);
        }

        [Fact]
        public async Task GetAllAsync_ByDateOfBirth_ShouldReturnPatientProfile()
        {
            var dateOfBirth = DateTime.Now.AddYears(-30).ToString("yyyy-MM-dd");
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Parse(dateOfBirth), new Email("joao.sousa@example.com"), new ContactInformation(966666666));
            _repoMock.Setup(repo => repo.FindByDateOfBirthAsync(DateTime.Parse(dateOfBirth))).ReturnsAsync(profile.Object);

            var result = await _service.GetAllAsync(dateOfBirth: dateOfBirth);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(dateOfBirth, result[0].DateOfBirth.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public async Task GetAllAsync_ByPhoneNumber_ShouldReturnPatientProfile()
        {
            var phoneNumber = "966666666";
            var profile = new Mock<PatientProfile>(new FirstName("João"), new LastName("Sousa"), new FullName("João Carneiro Sousa"), DateTime.Now.AddYears(-30), new Email("joao.sousa@example.com"), new ContactInformation(long.Parse(phoneNumber)));
            _repoMock.Setup(repo => repo.FindByPhoneNumberAsync(long.Parse(phoneNumber))).ReturnsAsync(profile.Object);

            var result = await _service.GetAllAsync(phoneNumber: phoneNumber);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(phoneNumber, result[0].ContactInformation.ToString());
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

        // [Fact]
        // public async Task ConfirmAndDeleteAccountAsync_ShouldDeleteAccount()
        // {
        //     // Arrange
        //     var email = "luna.sp.1737@gmail.com";
        //     var token = Guid.NewGuid().ToString();
        //     var profile = new Mock<PatientProfile>(new FirstName("Luna"), new LastName("Silva"), new FullName("Luna Silva"), DateTime.Now.AddYears(-30), new Email(email), new ContactInformation(962398532));

        //     _tokenManagerMock.Setup(manager => manager.TryGetToken(token, out email)).Returns(true);
        //     _repoMock.Setup(repo => repo.FindByEmailAsync(email)).ReturnsAsync(profile.Object);
        //     _repoMock.Setup(repo => repo.Remove(profile.Object));
        //     _unitOfWorkMock.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

        //     string capturedToken = null;

        //     _tokenManagerMock.Setup(manager => manager.RemoveToken(It.IsAny<string>()))
        //         .Callback<string>(t => capturedToken = t);

        //     // Act
        //     await _service.ConfirmAndDeleteAccountAsync(token, email);

        //     // Assert
        //     Assert.Equal(token, capturedToken);
        //     _repoMock.Verify(repo => repo.FindByEmailAsync(email), Times.Once);
        //     _repoMock.Verify(repo => repo.Remove(profile.Object), Times.Once);
        //     _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        // }
    }
}