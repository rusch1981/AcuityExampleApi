using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace MDAR_AcuityServiceAPI.Utils
{
    public class EmailUtil
    {
        public static void SendEmail(Exception exception)
        {
            var email = ConfigurationManager.AppSettings["hotmailEmail"];
            var pass = ConfigurationManager.AppSettings["emailPass"];

            var mail = new MailMessage {From = new MailAddress(email)};
            mail.To.Add(ConfigurationManager.AppSettings["FatalErrorEmailAddress"]);
            mail.Subject = "Acuity Service Fatal Error Alert";
            mail.Body = exception.ToString();

            var smtpServer = new SmtpClient("smtp.live.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(email, pass),
                EnableSsl = true
            };
            smtpServer.Send(mail);
        }
    }
}