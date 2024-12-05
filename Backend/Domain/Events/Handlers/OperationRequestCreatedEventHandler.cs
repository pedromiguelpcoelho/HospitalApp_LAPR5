using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDDSample1.Domain.Events.Handlers
{
    public class OperationRequestCreatedEventHandler
    {
        private readonly ILogger<OperationRequestCreatedEventHandler> _logger;

        public OperationRequestCreatedEventHandler(ILogger<OperationRequestCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(OperationRequestCreatedEvent eventData)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"Operation Request Created: {eventData.OperationRequestId} for Patient: {eventData.PatientId} by Doctor: {eventData.DoctorId} with Operation Type: {eventData.OperationTypeId} at {eventData.CreatedAt}");
            });
        }
    }
}