using Ddd.Taxi.Infrastructure;
using System.Globalization;

namespace Ddd.Taxi.Domain;

public class TaxiOrder : Entity<int>
{
    public TaxiOrder(int id, OrderInfo info, PersonName personName, Address startAddress) : base(id)
    {
        ClientName = personName;
        Start = startAddress;
        OrderInfo = info;
    }

    public PersonName ClientName { get; }
    public Address Start { get; }
    public Address Destination { get; private set; }
    public Driver Driver { get; private set; }
    public OrderInfo OrderInfo { get; }

    public static TaxiOrder CreateOrderWithoutDestination(int id, OrderInfo info, PersonName person, Address address)
    {
        return new TaxiOrder(id, info, person, address);
    }

    public void UpdateDestination(Address address)
    {
        if (string.IsNullOrEmpty(address.Street) || string.IsNullOrEmpty(address.Building))
            throw new InvalidOperationException();

        Destination = new Address(address.Street, address.Building);
    }

    public void AssignDriver(Driver driver)
    {
        if (Driver is not null) throw new InvalidOperationException("This order allready has driver");
        Driver = driver;
        OrderInfo.RecordDriverAssignment();
        OrderInfo.ChangeStatus(TaxiOrderStatus.WaitingCarArrival);
    }

    public void UnassignDriver()
    {
        if (Driver is null) throw new InvalidOperationException("This order allready WaitingForDriver");
        if (OrderInfo.Status is TaxiOrderStatus.InProgress) throw new InvalidOperationException("Невозможно отменить");
        Driver = null;
        OrderInfo.ChangeStatus(TaxiOrderStatus.WaitingForDriver);
    }

    public string GetDriverFullInfo()
    {
        if (OrderInfo.Status == TaxiOrderStatus.WaitingForDriver) return null;
        return string.Join(" ",
            "Id: " + Driver.Id,
            "DriverName: " + FormatName(Driver?.PersonName),
            "Color: " + Driver.Car.CarColor,
            "CarModel: " + Driver.Car.CarModel,
            "PlateNumber: " + Driver.Car.CarPlateNumber);
    }

    public string GetShortOrderInfo()
    {
        return string.Join(" ",
            "OrderId: " + Id,
            "Status: " + OrderInfo.Status,
            "Client: " + FormatName(ClientName),
            "Driver: " + FormatName(Driver?.PersonName),
            "From: " + FormatAddress(Start),
            "To: " + FormatAddress(Destination),
            "LastProgressTime: " + GetLastProgressTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
    }

    public DateTime GetLastProgressTime()
    {
        if (OrderInfo.Status == TaxiOrderStatus.WaitingForDriver) return OrderInfo.CreationTime;
        if (OrderInfo.Status == TaxiOrderStatus.WaitingCarArrival) return OrderInfo.DriverAssignmentTime;
        if (OrderInfo.Status == TaxiOrderStatus.InProgress) return OrderInfo.StartRideTime;
        if (OrderInfo.Status == TaxiOrderStatus.Finished) return OrderInfo.FinishRideTime;
        if (OrderInfo.Status == TaxiOrderStatus.Canceled) return OrderInfo.CancelTime;
        throw new NotSupportedException(OrderInfo.Status.ToString());
    }

    private string FormatName(PersonName personName)
    {
        if (personName is null) return string.Empty;
        return string.Join(" ", new[] { personName.FirstName, personName.LastName }.Where(n => n != null));
    }

    private string FormatAddress(Address address)
    {
        if (address is null) return string.Empty;
        return string.Join(" ", new[] { address.Street, address.Building }.Where(n => n != null));
    }

    public void Cancel()
    {
        if (OrderInfo.Status is TaxiOrderStatus.InProgress) throw new InvalidOperationException("Невозможно отменить");
        OrderInfo.ChangeStatus(TaxiOrderStatus.Canceled);
        OrderInfo.RecordCancellation();
    }

    public void StartRide()
    {
        if (OrderInfo.Status is not TaxiOrderStatus.WaitingCarArrival) throw new InvalidOperationException("Машина ещё не прибыла");
        OrderInfo.ChangeStatus(TaxiOrderStatus.InProgress);
        OrderInfo.RecordRideStart();
    }

    public void FinishRide()
    {
        if (OrderInfo.Status is not TaxiOrderStatus.InProgress) throw new InvalidOperationException("Поездка не начиналась");
        OrderInfo.ChangeStatus(TaxiOrderStatus.Finished);
        OrderInfo.RecordRideFinish();
    }
}

public class TaxiApi : ITaxiApi<TaxiOrder>
{
    private readonly DriversRepository _driversRepo;
    private readonly Func<DateTime> _currentTime;
    private int _idCounter;

    public TaxiApi(DriversRepository driversRepo, Func<DateTime> currentTime)
    {
        _driversRepo = driversRepo;
        _currentTime = currentTime;
    }

    public TaxiOrder CreateOrderWithoutDestination(string firstName, string lastName, string street, string building)
    {
        var person = new PersonName(firstName, lastName);
        var address = new Address(street, building);
        return TaxiOrder.CreateOrderWithoutDestination(_idCounter++, new OrderInfo(_currentTime), person, address);
    }

    public void UpdateDestination(TaxiOrder order, string street, string building)
    {
        var address = new Address(street, building);
        order.UpdateDestination(address);
    }

    public void AssignDriver(TaxiOrder order, int driverId)
    {
        var driver = _driversRepo.GetDriverToOrder(driverId);
        order.AssignDriver(driver);
    }

    public void UnassignDriver(TaxiOrder order)
    {
        order.UnassignDriver();
    }

    public string GetDriverFullInfo(TaxiOrder order)
    {
        return order.GetDriverFullInfo();
    }

    public string GetShortOrderInfo(TaxiOrder order)
    {
        return order.GetShortOrderInfo();
    }

    private DateTime GetLastProgressTime(TaxiOrder order)
    {
        return order.GetLastProgressTime();
    }

    public void Cancel(TaxiOrder order)
    {
        order.Cancel();
    }

    public void StartRide(TaxiOrder order)
    {
        order.StartRide();
    }

    public void FinishRide(TaxiOrder order)
    {
        order.FinishRide();
    }
}