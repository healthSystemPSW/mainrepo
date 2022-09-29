using System.Net.Mail;
using System.Threading.Tasks;

namespace Integration.Shared.Repository
{
    public interface ISmtpClient 
    {
        Task SendMailAsync(MailMessage mail);
    }
}
