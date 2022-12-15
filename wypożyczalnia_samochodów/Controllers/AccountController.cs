using Application.Models;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Infrastructure.Methods.Commands;
using System.Threading.Tasks;

namespace UI.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public AccountController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto dto)
        {
            var newUser=new RegisterUser(dto.Email, dto.Password, dto.Name, dto.LastName, dto.Phone, dto.Address, dto.City, dto.PostalCode, dto.RoleId);
            return Ok(await _mediatR.Send(newUser));
        }
        [HttpPost("employee")]
        public async Task<ActionResult> LoginEmployee([FromBody] LoginDto dto)
        {
            var loginDto = new EmployeeToken(dto.Email, dto.Password);
            return Ok(await _mediatR.Send(loginDto));
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            var loginDto=new GenerateToken(dto.Email, dto.Password);
            return Ok( await _mediatR.Send(loginDto));
        }
    }
}
