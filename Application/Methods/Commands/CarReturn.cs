using Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Methods.Commands
{
    public record CarReturn(int Id) : IRequest<Unit>;
  
    public class CarReturnHandler : IRequestHandler<CarReturn, Unit>
    {
        private readonly CarRentDbContext _dbContext;
        public CarReturnHandler(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(CarReturn request, CancellationToken cancellationToken)
        {
            var carToReturn = await _dbContext.Rents.FirstOrDefaultAsync(r => r.CarId == request.Id);
            if (carToReturn is null) { throw new Exception(); }
            _dbContext.Rents.Remove(carToReturn);
            await _dbContext.SaveChangesAsync();
            var changeCarStatus = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == request.Id);
            changeCarStatus.IsCar = true;
            await _dbContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
