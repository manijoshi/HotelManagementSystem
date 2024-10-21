
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HotelBookingSystem.Infrastructure.EmailSender
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _smtpClient = new SmtpClient
            {
                Host = configuration["SmtpSettings:Server"],
                Port = int.Parse(configuration["SmtpSettings:Port"]),
                EnableSsl = false 
            };

            _fromEmail = configuration["SmtpSettings:SenderEmail"];
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var mailMessage = new MailMessage(_fromEmail, to, subject, body) { IsBodyHtml = true };
                await _smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully to {Email}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", to);
                throw;
            }
        }
    }

}