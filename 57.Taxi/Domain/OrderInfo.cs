using Ddd.Taxi.Infrastructure;

namespace Ddd.Taxi.Domain;
#region

public class OrderInfo : ValueType<OrderInfo>
{
    private readonly Func<DateTime> _currentTime;

    public OrderInfo(Func<DateTime> currentTime)
    {
        _currentTime = currentTime;
        CreationTime = _currentTime();
        Status = TaxiOrderStatus.WaitingForDriver;
    }

    public TaxiOrderStatus Status { get; private set; }
    public DateTime CreationTime { get; }
    public DateTime DriverAssignmentTime { get; private set; }
    public DateTime CancelTime { get; private set; }
    public DateTime StartRideTime { get; private set; }
    public DateTime FinishRideTime { get; private set; }

    public void ChangeStatus(TaxiOrderStatus newStatus)
    {
        Status = newStatus;
    }

    public void RecordDriverAssignment() => DriverAssignmentTime = _currentTime();
    public void RecordCancellation() => CancelTime = _currentTime();
    public void RecordRideStart() => StartRideTime = _currentTime();
    public void RecordRideFinish() => FinishRideTime = _currentTime();
}
