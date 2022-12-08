using MediatR;
using Infrastructure.DbContexts;
using Application.Requests;
using Domain.Entites;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Methods.Commands;
using System.Numerics;

namespace Application.Methods.Commands
{
    public class Reserve : IRequest<Unit>
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
        public Reserve(List<int> carsId, string name, string lastName,
            string address, string postalCode, string phone, string email, DateTime from, DateTime to)
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
        }
    }
    public class ReserveHandler : IRequestHandler<Reserve, Unit>
    {
        private readonly CarRentDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        public ReserveHandler(CarRentDbContext dbContext, IMediator mediator,IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _configuration = configuration;
        }
        public Customer MapCustomer(Reserve reservationParams)
        {
            var customer = new Customer();
            if (reservationParams != null)
            {
                customer.Name = reservationParams.Name;
                customer.LastName = reservationParams.LastName;
                customer.Email = reservationParams.Email;
                customer.Phone = reservationParams.Phone;
                customer.Address = reservationParams.Address;
                customer.PostalCode = reservationParams.PostalCode;
            }
            return customer;
        }
        public Rent MapRent(Reserve reservationParams)
        {
            var rent = new Rent();
            if (reservationParams != null)
            {
                rent.From = reservationParams.From;
                rent.To = reservationParams.To;
            }
            return rent;
        }
        public async Task<Unit> Handle(Reserve request, CancellationToken cancellationToken)
        {
            List<Car> carsToReserve = new List<Car>();
            for (int i = 0; i < request.CarsId.Count; i++)
            {
                carsToReserve.Add(_dbContext.Cars.FirstOrDefault(c => c.Id == request.CarsId[i]));
                if (!carsToReserve[i].IsCar)
                    throw new InvalidOperationException();
            }
            var employee = await _dbContext.Employees.OrderBy(e => e.Customers.Count).FirstOrDefaultAsync();
            var customer = MapCustomer(request);

            customer.EmployeeId = employee.Id;
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
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
            await _dbContext.SaveChangesAsync();
            var EmailtoEmployee=new EmailSending(request.CarsId, request.Name, request.LastName, request.Address, request.PostalCode, request.Phone, request.Email, request.From, request.To, EmployeeEmail);
            await _mediator.Send(EmailtoEmployee);
            var EmailToCustomer= new EmailSending(request.CarsId, request.Name, request.LastName, request.Address, request.PostalCode, request.Phone, request.Email, request.From, request.To, request.Email);
            await _mediator.Send(EmailToCustomer);
            return Unit.Value;
           
        }
    }
}
