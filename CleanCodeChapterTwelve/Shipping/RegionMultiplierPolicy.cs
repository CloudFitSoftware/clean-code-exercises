
using System;
namespace Shipping;

public sealed class RegionMultiplierPolicy : IShippingRatePolicy
{
    public decimal Apply(decimal current, ShippingRequest r)
    {
        var factor = r.Region == Region.International ? 1.5m : 1.0m;
        var total = decimal.Round(current * factor, 2);
        // Current behavior: weekend surcharge only for International shipments
        if (r.Region == Region.International && (r.ShipDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday))
        {
            total = decimal.Round(total * 1.10m, 2);
        }
        return total;
    }
}
