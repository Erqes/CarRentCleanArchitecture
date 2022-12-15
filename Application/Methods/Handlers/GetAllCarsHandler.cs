using Application.Methods.Queries;
using Application.Models;
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
    public class GetAllCarsHandler : IRequestHandler<GetAllCars, IEnumerable<CarDto>>
    {
        private readonly ICarRentDbContext _dbContext;
        public GetAllCarsHandler(ICarRentDbContext dbContext)
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
