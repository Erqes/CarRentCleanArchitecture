using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Methods.Commands
{
    public record RegisterUser(string Email, string Password, string Name,
        string LastName,string Phone, string Address, string City,
        string PostalCode, int RoleId):IRequest<Unit>;
}
