

namespace Application.Requests
{
    public class ReservationParams
    {
        public List<int> CarsId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
