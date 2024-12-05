using System.Threading.Tasks;
using DDDSample1.Domain.Events;
using Microsoft.Extensions.Logging;

public class OperationRequestDeletedEventHandler
{
    private readonly ILogger<OperationRequestDeletedEventHandler> _logger;

    public OperationRequestDeletedEventHandler(ILogger<OperationRequestDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(OperationRequestDeletedEvent eventData)
    {
        await Task.Run(() =>
        {
            _logger.LogInformation($"Operation Request Deleted: {eventData.OperationRequestId} at {eventData.DeletedAt}");
        });
    }
}