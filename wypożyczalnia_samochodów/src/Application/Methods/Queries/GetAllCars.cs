using CarRent.src.Application.Models;
using CarRent.src.Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarRent.src.Application.Methods.Queries
{
    public class GetAllCars : IRequest<IEnumerable<CarDto>>
    {
    }
    public class GetAllCarsHandler : IRequestHandler<GetAllCars, IEnumerable<CarDto>>
    {
        private readonly CarRentDbContext _dbContext;
        public GetAllCarsHandler(IMediator mediator, CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<CarDto>> Handle(GetAllCars request, CancellationToken cancellationToken)
        {
            var cars = await _dbContext.Cars.ToListAsync();
            if (cars is null)
            {
                return null;
            }
            var carsDtos = cars.Select(c => new CarDto()
            {
                Id = c.Id,
                Name = c.Name,
                Class = c.Class,
                Color = c.Color,
                Combustion = c.Combustion,
                Localization = c.Localization,
                HorsePower = c.HorsePower,
                Price = c.Price,
            });
            return carsDtos;
        }
    }

}
