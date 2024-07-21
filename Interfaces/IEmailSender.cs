using Ava.Data;

namespace Ava.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink);
        Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink);
        Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode);
    }
}
