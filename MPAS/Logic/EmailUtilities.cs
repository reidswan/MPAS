using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace MPAS.Logic
{
    public class EmailUtilities
    {
        public static void Email(string emailAddress, string subject, string content)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("mpasUCT@gmail.com");
                mail.To.Add(emailAddress);
                mail.Subject = subject;
                mail.Body = content;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("mpasUCT@gmail.com", "michaellarryreid");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("Email sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email not sent");
            }
        }

        public static void EmailMultiple(string[] addresses, string subject, string content)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("mpasUCT@gmail.com");
                foreach(string emailAddress in addresses) mail.To.Add(emailAddress);
                mail.Subject = subject;
                mail.Body = content;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("mpasUCT@gmail.com", "michaellarryreid");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                // todo: show mail-sending exception
            }
        }
    }
}