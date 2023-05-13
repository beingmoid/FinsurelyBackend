using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;
using static AIB.Common.Constants;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace AIB.Common.EmailService
{
    public class MailKitService:IEmailService
    {
      



        public bool SendEmailWithoutTemplate(string ToEmail, string Name, string subject ,string body, bool isBodyHtml = false)
        {
            try
            {


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("NukesLab LLC", "support@nukeslab.com"));
                message.To.Add(new MailboxAddress($"${Name}", ToEmail));
                message.Subject = "Email Verification Notification From NukesLab LLC";
                message.Body = new TextPart("html");
                var textPart = new TextPart("html");
                if (isBodyHtml)
                {
                    textPart.Text = body;
                    message.Body = textPart;


                }
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.titan.email", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("support@nukeslab.com", "Server@1234");

                    client.Send(message);
                    client.Disconnect(true);

                    return true;
                }
            }

            catch (Exception ex)
            {

                return false;
            }
        }
    }
}