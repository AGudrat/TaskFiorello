using System.Net.Mail;
using System.Threading.Tasks;

namespace Fiorello.Services
{
    public interface IEmailSender
    {
        Task SendSmsAsync(string number, string message);
        Task SendEmailAsycn(MailMessage msg);
    }
}