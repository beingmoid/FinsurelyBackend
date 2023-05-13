using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using static AIB.Common.Constants;

namespace AIB.Common.EmailService
{
 

    public class Mailer : IMailer
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment  _env;

        public Mailer(IOptions<SmtpSettings> smtpSettings, IHostingEnvironment env)
        {
            _smtpSettings = smtpSettings.Value;
            _env = env;
        }

        public async Task<bool> SendEmailAsync(string name,string email, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress(name,email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    if (_env.IsDevelopment())
                    {
                        await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, true);
                    }
                    else
                    {
                        await client.ConnectAsync(_smtpSettings.Server);
                    }

                    await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    return await Task.FromResult(true);
                }
            }
            catch (Exception e)
            {
                string m =(e.Message);
                OtherConstants.responseMsg = m+"__"+e.Source;
                OtherConstants.isSuccessful = false;
                
                
            }
            return await Task.FromResult(false);
        }

    }
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public interface IMailer
    {
        Task<bool> SendEmailAsync(string name, string email, string subject, string body);
    }
}
