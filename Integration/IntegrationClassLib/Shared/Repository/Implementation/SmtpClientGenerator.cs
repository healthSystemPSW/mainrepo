using Integration.Shared.Model;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;

namespace Integration.Shared.Repository.Implementation
{
    class SmtpClientGenerator : ISmtpClientGenerator
    {
        private ISmtpClient _smtpClient;
        private readonly IOptions<EmailConfiguration> _configuration;
        private readonly string _hospitalEmail = "psw.company2@gmail.com";
        private readonly string _emailPassword = Environment.GetEnvironmentVariable("HOSPITAL_EMAIL_PASSWORD");

        public SmtpClientGenerator(IOptions<EmailConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public ISmtpClient GenerateClient()
        {
            _smtpClient = new SmtpClientWrapper()
            {
                Port = _configuration.Value.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration.Value.Username, _configuration.Value.Password),
                EnableSsl = _configuration.Value.EnableSSL,
                Host = _configuration.Value.Host,
            };

            return _smtpClient;
        }

      
    }
}
