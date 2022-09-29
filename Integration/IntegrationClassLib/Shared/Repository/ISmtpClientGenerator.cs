using System.Net.Mail;

namespace Integration.Shared.Repository
{
    public interface ISmtpClientGenerator
    {
        ISmtpClient GenerateClient();
    }
}
