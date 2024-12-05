using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDDSample1.Domain.Events.Handlers
{
    public class OperationRequestUpdatedEventHandler
    {
        private readonly ILogger<OperationRequestUpdatedEventHandler> _logger;

        public OperationRequestUpdatedEventHandler(ILogger<OperationRequestUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(OperationRequestUpdatedEvent eventData)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"Operation Request Updated: {eventData.OperationRequestId}, Priority: {eventData.OriginalPriority} -> {eventData.UpdatedPriority}, Deadline: {eventData.OriginalDeadline} -> {eventData.UpdatedDeadline} at {eventData.UpdatedAt}");
            });
        }
    }
}