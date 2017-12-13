using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MDAR_AcuityServiceAPI.Utils
{
    public class EmailUtil
    {
        public static void SendLogEmail(String message)
        {
            try
            {
                MailMessage msg = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                StringBuilder sb = new StringBuilder();
                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["hotmailEmail"]);

                smtp.Host = ConfigurationManager.AppSettings["smtpHost"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpSsl"]);
                smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["smtpPort"]);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["hotmailEmail"],
                    ConfigurationManager.AppSettings["emailPass"]);

                sb.Append(message);

                msg.To.Add(ConfigurationManager.AppSettings["hotmailEmail"]);
                msg.From = from;
                msg.Subject = "MDAR_ServiceLog";
                msg.IsBodyHtml = false;
                msg.Body = sb.ToString();
                smtp.Send(msg);
                msg.Dispose();
            }
            catch (Exception e)
            {
                MailMessage msg = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                StringBuilder sb = new StringBuilder();
                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["hotmailEmail"]);

                smtp.Host = ConfigurationManager.AppSettings["smtpHost"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpSsl"]);
                smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["smtpPort"]);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["hotmailEmail"],
                    ConfigurationManager.AppSettings["emailPass"]);

                sb.Append(message);

                msg.To.Add(ConfigurationManager.AppSettings["hotmailEmail"]);
                msg.From = from;
                msg.Subject = "MDAR_ServiceLog";
                msg.IsBodyHtml = false;
                msg.Body = sb.ToString();
                smtp.Send(msg);
                msg.Dispose();
            }
        }

    public static void SendExceptionEmail(Exception exception, String message)
        {
            try
            {
                MailMessage msg = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                StringBuilder sb = new StringBuilder();
                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["hotmailEmail"]);

                smtp.Host = ConfigurationManager.AppSettings["smtpHost"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpSsl"]);
                smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["smtpPort"]);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["hotmailEmail"],
                    ConfigurationManager.AppSettings["emailPass"]);

                
                sb.Append(exception.Message);
                sb.Append(Environment.NewLine);
                sb.Append(exception.StackTrace);
                sb.Append(Environment.NewLine);
                sb.Append(message);

                msg.To.Add(ConfigurationManager.AppSettings["FatalErrorEmailAddress"]);
                msg.From = from;
                msg.Subject = "MDAR_ServiceError";
                msg.IsBodyHtml = false;
                msg.Body = sb.ToString();
                smtp.Send(msg);
                msg.Dispose();
            }
            catch (Exception e)
            {
                MailMessage msg = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                StringBuilder sb = new StringBuilder();
                MailAddress from = new MailAddress(ConfigurationManager.AppSettings["hotmailEmail"]);

                smtp.Host = ConfigurationManager.AppSettings["smtpHost"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpSsl"]);
                smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["smtpPort"]);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["hotmailEmail"],
                    ConfigurationManager.AppSettings["emailPass"]);

                sb.Append(Environment.NewLine);
                sb.Append(exception.Message);
                sb.Append(Environment.NewLine);
                sb.Append(exception.StackTrace);
                sb.Append(message);

                msg.To.Add(ConfigurationManager.AppSettings["FatalErrorEmailAddress"]);
                msg.From = from;
                msg.Subject = "MDAR_ServiceError";
                msg.IsBodyHtml = false;
                msg.Body = sb.ToString();
                smtp.Send(msg);
                msg.Dispose();
            }
        }
    }
}