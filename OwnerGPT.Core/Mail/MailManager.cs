using OwnerGPT.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OwnerGPT.Core.Mail
{
    public class MailManager
    {
        private readonly string SMTP_EMAIL;
        private readonly string SMTP_USERNAME;
        private readonly string SMTP_PASSWORD;

        private readonly string SMTP_SERVER;
        private readonly int SMTP_PORT;

        private readonly string SMTP_SYSTEM_NAME;

        public MailManager() {
            SMTP_SERVER = ConfigurationUtil.GetValue<string>("EMAIL:SERVER");
            SMTP_PORT = ConfigurationUtil.GetValue<int>("EMAIL:PORT");

            SMTP_SYSTEM_NAME = ConfigurationUtil.GetValue<string>("SYSTEM_NAME");

            SMTP_EMAIL = ConfigurationUtil.GetValue<string>("EMAIL:EMAIL");
            SMTP_USERNAME = ConfigurationUtil.GetValue<string>("EMAIL:USERNAME");
            SMTP_PASSWORD = ConfigurationUtil.GetValue<string>("EMAIL:PASSWORD");
        }

    }
}
