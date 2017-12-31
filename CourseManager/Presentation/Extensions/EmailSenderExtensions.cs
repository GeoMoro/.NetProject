using System.Threading.Tasks;
using Data.Domain.Interfaces.ServicesInterfaces;
using Presentation.Models.AccountViewModels;

namespace Presentation.Extensions
{
    public static class EmailSenderExtensions
    {
        private const string AssistantConfirmerEmail = "coursemanager.noreply@gmail.com";

        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: {link}");
        }

        public static Task SendAssistantEmailConfirmationAsync(this IEmailSender emailSender, RegisterAssistantViewModel model, string link)
        {
            return emailSender.SendEmailAsync(AssistantConfirmerEmail, $"Confirm {model.FirstName} {model.LastName} email",
                "Assistant details:\n" +
                $"First name: {model.FirstName}\n" +
                $"Last name: {model.LastName}\n" +
                $"Email: {model.Email}\n" +
                $"Secret word: {model.SecretWord}\n\n" +
                $"Please confirm the account by clicking this link: {link}");
        }
    }
}
