using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CarRent.src.Domain.Entites;

namespace CarRent.src.Infrastructure.DbContexts.FluentApi
{
    internal class EmployeeBuilder : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasMany(e => e.Customers).WithOne(c => c.Employee).HasForeignKey(c => c.EmployeeId);
            builder.Property(wi => wi.Phone).IsRequired().HasMaxLength(9);
        }
    }
}
