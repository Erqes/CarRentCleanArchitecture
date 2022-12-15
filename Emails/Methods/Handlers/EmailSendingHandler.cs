using Emails.Methods.Commands;
using MailKit.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace Emails.Methods.Handlers
{
    public class EmailSendingHandler : IRequestHandler<EmailSending, Unit>
    {
        private readonly IConfiguration _configuration;
        public EmailSendingHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Unit> Handle(EmailSending request, CancellationToken cancellationToken)
        {
            var ReservationParamsInfo = request.ToString().ToList();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(request.EmailTo));
            if (email.From == email.To)
            {
                email.Subject = $"Rezerwacja od {request.Email}";
                email.Body = new TextPart(TextFormat.Html) { Text = $"{ReservationParamsInfo}" };
            }
            else
            {
                email.Subject = $"Rezerwacja Car Rental dokonana.";
                email.Body = new TextPart(TextFormat.Html) { Text = $"Dokonano rezerwacji samochodu." };
            }
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration.GetSection("EmailUserName").Value, _configuration.GetSection("EmailPassword").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            return Unit.Value;
        }
    }
}
