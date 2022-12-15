using Domain.Entites;
using Infrastructure.DbContexts;
using Infrastructure.Methods.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Methods.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUser, Unit>
    {
        private readonly CarRentDbContext _dbContext;
        private readonly IPasswordHasher<Customer> _passwordHasher;
        private readonly IPasswordHasher<Employee> _passwordHasherEmployee;

        public RegisterUserHandler(CarRentDbContext dbContext, IPasswordHasher<Customer> passwordHasher, IPasswordHasher<Employee> passwordHasherEmployee)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _passwordHasherEmployee = passwordHasherEmployee;
        }

        public async Task<Unit> Handle(RegisterUser request, CancellationToken cancellationToken)
        {
            if (request.RoleId != 1)
            {
                var newUser = new Employee()
                {
                    Email = request.Email,
                    RoleId = request.RoleId,
                    Name = request.Name,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    Address = request.Address,
                    City = request.City,
                    PostalCode = request.PostalCode,

                };
                
                var hashedPassword = _passwordHasherEmployee.HashPassword(newUser, request.Password);
                newUser.PasswordHash = hashedPassword;
                await _dbContext.Employees.AddAsync(newUser);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            else
            {

                var newUser = new Customer()
                {
                    Email = request.Email,
                    RoleId = request.RoleId,
                    Name = request.Name,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    Address = request.Address,
                    City = request.City,
                    PostalCode = request.PostalCode,

                };
                var employee = await _dbContext.Employees.OrderBy(e => e.Customers.Count).FirstOrDefaultAsync();
                newUser.EmployeeId = employee.Id;
                var hashedPassword = _passwordHasher.HashPassword(newUser, request.Password);
                newUser.PasswordHash = hashedPassword;
                await _dbContext.Customers.AddAsync(newUser);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            
        }
    }
}
