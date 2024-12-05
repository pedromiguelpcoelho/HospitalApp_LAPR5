using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDDSample1.Domain.Events.Handlers
{
    public class StaffDeletedEventHandler
    {
        private readonly ILogger<StaffDeletedEventHandler> _logger;

        public StaffDeletedEventHandler(ILogger<StaffDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(StaffDeletedEvent eventData)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"Staff Profile Deleted: {eventData.StaffId} - {eventData.Name} with License Number: {eventData.LicenseNumber} at {eventData.DeletedAt}");
            });
        }
    }
}
