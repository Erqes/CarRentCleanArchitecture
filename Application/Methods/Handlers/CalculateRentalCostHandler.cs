using Application.Methods.Queries;
using Domain.Entites;
using Domain.Enums;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Methods.Handlers
{
    public class CalulateRentalCostHandler : IRequestHandler<CalculateRentalCost, string>
    {
        private readonly ICarRentDbContext _dbContext;
        public CalulateRentalCostHandler(ICarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Car> GetType(CarType carClass)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Class == carClass.ToString());
            if (car is null)
                throw new InvalidEnumArgumentException();
            else
                return car;

        }
        public async Task<int> GetCount()
        {

            return await _dbContext.Cars.CountAsync();
        }
        public async Task<string> Handle(CalculateRentalCost request, CancellationToken cancellationToken)
        {
            var fuelPrice = 8;
            var lendPrice = 100;
            var car = await GetType(request.CarClass);
            var CarsCountInRental = await GetCount();
            var HowLongDriveLincense = DateTime.Now - request.DriveLicense;
            var floatHowLongDriveLicense = (float)HowLongDriveLincense.TotalDays;
            var RentTimeSpan = request.To - request.From;
            var howManyDays = RentTimeSpan.TotalDays;
            var floatDays = (float)howManyDays;//ilosc dni w int 
            var result = car.Combustion * request.Km / 100 * fuelPrice + lendPrice * floatDays;

            var res = (int)request.CarClass switch
            {
                0 => result = result,
                10 => result = result * 1.3f,
                20 => result = result * 1.6f,
                30 => result = result * 2,
                _ => throw new InvalidEnumArgumentException()
            };
            float resultYears, resultCount;
            //jeśli prawo jazdy mniej niż 5 lat 
            if (5 * 365 > floatHowLongDriveLicense)
                resultYears = result + result * 0.2f;
            else
                resultYears = 0;
            //jesli aut jest mniej niż 3 
            if (CarsCountInRental < 3)
                resultCount = result + result * 0.15f;
            else
                resultCount = 0;
            //jeśli prawo jazdy mniej niż 3 lata i klasa Premium
            if (3 * 365 > floatHowLongDriveLicense && request.CarClass.ToString() == "Premium")
                return "Nie można wypożyczyć samochodu";

            result = result + resultCount + resultYears;

            return $"Cena netto: {result}, Cena brutto: {result + result * 0.23}, Cena Całkowita= (Cena Wypożyczenia howManyDays ilość dni + koszt paliwa) howManyDays klasa samochodu" +
            $" + Koszt(jesli prawo jazdy posiadane jest mniej niż 5 lat) + Koszt(jeśli aut jest dostępnych mniej niż 3)=" +
            $"({100}x{floatDays}+{car.Combustion * request.Km / 100 * fuelPrice}) x {request.CarClass} + {resultYears} + {resultCount}";
        }

    }


}

