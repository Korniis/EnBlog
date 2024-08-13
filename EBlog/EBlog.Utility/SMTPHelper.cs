using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
namespace EBlog.Utility
{
    public class SMTPHelper
    {
        

        public static SmtpClient CreateSmtpClient()
        {
            return new SmtpClient()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network, //指定电子邮件发送
                Host = @"smtp.qq.com", //smtp服务器
                EnableSsl = true,//
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("532006601@qq.com", "gahpregwgsftbicg"),
            };
        }
        public static async Task<bool> UseSmtpAsync(string mailTo, string mailTitle, string mailContent)
        {
            SmtpClient smtpClient = CreateSmtpClient();
            MailMessage mailMessage = new MailMessage("532006601@qq.com", mailTo);
            mailMessage.Subject = mailTitle;
            mailMessage.Body = mailContent;
            mailMessage.BodyEncoding = Encoding.UTF8;
            
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.Normal;

            try
            {
               await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch { 
           
                return false;
            }

        }

    }
}
