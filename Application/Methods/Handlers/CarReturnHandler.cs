using Application.Methods.Commands;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Methods.Handlers
{
    public class CarReturnHandler : IRequestHandler<CarReturn, Unit>
    {
        private readonly ICarRentDbContext _dbContext;
        public CarReturnHandler(ICarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(CarReturn request, CancellationToken cancellationToken)
        {
            var carToReturn = await _dbContext.Rents.FirstOrDefaultAsync(r => r.CarId == request.Id);
            if (carToReturn is null) { throw new Exception(); }
            _dbContext.Rents.Remove(carToReturn);
            await _dbContext.SaveChangesAsync(cancellationToken);
            var changeCarStatus = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == request.Id);
            changeCarStatus.IsCar = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
