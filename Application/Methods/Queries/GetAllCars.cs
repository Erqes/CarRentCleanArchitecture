using Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Methods.Queries
{
    public class GetAllCars : IRequest<IEnumerable<CarDto>>
    {
    }
   

}
