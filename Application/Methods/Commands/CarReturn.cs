
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Methods.Commands
{
    public record CarReturn(int Id) : IRequest<Unit>;
  
   
}
