using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Application.Requests;
using Application.Services;
using Application.Methods.Commands;
using MediatR;

namespace UI.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IMediator _mediator;
        public ReservationController(IReservationService reservationService, IMediator mediator)
        {
            _reservationService = reservationService;
            _mediator = mediator;
        }
        [HttpPost("reserve")]
        public async Task<ActionResult> Reservation([FromBody] ReservationParams request)
        {
            var result = new Reserve(request.CarsId, request.Name, request.LastName, request.Address, request.PostalCode, request.Phone, request.Email, request.From, request.To);
            return Ok(await _mediator.Send(result));
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> CarReturn([FromRoute] int id)
        {
            var returnedSuccessfully = new CarReturn(id);
            return Ok(await _mediator.Send(returnedSuccessfully));
        }
    }
}
