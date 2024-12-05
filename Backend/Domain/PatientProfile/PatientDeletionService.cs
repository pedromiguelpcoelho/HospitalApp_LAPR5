using System;
using System.Threading;
using System.Threading.Tasks;
using DDDNetCore.Domain.Email;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class PatientDeletionService : IHostedService, IDisposable
{
    private readonly ILogger<PatientDeletionService> _logger;
    private readonly IServiceProvider _serviceProvider;

    private readonly IEmailService _emailService;
    private Timer _timer;

    public PatientDeletionService(ILogger<PatientDeletionService> logger, IServiceProvider serviceProvider, IEmailService emailService)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _emailService = emailService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Patient Deletion Service is starting.");
        _timer = new Timer(CheckForPatientDeletion, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private async void CheckForPatientDeletion(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var patientRepository = scope.ServiceProvider.GetRequiredService<IPatientProfileRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<PatientProfileService>>();

            var patientsForDeletion = await patientRepository.GetPatientsMarkedForDeletionAsync();

            foreach (var patient in patientsForDeletion)
            {
                if (patient.DeletionScheduledDate <= DateTime.UtcNow)
                {
                    await ConfirmAndDeleteAccountAsync(patient.Id, patientRepository, unitOfWork, logger);
                }
            }
        }
    }

    public async Task ConfirmAndDeleteAccountAsync(PatientProfileId patientId, IPatientProfileRepository patientRepository, IUnitOfWork unitOfWork, ILogger logger)
    {
        var patient = await patientRepository.GetByIdAsync(patientId);
        if (patient == null)
        {
            logger.LogWarning($"Patient with ID {patientId.AsGuid()} not found.");
            return;
        }

        await DeletePatientProfileAsync(patient, patientRepository, unitOfWork, logger);
    }

    private async Task DeletePatientProfileAsync(PatientProfile patient, IPatientProfileRepository patientRepository, IUnitOfWork unitOfWork, ILogger logger)
    {
        patientRepository.Remove(patient);
        await unitOfWork.CommitAsync();
        logger.LogInformation($"Patient profile with ID {patient.Id.AsGuid()} deleted.");

        await _emailService.SendAccountDeletionNotificationEmailAsync(patient.Email.Value);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Patient Deletion Service is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}