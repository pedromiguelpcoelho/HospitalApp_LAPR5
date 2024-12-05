using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DDDNetCore.Domain.Email;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // Configuração do SMTP para Gmail
        var smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("lapr5.g44.3dh@gmail.com", "mzzy ipub iitc ruxc"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("lapr5.g44.3dh@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(to);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public string CreateProfileUpdateEmailForPatient(string firstName, string lastName, string changesSummary)
    {
        string body = $@"
        <p>Dear {firstName} {lastName},</p>
        <p>We are pleased to inform you that your patient profile information has been successfully updated.</p>
        <p>{changesSummary}</p>
        <p>If you have any questions or need further assistance, please do not hesitate to reach out to us.</p>
        <p>Thank you for choosing our healthcare services.</p>
        <p>Best regards,<br/>The Wonderful Hospital Team</p>";

        return body;
    }

    public async Task SendAccountDeletionConfirmationEmailAsync(string email, string token)
    {
        string subject = "Confirm Account Deletion";
        string body = $@"
        <p>Dear Patient,</p>
        <p>We have received a request to delete your account. If you did not make this request, please ignore this email.</p>
        <p>To confirm the deletion of your account, please use the following token:</p>
        <p><strong>{token}</strong></p>
        <p>This token is valid for 1 hour.</p>
        <p>Best regards,<br/>The Wonderful Hospital Team</p>";
        await SendEmailAsync(email, subject, body);
    }

    public async Task SendAccountDeletionNotificationEmailAsync(string email)
    {
        string subject = "Account Deletion Completed";
        string body = $@"
        <p>Dear Patient,</p>
        <p>We would like to inform you that your account has been successfully deleted from our system.</p>
        <p>If you have any questions or need further assistance, please do not hesitate to reach out to us.</p>
        <p>Best regards,<br/>The Wonderful Hospital Team</p>";
        await SendEmailAsync(email, subject, body);
    }

    public async Task SendProfileUpdateEmailAsync(string firstName, string lastName, string email, string changesSummary)
    {
        string subject = "Patient Profile Update Notification";
        string body = $@"
        <p>Dear {firstName} {lastName},</p>
        <p>We are pleased to inform you that your patient profile information has been successfully updated.</p>
        <p>{changesSummary}</p>
        <p>If you have any questions or need further assistance, please do not hesitate to reach out to us.</p>
        <p>Thank you for choosing our healthcare services.</p>
        <p>Best regards,<br/>The Wonderful Hospital Team</p>";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendStaffUpdateEmailAsync(string firstName, string lastName, string email, string changesSummary)
    {
        string subject = "Staff Update Notification";
        string body = $@"
            <p>Dear {firstName} {lastName},</p>
            <p>We are pleased to inform you that your staff profile information has been successfully updated.</p>
            <p><strong>Here are the details of the updates:</strong></p>
            <p>{changesSummary}</p>
            <p>If you have any questions or need further assistance, please do not hesitate to reach out to us.</p>
            <p>Thank you for choosing our healthcare services.</p>
            <p>Best regards,<br/>The Wonderful Hospital Team</p>";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendAccountDeletionScheduledEmailAsync(string email, DateTime deletionDate)
    {
        string subject = "Account Deletion Scheduled";
        string body = $@"
        <p>Dear Patient,</p>
        <p>We would like to inform you that your account has been scheduled for deletion on {deletionDate.ToString("f")}.</p>
        <p>If you have any questions or need further assistance, please do not hesitate to reach out to us.</p>
        <p>Best regards,<br/>The Wonderful Hospital Team</p>";
        await SendEmailAsync(email, subject, body);
    }

}