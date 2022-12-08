﻿using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Domain.Entites;
using Application.Requests;
using Infrastructure.DbContexts;

namespace Application.Services
{
    public interface IReservationService
    {
        Task<string> Reservation(ReservationParams reservationParams);
        Task<bool> CarReturn(int carId);
    }

    public class ReservationService : IReservationService
    {
        private readonly IConfiguration _configuration;
        private readonly CarRentDbContext _dbContext;
        public ReservationService(IConfiguration configuration, CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public Customer MapCustomer(ReservationParams reservationParams)
        {
            var customer = new Customer();
            if (reservationParams != null)
            {
                customer.Name = reservationParams.Name;
                customer.LastName = reservationParams.LastName;
                customer.Email = reservationParams.Email;
                customer.Phone = reservationParams.Phone;
                customer.Address = reservationParams.Address;
                customer.PostalCode = reservationParams.PostalCode;
            }
            return customer;
        }
        public Rent MapRent(ReservationParams reservationParams)
        {
            var rent = new Rent();
            if (reservationParams != null)
            {
                rent.From = reservationParams.From;
                rent.To = reservationParams.To;
            }
            return rent;
        }
        public async Task<string> Reservation(ReservationParams reservationParams)//dodać uniklanych klientów 
        {
            List<Car> carsToReserve = new List<Car>();
            for (int i = 0; i < reservationParams.CarsId.Count; i++)
            {
                carsToReserve.Add(_dbContext.Cars.FirstOrDefault(c => c.Id == reservationParams.CarsId[i]));
                if (!carsToReserve[i].IsCar)
                    throw new InvalidOperationException();
            }
            var employee = await _dbContext.Employees.OrderBy(e => e.Customers.Count).FirstOrDefaultAsync();
            var customer = MapCustomer(reservationParams);

            customer.EmployeeId = employee.Id;
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            List<Rent> NewRents = new List<Rent>();
            for (int i = 0; i < carsToReserve.Count; i++)
            {

                carsToReserve[i].IsCar = false;
                NewRents.Add(MapRent(reservationParams));
                await _dbContext.Rents.AddAsync(NewRents[i]);
                NewRents[i].CarId = carsToReserve[i].Id;
                NewRents[i].CustomerId = customer.Id;

            }

            await _dbContext.SaveChangesAsync();
            await Send(reservationParams, _configuration.GetSection("EmailUserName").Value);
            await Send(reservationParams, reservationParams.Email);
            return "Zarezerwowano";
        }
        public async Task<bool> CarReturn(int carId)
        {
            var carToReturn =await _dbContext.Rents.FirstOrDefaultAsync(r => r.CarId == carId);
            if (carToReturn is null) { return false; }
            _dbContext.Rents.Remove(carToReturn);
            await _dbContext.SaveChangesAsync();
            var changeCarStatus = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == carId);
            changeCarStatus.IsCar = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task Send(ReservationParams reservationParams, string emailTo)
        {
            var ReservationParamsInfo = reservationParams.ToString();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(emailTo));
            if (email.From == email.To)
            {
                email.Subject = $"Rezerwacja od {reservationParams.Email}";
                email.Body = new TextPart(TextFormat.Html) { Text = $"{ReservationParamsInfo}" };
            }
            else
            {
                email.Subject = $"Rezerwacja Car Rental dokonana.";
                email.Body = new TextPart(TextFormat.Html) { Text = $"Dokonano rezerwacji samochodu." };
            }
            using var smtp = new SmtpClient();
            await Task.Run(() => smtp.Connect(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls));
            await Task.Run(() => smtp.Authenticate(_configuration.GetSection("EmailUserName").Value, _configuration.GetSection("EmailPassword").Value));
            await Task.Run(() => smtp.Send(email));
            await Task.Run(() => smtp.Disconnect(true));
        }
    }
}
