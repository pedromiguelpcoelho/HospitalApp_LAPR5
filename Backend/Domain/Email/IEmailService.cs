using System;
using System.Threading.Tasks;

namespace DDDNetCore.Domain.Email;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendAccountDeletionConfirmationEmailAsync(string email, string token);
    Task SendAccountDeletionNotificationEmailAsync(string email);
    Task SendProfileUpdateEmailAsync(string firstName, string lastName, string email, string changes);
    Task SendStaffUpdateEmailAsync(string firstName, string lastName, string email,string changes);
    Task SendAccountDeletionScheduledEmailAsync(string email, DateTime deletionDate);

}