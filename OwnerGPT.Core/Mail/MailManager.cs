using OwnerGPT.Core.Utilities;
using System;
using System.Net;
using System.Net.Mail;

namespace OwnerGPT.Core.Mail
{
    public class MailManager
    {
        private readonly string SMTP_EMAIL;
        private readonly string SMTP_PASSWORD;

        private readonly string SMTP_SERVER;
        private readonly int SMTP_PORT;

        private readonly string SMTP_SYSTEM_NAME;

        public MailManager() {
            SMTP_SERVER = ConfigurationUtil.GetValue<string>("EMAIL:SERVER");
            SMTP_PORT = ConfigurationUtil.GetValue<int>("EMAIL:PORT");

            SMTP_SYSTEM_NAME = ConfigurationUtil.GetValue<string>("SYSTEM_NAME");

            SMTP_EMAIL = ConfigurationUtil.GetValue<string>("EMAIL:EMAIL");
            SMTP_PASSWORD = ConfigurationUtil.GetValue<string>("EMAIL:PASSWORD");
        }

        public void SendMail(string subject, string content, params string[] toEmails)
        {
            MailMessage message = this.SetupMailMessage();

            message.Subject = subject;
            message.Body = content;

            this.SetMailReceivers(message, toEmails);

            this.SendIt(message);
        }

        private void SendIt(MailMessage mailMessage)
        {
            this.CreateSMTPClient().SendAsync(mailMessage, null);
        }

        private void SetMailReceivers(MailMessage mailMessage, string[] toEmails)
        {
            foreach (string toEmail in toEmails)
            {
                mailMessage.To.Add(new MailAddress(toEmail));
            }
        }

        private MailMessage SetupMailMessage()
        {
            return new MailMessage {
                From = new MailAddress(SMTP_EMAIL, SMTP_SYSTEM_NAME),
                Priority = MailPriority.Normal,
                IsBodyHtml = true,
            };
        }

        private SmtpClient CreateSMTPClient()
        {
            return new SmtpClient {
                Host = SMTP_SERVER,
                Port = SMTP_PORT,
                EnableSsl = false,
                Credentials = new NetworkCredential(SMTP_EMAIL, SMTP_PASSWORD)
            };
        }
    }
}
