using CarRent.src.Infrastructure.DbContexts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Misc;
using System.Threading;
using System.Threading.Tasks;

namespace CarRent.src.Application.Methods.Commands
{
    public class CarReturn:IRequest<Task>
    {
        public int Id { get; set; }
        public CarReturn(int Id)
        {
            this.Id = Id;
        }
    }
    public class CarReturnHandler : IRequestHandler<CarReturn, Task>
    {
        private readonly CarRentDbContext _dbContext;
        public CarReturnHandler(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Task> Handle(CarReturn request, CancellationToken cancellationToken)
        {
            var carToReturn = await _dbContext.Rents.FirstOrDefaultAsync(r => r.CarId == request.Id);
            if (carToReturn is null) { return null; }
            _dbContext.Rents.Remove(carToReturn);
            await _dbContext.SaveChangesAsync();
            var changeCarStatus = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == request.Id);
            changeCarStatus.IsCar = true;
            await _dbContext.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
