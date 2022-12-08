using System.Collections.Generic;

namespace Application.Models
{
    public class CarRentDto
    {
        public int Id { get; set; }
        public List<CarDto> Cars { get; set; }
    }
}
