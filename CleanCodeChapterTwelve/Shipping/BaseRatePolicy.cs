namespace Shipping;

public sealed class BaseRatePolicy : IShippingRatePolicy
{
    public decimal Apply(decimal current, ShippingRequest r) => current + (r.Speed switch
    {
        ShippingSpeed.Standard => 5m,
        ShippingSpeed.Express => 10m,
        ShippingSpeed.Overnight => 20m,
        _ => 5m
    });
}
