using Infrastructure.DbContexts;
using Domain.Entites;

namespace Infrastructure.Seeders
{
    public class CarRentSeeder
    {
        private readonly CarRentDbContext _dbContext;
        public CarRentSeeder(CarRentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.CarRents.Any())
                {
                    var carRents = GetCarRents();
                    _dbContext.CarRents.AddRange(carRents);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name="User"
                },
                new Role()
                {
                    Name="Administrator"
                },
                new Role()
                {
                    Name="Employee"
                },

            };
            return roles;
        }
        private IEnumerable<CarRental> GetCarRents()
        {
            var carRents = new List<CarRental>()
            {
                new CarRental()
                {
                    Cars=new List<Car>()
                    {
                        new Car()
                        {
                            Class="Premium",
                            Name="BWM",
                            Localization="Warszawa",
                            Combustion=9f,
                            HorsePower=300,
                            Price=80000.50f,
                            Color="Gold",

                        },
                         new Car()
                         {
                             Class="Medium",
                             Name="Skoda",
                             Localization="Kraków",
                             Combustion=7f,
                             HorsePower=200,
                             Price=50000.50f,
                             Color="Green",
                         },
                         new Car()
                         {
                             Class="Standard",
                             Name="Opel",
                             Localization="Nowy Sącz",
                             Combustion=6f,
                             HorsePower=100,
                             Price=30000.50f,
                             Color="Silver",
                         },
                         new Car()
                         {
                             Class="Basic",
                             Name="Fiat",
                             Localization="Warszawa",
                             Combustion=5f,
                             HorsePower=80,
                             Price=10000.50f,
                             Color="Red",
                         },
                         new Car()
                         {
                             Class="Basic",
                             Name="Alfa",
                             Localization="Warszawa",
                             Combustion=5f,
                             HorsePower=50,
                             Price=5000.50f,
                             Color="Red",
                         }

                    },
                    

                },

        };
            return carRents;
        }
    }
}
