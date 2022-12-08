
namespace Domain.Entites
{
    public class CarRental
    {
        public int Id { get; set; }
        public virtual List<Car> Cars { get; set; }
        public virtual List<Employee> Employees { get; set; }
    }
}
