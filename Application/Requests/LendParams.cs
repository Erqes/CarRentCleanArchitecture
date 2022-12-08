using Domain.Enums;

namespace Application.Requests
{
    public class LendParams
    {
        public CarType CarClass { get; set; }
        public float Km { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime DriveLicense { get; set; }
    }
}
