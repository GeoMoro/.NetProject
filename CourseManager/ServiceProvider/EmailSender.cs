using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Business.ServicesInterfaces;

namespace ServicesProvider
{
    public class EmailSender : IEmailSender
    {
        private const string Username = "coursemanager.noreply@gmail.com";
        private const string Password = ".netproject";

        // For another server provider search for the proper smtp
        private const string Server = "smtp.gmail.com";

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = new MailMessage(Username, email, subject, message);

            var client = new SmtpClient(Server)
            {
                Port = 587, // 465, 587 these are not random ports, search for them too acording to the server
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = true,
            };

            return client.SendMailAsync(mail);
        }
    }
}
