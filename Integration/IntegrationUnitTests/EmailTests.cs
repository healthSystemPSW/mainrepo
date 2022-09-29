using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Integration.Pharmacies.Model;
using Integration.Shared.Model;
using Integration.Shared.Repository;
using Integration.Shared.Service;
using Integration.Tendering.Model;
using IntegrationUnitTests.Base;
using Moq;
using Xunit;

namespace IntegrationUnitTests
{
    public class EmailTests : BaseTest
    {
        private readonly string _hospitalEmail = "psw.company2@gmail.com";
        public EmailTests(BaseFixture fixture) : base(fixture)
        {
            ClearDbContext();
        }

        [SkippableFact]
        public void Send_email_success()
        {
            var env = Environment.GetEnvironmentVariable("PRODUCTION");
            Skip.If(env == null || env.Equals("1"));
            var eService = new EmailService();
            bool exceptionCaught = false;
            try
            {
               eService.SendMail("psw.company2.pharmacy@gmail.com,psw.company2@gmail.com", "testTitle", "testText");
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                exceptionCaught = true;
            }
            Assert.False(exceptionCaught);
        }
        [Fact]
        public void Send_email_verify()
        {
           /* //Arrange
            var mock = new Mock<ISmtpClient>();
            EmailService emailService = new(mock.Object);

            //Act
            emailService.SendMail("psw.company2.pharmacy@gmail.com,psw.company2@gmail.com", "testTitle", "testText");
           
            //Assert
            mock.Verify(t => t.SendMail(It.Is<MailMessage>(s => s.Subject.Equals("testTitle"))));
            mock.Verify(t => t.SendMail(It.Is<MailMessage>(s => s.From.Address.Equals(_hospitalEmail))));
           */
        }
        [Fact]
        public async Task SendEmailAsync_WithCorrectParams_ShouldReturnTaskComplete()
        {
            //Arrange
            var _mockSmtpClientGenerator = new Mock<ISmtpClientGenerator> ();
            var _mockSmtpClient = new Mock<ISmtpClient>();

            var emailFrom = _hospitalEmail;
            var emailTo = "psw.company2.pharmacy@gmail.com,psw.company2@gmail.com";
            var subject = "testSubject";
            var message = "testText";

            _mockSmtpClientGenerator.Setup(generator => generator.GenerateClient())
            .Returns(_mockSmtpClient.Object);
           
            EmailService emailService = new(_mockSmtpClientGenerator.Object);

            // Act
            var result = emailService.SendMail(
                emailTo,
                subject,
                message);

            await result;

            // Assert
            Assert.True(result.IsCompletedSuccessfully);
            _mockSmtpClient.Verify(t => t.SendMailAsync(It.Is<MailMessage>(s => s.Subject.Equals("testSubject"))), Times.Once);
            _mockSmtpClient.Verify(t => t.SendMailAsync(It.Is<MailMessage>(s => s.From.Address.Equals(emailFrom))));

        }

            [SkippableFact]
        public void Send_new_tender_email()
        {
            var env = Environment.GetEnvironmentVariable("PRODUCTION");
            Skip.If(env == null || env.Equals("1"));
            List<Pharmacy> pharmacies = new List<Pharmacy>
            {
                new Pharmacy
                {
                    Email = "psw.company2.pharmacy@gmail.com"
                },
                new Pharmacy
                {
                    Email = "psw.company2@gmail.com"
                }
            };
            Tender tender = new("NEW_TENDER_EMAIL_TEST",
                new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1)));
            tender.AddMedicationRequest(new MedicationRequest("Aspirin", 5));
            tender.AddMedicationRequest(new MedicationRequest("Brufen", 5));
            bool exceptionCaught = false;
            try
            {
                new EmailService().SendNewTenderMail(tender, pharmacies);
            }
            catch
            {
                exceptionCaught = true;
            }
            Assert.False(exceptionCaught);
        }

        [SkippableFact]
        public void Send_winning_offer_email()
        {
            var env = Environment.GetEnvironmentVariable("PRODUCTION");
            Skip.If(env == null || env.Equals("1"));
            var pharmacy = new Pharmacy()
            {
                Email = "psw.company2.pharmacy@gmail.com"
            };
            Tender tender = new Tender("TENDER_OFFER_WON_EMAIL_TEST",
                new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1)));
            tender.AddMedicationRequest(new MedicationRequest("Aspirin", 5));
            tender.AddMedicationRequest(new MedicationRequest("Brufen", 5));
            TenderOffer offer = new TenderOffer(pharmacy, new Money(50, 0), DateTime.Now);
            offer.AddMedicationRequest(new MedicationRequest("Aspirin", 5));
            offer.AddMedicationRequest(new MedicationRequest("Brufen", 5));
            tender.AddTenderOffer(offer);
            tender.ChooseWinner(offer);
            bool exceptionCaught = false;
            try
            {
                new EmailService().SendWinningOfferMail(tender);
            }
            catch
            {
                exceptionCaught = true;
            }
            Assert.False(exceptionCaught);
        }

        [SkippableFact]
        public void Send_tender_closed_mail()
        {
            var env = Environment.GetEnvironmentVariable("PRODUCTION");
            Skip.If(env == null || env.Equals("1"));
            List<Pharmacy> pharmacies = new List<Pharmacy>();
            pharmacies.Add(new Pharmacy
            {
                Email = "psw.company2.pharmacy@gmail.com"
            });
            pharmacies.Add(new Pharmacy
            {
                Email = "psw.company2@gmail.com"
            });
            Tender tender = new Tender("TENDER_OFFER_WON_EMAIL_TEST",
                new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1)));
            tender.AddMedicationRequest(new MedicationRequest("Aspirin", 5));
            tender.AddMedicationRequest(new MedicationRequest("Brufen", 5));
            tender.CloseTender();
            bool exceptionCaught = false;
            try
            {
                new EmailService().SendCloseTenderMail(tender, pharmacies);
            }
            catch
            {
                exceptionCaught = true;
            }
            Assert.False(exceptionCaught);
        }
    }
}
