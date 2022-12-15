using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Methods.Commands
{
    public record EmployeeToken(string email, string password) : IRequest<string>;
}
