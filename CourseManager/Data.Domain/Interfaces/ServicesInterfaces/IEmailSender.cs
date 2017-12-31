using System.Threading.Tasks;

namespace Data.Domain.Interfaces.ServicesInterfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
