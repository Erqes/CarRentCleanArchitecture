using Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface ICarRentDbContext
    {
        DbSet<CarRental> CarRents { get; set; }
        DbSet<Car> Cars { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<Rent> Rents { get; set; }
        DbSet<Role> Roles { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}