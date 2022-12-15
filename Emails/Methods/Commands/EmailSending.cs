using MediatR;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace Emails.Methods.Commands
{
    public class EmailSending: IRequest<Unit>
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
}
