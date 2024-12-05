using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDDSample1.Domain.Events.Handlers
{
    public class PatientProfileUpdatedEventHandler
    {
        private readonly ILogger<PatientProfileUpdatedEventHandler> _logger;

        public PatientProfileUpdatedEventHandler(ILogger<PatientProfileUpdatedEventHandler> logger) {
            _logger = logger;
        }

        public async Task Handle(PatientProfileUpdatedEvent eventData)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"Patient Profile Updated: {eventData.PatientId} for Patient {eventData.FullName} with the email {eventData.Email} at {eventData.UpdatedAt}");
            });
        }
    }
}