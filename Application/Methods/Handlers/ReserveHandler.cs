using Application.Methods.Commands;
using Domain.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emails.Methods.Commands;
using Application.Interfaces;
namespace Application.Methods.Handlers
{
    public class ReserveHandler : IRequestHandler<Reserve, Unit>
    {
        private readonly ICarRentDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        public ReserveHandler(ICarRentDbContext dbContext, IMediator mediator, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _configuration = configuration;
        }
        public Rent MapRent(Reserve reservationParams)
        {
            var rent = new Rent();
            if (reservationParams != null)
            {
                rent.From = reservationParams.from;
                rent.To = reservationParams.to;
            }
            return rent;
        }
        public async Task<Unit> Handle(Reserve request, CancellationToken cancellationToken)
        {
            List<Car> carsToReserve = new List<Car>();
            for (int i = 0; i < request.carsId.Count; i++)
            {
                carsToReserve.Add(_dbContext.Cars.FirstOrDefault(c => c.Id == request.carsId[i]));
                if (!carsToReserve[i].IsCar)
                    throw new InvalidOperationException();
            }
            var employee = await _dbContext.Employees.OrderBy(e => e.Customers.Count).FirstOrDefaultAsync();
            
            var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == request.customerId);
            List<Rent> NewRents = new List<Rent>();
            for (int i = 0; i < carsToReserve.Count; i++)
            {

                carsToReserve[i].IsCar = false;
                NewRents.Add(MapRent(request));
                await _dbContext.Rents.AddAsync(NewRents[i]);
                NewRents[i].CarId = carsToReserve[i].Id;
                NewRents[i].CustomerId = customer.Id;
            }
            var EmployeeEmail = _configuration.GetSection("EmailUserName").Value;
            await _dbContext.SaveChangesAsync(cancellationToken);
            var EmailtoEmployee = new EmailSending(request.carsId, customer.Name, customer.LastName, customer.Address, customer.PostalCode, customer.Phone, customer.Email, request.from, request.to, EmployeeEmail);
            await _mediator.Send(EmailtoEmployee);
            var EmailToCustomer = new EmailSending(request.carsId, customer.Name, customer.LastName, customer.Address, customer.PostalCode, customer.Phone, customer.Email, request.from, request.to, customer.Email);
            await _mediator.Send(EmailToCustomer);
            return Unit.Value;

        }
    }
}
