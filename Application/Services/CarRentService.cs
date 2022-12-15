﻿using Domain.Entites;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Application.Requests;
using Application.Interfaces;
using Application.Models;

namespace Application.Services
{
    public interface ICarRentService
    {
        Task<string> CountBy(LendParams lendParams);
        Task<IEnumerable<CarDto>> GetAll();
        //IEnumerable<CarDto> GetByParams(CarParams carParams);
    }

    public class CarRentService : ICarRentService
    {

        private readonly ICarRentDbContext _dbContext;
        public CarRentService(ICarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Car> GetType(CarType carClass)
        {
            return await _dbContext.Cars.FirstOrDefaultAsync(c => c.Class == carClass.ToString());
        }
        public async Task<int> GetCount()
        {

            return await _dbContext.Cars.CountAsync();
        }
        public async Task<string> CountBy(LendParams lendParams)
        {
            var fuelPrice = 8;
            var lendPrice = 100;
            var car =await GetType(lendParams.CarClass);
            var CarsCountInRental =await GetCount();
            var HowLongDriveLincense = DateTime.Now - lendParams.DriveLicense;
            var floatHowLongDriveLicense = (float)HowLongDriveLincense.TotalDays;
            var RentTimeSpan = lendParams.To - lendParams.From;
            var howManyDays = RentTimeSpan.TotalDays;
            var floatDays = (float)howManyDays;//ilosc dni w int 
            var result = car.Combustion * lendParams.Km / 100 * fuelPrice + lendPrice * floatDays;

            var res = (int)lendParams.CarClass switch
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
            if (3 * 365 > floatHowLongDriveLicense && lendParams.CarClass.ToString() == "Premium")
                return "Nie można wypożyczyć samochodu";

            result = result + resultCount + resultYears;

            return $"Cena netto: {result}, Cena brutto: {result + result * 0.23}, Cena Całkowita= (Cena Wypożyczenia howManyDays ilość dni + koszt paliwa) howManyDays klasa samochodu" +
            $" + Koszt(jesli prawo jazdy posiadane jest mniej niż 5 lat) + Koszt(jeśli aut jest dostępnych mniej niż 3)=" +
            $"({100}x{floatDays}+{car.Combustion * lendParams.Km / 100 * fuelPrice}) x {lendParams.CarClass} + {resultYears} + {resultCount}";
        }
        public async Task<IEnumerable<CarDto>> GetAll()
        {
            var cars = await _dbContext.Cars.ToListAsync();
            if(cars is null)
            {
                return null;
            }
            var carsDtos = cars.Select(c=> new CarDto() 
            {
                Id= c.Id,
                Name= c.Name,
                Class=c.Class,
                Color=c.Color,
                Combustion=c.Combustion,
                Localization=c.Localization,
                HorsePower=c.HorsePower,
                Price=c.Price,
            });
            return carsDtos;
        }
       
        //public IEnumerable<CarDto> GetByParams(CarParams carParams)
        //{
        //    var cars = _dbContext.Cars.Where(c => c.Name == carParams.Name)
        //        .Where(c => c.Color == carParams.Color)
        //        .Where(c => c.Price <= carParams.PriceTo)
        //        .Where(c => c.Price >= carParams.PriceFrom)
        //        .Where(c => c.Combustion <= carParams.CombustionTo)
        //        .Where(c => c.Combustion >= carParams.CombustionFrom)
        //        .Where(c => c.HorsePower <= carParams.HorsePowerTo)
        //        .Where(c => c.HorsePower >= carParams.HorsePowerTo).ToList();

        //    var carsDto = cars.Select(c => new CarDto()
        //    {
        //        Id = c.Id,
        //        Name = c.Name,
        //        Color = c.Color,
        //        Price = c.Price,
        //        Combustion = c.Combustion,
        //        Localization = c.Localization,
        //        Class = c.Class,
        //        HorsePower = c.HorsePower,
        //    }).ToList();

        //    if (carParams.PriceSort == "descending")
        //        carsDto.Sort();
        //    else
        //        carsDto.Reverse();
        //    return carsDto;
        //}
    }
}
