using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExpTrackr.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfig _emailConfig;

        public EmailSender(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Clear();
                emailMessage.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromAddress));

                emailMessage.To.Clear();
                emailMessage.To.Add(new MailboxAddress(email));

                emailMessage.Subject = subject;

                emailMessage.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailConfig.MailServerAddress, int.Parse(_emailConfig.MailServerPort), SecureSocketOptions.Auto)
                        .ConfigureAwait(false);

                    await client.AuthenticateAsync(new NetworkCredential(_emailConfig.UserId, _emailConfig.UserPassword));

                    await client.SendAsync(emailMessage).ConfigureAwait(false);

                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
