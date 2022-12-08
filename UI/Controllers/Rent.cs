using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entites;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Application.Services;
using Application.Requests;
using Application.Models;
using MediatR;
using Application.Methods.Queries;

namespace UI.Controllers
{

    [ApiController]
    [Route("rents")]
    public class RentController : ControllerBase
    {
        private readonly ICarRentService _carRentService;
        private readonly IMediator _mediator;
        public RentController(ICarRentService carRentService, IMediator mediator)
        {
            _carRentService = carRentService;
            _mediator = mediator;
        }
        [HttpGet("count")]
        public async Task<object> Count([FromBody] LendParams lendParams)
        {
            var count =new CalculateRentalCost(lendParams.CarClass,lendParams.Km,lendParams.From, lendParams.To, lendParams.DriveLicense);      
            return await _mediator.Send(count);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetAll()
        {
            var query = new GetAllCars();
            var cars = await _mediator.Send(query);
            if (cars == null)
                return NotFound();
            return Ok(cars);
        }
        //[HttpGet("byparams")]
        //public ActionResult<IEnumerable<CarRentDto>> GetByParams([FromBody] CarParams carParams)
        //{
        //    // filtruj po:
        //    // do wyboru kolor, spalanie od-do, cena od-do, moc od-do, nazwa
        //    //sortuj po cenie od nw, od nm
        //    var cars=_carRentService.GetByParams(carParams);
        //    return Ok(cars);
        //}
        
    }
}
