using System.Threading.Tasks;

namespace Business.ServicesInterfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
