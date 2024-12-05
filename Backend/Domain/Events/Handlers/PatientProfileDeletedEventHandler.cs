using System.Threading.Tasks;
using DDDNetCore.Domain.Email;
using Microsoft.Extensions.Logging;

namespace DDDSample1.Domain.Events.Handlers
{
    public class PatientProfileDeletedEventHandler
    {
        private readonly ILogger<PatientProfileDeletedEventHandler> _logger;

        public PatientProfileDeletedEventHandler(ILogger<PatientProfileDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(PatientProfileDeletedEvent eventData)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"Patient account with ID {eventData.PatientId} has been deleted on {eventData.DeletedAt}.");
                _logger.LogInformation($"Patient Profile Deleted: {eventData.PatientId} for Patient {eventData.FullName} with the email {eventData.Email} at {eventData.DeletedAt}");
            });
        }
    }
}