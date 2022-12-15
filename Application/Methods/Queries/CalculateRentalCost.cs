
using MediatR;
using System.ComponentModel;
using Domain.Entites;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace Application.Methods.Queries
{
    public class CalculateRentalCost : IRequest<string>
    {
        public CarType CarClass { get; set; }
        public float Km { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime DriveLicense { get; set; }

        public CalculateRentalCost(CarType CarClass, float Km, DateTime From, DateTime To, DateTime DriveLicense)
        {
            this.CarClass = CarClass;
            this.Km = Km;
            this.From = From;
            this.To = To;
            this.DriveLicense = DriveLicense;
        }

    }
}
