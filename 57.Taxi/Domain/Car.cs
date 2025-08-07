using Ddd.Taxi.Infrastructure;

namespace Ddd.Taxi.Domain;

public class Car : ValueType<Car>
{
    public Car(string carColor, string carModel, string carPlateNumber)
    {
        CarColor = carColor;
        CarModel = carModel;
        CarPlateNumber = carPlateNumber;
    }

    public string CarColor { get; }
    public string CarModel { get; }
    public string CarPlateNumber { get; }
}
