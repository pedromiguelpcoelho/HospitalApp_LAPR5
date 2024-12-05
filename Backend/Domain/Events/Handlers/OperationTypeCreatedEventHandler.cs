using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDDSample1.Domain.Events.Handlers;

public class OperationTypeCreatedEventHandler
{
    private readonly ILogger<OperationTypeCreatedEventHandler> _logger;

    public OperationTypeCreatedEventHandler(ILogger<OperationTypeCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(OperationTypeCreatedEvent eventData)
    {
        await Task.Run(() => 
        {
            _logger.LogInformation($"Operation Type Created: {eventData.Name} (ID: {eventData.OperationTypeId}) at {eventData.CreatedAt}");
        });
    }
}
