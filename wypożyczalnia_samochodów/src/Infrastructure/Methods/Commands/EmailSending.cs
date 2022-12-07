using MediatR;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CarRent.src.Application.Requests;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace CarRent.src.Infrastructure.Methods.Commands
{
    public class EmailSending: IRequest<Task>
    {
        public List<int> CarsId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string EmailTo { get; set; }
        public EmailSending(List<int> carsId, string name, string lastName, 
            string address, string postalCode, string phone, string email, DateTime from, DateTime to, string emailTo)
        {
            CarsId = carsId;
            Name = name;
            LastName = lastName;
            Address = address;
            PostalCode = postalCode;
            Phone = phone;
            Email = email;
            From = from;
            To = to;
            EmailTo = emailTo;
        }
    }
    public class EmailSendingHandler : IRequestHandler<EmailSending, Task>
    {
        private readonly IConfiguration _configuration;
        public EmailSendingHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Task> Handle(EmailSending request, CancellationToken cancellationToken)
        {
            var ReservationParamsInfo=request.ToString().ToList();
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
            await Task.Run(() => smtp.Connect(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls));
            await Task.Run(() => smtp.Authenticate(_configuration.GetSection("EmailUserName").Value, _configuration.GetSection("EmailPassword").Value));
            await Task.Run(() => smtp.Send(email));
            await Task.Run(() => smtp.Disconnect(true));
            return Task.CompletedTask;
        }
    }

}
