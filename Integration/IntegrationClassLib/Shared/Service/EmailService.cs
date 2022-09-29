using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Integration.Pharmacies.Model;
using Integration.Shared.Repository;
using Integration.Tendering.Model;

namespace Integration.Shared.Service
{
    public class EmailService
    {
        private readonly string _hospitalName = "Nasa bolnica";
        private readonly string _hospitalEmail = "psw.company2@gmail.com";
        private readonly ISmtpClientGenerator _smtpClientGenerator;

        public EmailService()
        {
        }

        public EmailService(ISmtpClientGenerator smtpClient)
        {
            _smtpClientGenerator = smtpClient;
        }

        public void SendNewTenderMail(Tender tender, IEnumerable<Pharmacy> pharmacies)
        {
            var mail = PrepareMailNames(pharmacies);
            string text = "<p>Greetings</p><p>We have created a new tender named " + tender.Name
                + ". This tender requires following medications:";
            text = MakeMedicationList(tender.MedicationRequests, text);
            text += "</p>";
            text = MakeRegardsText(text);
            //SendMail(mail, "New Tender", text);
        }

        public void SendWinningOfferMail(Tender tender)
        {
            var mail = tender.WinningOffer.Pharmacy.Email;
            string text = "<p>Greetings</p><p>We inform you that your tender offer for tender named " +
                          tender.Name +
                          " has won. We are looking forward to hearing from you about medication you promised in that offer. Promised medications are listed below:";
            text = MakeMedicationList(tender.WinningOffer.MedicationRequests, text);
            text += "</p>";
            text = MakeRegardsText(text);
            //SendMail(mail, "Winning tender offer", text);
        }

        public void SendCloseTenderMail(Tender tender, IEnumerable<Pharmacy> pharmacies)
        {
            var mail = PrepareMailNames(pharmacies);
            string text = "<p>Greetings</p><p>We inform you that our tender named " +
                          tender.Name + " has been closed.</p>";
            text = MakeRegardsText(text);
           // SendMail(mail, "Tender closed", text);
        }

        private string MakeRegardsText(string text)
        {
            text += "<p>Best regards,</p><p>" + _hospitalName + "</p>";
            return text;
        }

        private static string MakeMedicationList(IEnumerable<MedicationRequest> medicationRequests, string text)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(text);
            builder.Append("<ul>");
            foreach (var medReq in medicationRequests)
            {
                builder.Append("<li>");
                builder.Append(medReq.MedicineName);
                builder.Append(", Amount: ");
                builder.Append(medReq.Quantity);
                builder.Append("</li>");
            }
            builder.Append("</ul>");
            return builder.ToString();
        }

        private static string PrepareMailNames(IEnumerable<Pharmacy> pharmacies)
        {
            StringBuilder builder = new();
            foreach (string pharmacyMail in pharmacies.Select(x => x.Email))
            {
                builder.Append(pharmacyMail);
                builder.Append(',');
            }

            string mail = builder.ToString();
            mail = mail.Remove(mail.Length - 1);
            return mail;
        }


        public async Task SendMail(string emailTo, string title, string text)
        {
            MailMessage mailMessage = new(_hospitalEmail, emailTo) { 
                IsBodyHtml = true,
                Subject = title,
                Body = text,
            };
            var client = _smtpClientGenerator.GenerateClient();
            
            try
            {
               await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message not send error: { " + ex.Message + "}");

            }
        }
    }
}
