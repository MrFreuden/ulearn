namespace Ddd.Taxi.Domain;

public class DriversRepository
{
    public Driver GetDriverToOrder(int driverId)
    {
        if (driverId == 15)
        {
            var car = new Car("Baklazhan", "Lada sedan", "A123BT 66");
            return new Driver(driverId, "Drive", "Driverson", car);
        }
        else
            throw new Exception("Unknown driver id " + driverId);
    }
}
