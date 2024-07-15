using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAccounting;
public class AccountingModel : ModelBase
{
    private double _price;
    private int _nightsCount;
    private double _discount;
    private double _total;

    public double Price
    {
        get => _price;
        set
        {
            if (value < 0) throw new ArgumentException("Price cannot be negative.");
            _price = value;
            Notify(nameof(Price));
            UpdateTotal();
        }
    }

    public int NightsCount
    {
        get => _nightsCount;
        set
        {
            if (value <= 0) throw new ArgumentException("NightsCount must be positive.");
            _nightsCount = value;
            Notify(nameof(NightsCount));
            UpdateTotal();
        }
    }

    public double Discount
    {
        get => _discount;
        set
        {
            if (value < 0 || value > 100) throw new ArgumentException("Discount must be between 0 and 100.");
            _discount = value;
            Notify(nameof(Discount));
            UpdateTotal();
        }
    }

    public double Total
    {
        get => _total;
        set
        {
            if (value < 0) throw new ArgumentException("Total cannot be negative.");
            _total = value;
            Notify(nameof(Total));
            UpdateDiscountBasedOnTotal();
        }
    }

    private void UpdateTotal()
    {
        _total = _price * _nightsCount * (1 - _discount / 100);
        Notify(nameof(Total));
    }

    private void UpdateDiscountBasedOnTotal()
    {
        if (_price * _nightsCount != 0)
        {
            _discount = (1 - _total / (_price * _nightsCount)) * 100;
            Notify(nameof(Discount));
        }
        else
        {
            throw new InvalidOperationException("Cannot calculate discount when Price or NightsCount is zero.");
        }
    }
}
