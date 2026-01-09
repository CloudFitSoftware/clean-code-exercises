using System;

namespace Shipping;

public sealed class ShippingRequest
{
    public ShippingSpeed Speed { get; }
    public Region Region { get; }
    public decimal WeightKg { get; }
    public DateTime ShipDate { get; }

    public ShippingRequest(
        ShippingSpeed speed,
        Region region,
        decimal weightKg,
        DateTime shipDate)
    {
        Speed = speed;
        Region = region;
        WeightKg = weightKg;
        ShipDate = shipDate;
    }
}
