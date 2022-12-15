using MediatR;


namespace Application.Methods.Commands
{
    public record Reserve(List<int> carsId, int customerId, DateTime from, DateTime to) : IRequest<Unit>;
    

}

