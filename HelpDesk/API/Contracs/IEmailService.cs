using API.Utility;

namespace API.Contracs
{
    public interface IEmailService
    {
        void SendEmailAsync();
        EmailService SetEmail(string email);
        EmailService SetSubject(string subject);
        EmailService SetHtmlMessage(string htmlMessage);
    }
}
