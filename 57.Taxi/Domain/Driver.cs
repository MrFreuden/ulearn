using Ddd.Taxi.Infrastructure;

namespace Ddd.Taxi.Domain;

public class Driver : Entity<int>
{
    public Driver(int id, string firstName, string lastName, Car car) : base(id)
    {
        Car = car;
        PersonName = new PersonName(firstName, lastName);
    }

    public Driver(int id, PersonName personName, Car car) : base(id)
    {
        Car = car;
        PersonName = personName;
    }

    public PersonName PersonName { get; }
    public Car Car { get; }
}
