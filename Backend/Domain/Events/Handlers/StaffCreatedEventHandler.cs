using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDDSample1.Domain.Events.Handlers
{
    public class StaffCreatedEventHandler
    {
        private readonly ILogger<StaffCreatedEventHandler> _logger;

        public StaffCreatedEventHandler(ILogger<StaffCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(StaffCreatedEvent eventData)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"Staff Profile Created: {eventData.StaffId} - {eventData.Name} with License Number: {eventData.LicenseNumber}, Specialization: {eventData.Specialization} at {eventData.CreatedAt}");
            });
        }
    }
}
