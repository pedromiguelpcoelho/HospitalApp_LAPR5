using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDDSample1.Domain.Events.Handlers
{
    public class StaffUpdatedEventHandler
    {
        private readonly ILogger<StaffUpdatedEventHandler> _logger;

        public StaffUpdatedEventHandler(ILogger<StaffUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(StaffUpdatedEvent eventData)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"Staff Profile Updated: {eventData.StaffId} - Name: {eventData.FullName}, Role: {eventData.Role}, " +
                                       $"Specialization: {eventData.Specialization}, Email: {eventData.Email}, " +
                                       $"Phone Number: {eventData.PhoneNumber} at {eventData.UpdatedAt}");
            });
        }
    }
}
