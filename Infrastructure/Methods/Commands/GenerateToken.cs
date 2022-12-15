using MediatR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Methods.Commands
{
    public record GenerateToken(string email, string password):IRequest<string>;
}
