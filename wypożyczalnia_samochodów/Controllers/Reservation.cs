using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Requests;
using Application.Methods.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UI.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReservationController( IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("reserve")]
        [Authorize(Roles ="User")]
        public async Task<ActionResult> Reservation([FromBody] ReservationParams request)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = new Reserve(request.CarsId, userId, request.From, request.To);
            return Ok(await _mediator.Send(result));
        }
        [HttpPut("{Id}")]
        [Authorize(Roles ="Employee")]
        [Authorize(Roles ="Administrator")]
        public async Task<ActionResult> CarReturn([FromRoute] int id)
        {
            var returnedSuccessfully = new CarReturn(id);
            return Ok(await _mediator.Send(returnedSuccessfully));
        }
    }
}
