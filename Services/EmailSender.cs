using System.Net.Mail;
using System.Net;
using Ava.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Ava.Interfaces;

namespace Ava.Services
{
    /// <summary>
    /// Service for sending emails.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The logger instance.</param>
        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="email">The recipient email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The body of the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="SmtpException">Thrown when sending the email fails.</exception>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

            using (var client = new SmtpClient(smtpSettings.Server, smtpSettings.Port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                client.EnableSsl = smtpSettings.UseSsl;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings.SenderEmail, smtpSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                try
                {
                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Email sent to {email}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error sending email to {email}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Sends an email confirmation link asynchronously.
        /// </summary>
        /// <param name="user">The application user.</param>
        /// <param name="email">The recipient email address.</param>
        /// <param name="confirmationLink">The confirmation link.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
            this.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

        /// <summary>
        /// Sends a password reset link asynchronously.
        /// </summary>
        /// <param name="user">The application user.</param>
        /// <param name="email">The recipient email address.</param>
        /// <param name="resetLink">The password reset link.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
            this.SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

        /// <summary>
        /// Sends a password reset code asynchronously.
        /// </summary>
        /// <param name="user">The application user.</param>
        /// <param name="email">The recipient email address.</param>
        /// <param name="resetCode">The password reset code.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
            this.SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");
    }

    /// <summary>
    /// Represents the SMTP settings.
    /// </summary>
    public class SmtpSettings
    {
        /// <summary>
        /// Gets or sets the SMTP server address.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the SMTP server port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the sender name.
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the sender email address.
        /// </summary>
        public string SenderEmail { get; set; }

        /// <summary>
        /// Gets or sets the username for SMTP authentication.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for SMTP authentication.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use SSL for SMTP communication.
        /// </summary>
        public bool UseSsl { get; set; }
    }
}
