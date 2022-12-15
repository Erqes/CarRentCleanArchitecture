using System.Reflection;
using System.Threading;
using Domain.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;

namespace Infrastructure.DbContexts
{
    public class CarRentDbContext : DbContext, ICarRentDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public CarRentDbContext(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }
        public DbSet<CarRental> CarRents { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CarRentDbContextString"));

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.Entity<Employee>(eb =>
            //{
            //    eb.HasMany(e => e.Customers).WithOne(c => c.Employee).HasForeignKey(c => c.EmployeeId);
            //    eb.Property(wi => wi.Phone).IsRequired().HasMaxLength(9);
            //});
            //modelBuilder.Entity<Customer>(eb =>
            //{
            //    eb.HasMany(c => c.Cars).WithMany(cs => cs.Customers).UsingEntity<Rent>(
            //        c => c.HasOne(r => r.Car).WithMany().HasForeignKey(r => r.CarId),
            //        c => c.HasOne(r => r.Customer).WithMany().HasForeignKey(r => r.CustomerId),
            //        r =>
            //        {
            //            r.HasKey(x => new { x.CarId, x.CustomerId });
            //        });
            //});
            //modelBuilder.Entity<Car>(eb =>
            //{
            //    eb.Property(wi => wi.IsCar).HasDefaultValue(true);
            //    eb.Property(wi => wi.Name).IsRequired().HasMaxLength(20);
            //    eb.Property(wi => wi.Combustion).IsRequired();
            //    eb.Property(wi => wi.Localization).IsRequired();
            //    eb.Property(wi => wi.Class).IsRequired();
            //});
            //modelBuilder.Entity<Rent>(eb =>
            //{
            //});
            //modelBuilder.Entity<CarRental>(eb =>
            //{
            //    eb.HasMany(cr => cr.Cars).WithOne(c => c.carRent).HasForeignKey(c => c.CarRentId);
            //    eb.HasMany(cr => cr.Employees).WithOne(e => e.carRental).HasForeignKey(e => e.carRentalId);

            //});



        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
