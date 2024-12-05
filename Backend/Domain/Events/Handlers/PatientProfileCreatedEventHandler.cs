using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDDSample1.Domain.Events.Handlers
{
    public class PatientProfileCreatedEventHandler
    {
        private readonly ILogger<PatientProfileCreatedEventHandler> _logger;

        public PatientProfileCreatedEventHandler(ILogger<PatientProfileCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(PatientProfileCreadtedEvent eventData)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"Patient Profile Created: {eventData.PatientId} for Patient {eventData.FullName} with the email {eventData.Email} at {eventData.CreatedAt}");
            });
        }
    }
}