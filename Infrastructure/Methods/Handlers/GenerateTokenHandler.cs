using Infrastructure.DbContexts;
using Infrastructure.Methods.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Domain.Entites;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Methods.Handlers
{
    public class GenerateTokenHandler:IRequestHandler<GenerateToken, string>
    {
        private readonly CarRentDbContext _carRentDbContext;
        private readonly IPasswordHasher<Customer> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public GenerateTokenHandler(CarRentDbContext carRentDbContext, IPasswordHasher<Customer> passwordHasher,AuthenticationSettings authenticationSettings)
        {
            _carRentDbContext = carRentDbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public async Task<string> Handle(GenerateToken request,CancellationToken cancellationToken)
        {
            var user = await _carRentDbContext.Customers
                .Include(u=>u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.email);
         
            if (user == null)
            {
                throw new BadRequestException("Invalid username or password");
            }
            var result= _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.password);
            if(result==PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.LastName}"),
                new Claim(ClaimTypes.Role,$"{user.Role.Name}"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred=new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer, claims,
                expires: expires, 
                signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
